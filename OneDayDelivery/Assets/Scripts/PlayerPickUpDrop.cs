using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// Mechanics for handling objects: pickup, throw, put in inventory etc.
public class PlayerPickUpDrop : MonoBehaviour
{
    [SerializeField] private Transform playerParentBody;
    [SerializeField] private Transform playerCameraTransform;
    [SerializeField] private Transform objectGrabPointTransform;
    [SerializeField] private LayerMask pickUpLayerMask;
    [SerializeField] private Transform parent;

    private ObjectGrabbable objectGrabbable;

    private UIManager uiManager;

    private PlayerMovement speedChanger;
    private ObjectGrabbable[] inventory;
    private int indexInventory;
    Transform parentTransform;

    [Header("Changable values for play testing")]
    [SerializeField] private int sizeOfInventory;
    [SerializeField] private int speedChangeOfInventory;
    [SerializeField] private float throwForce;
    [SerializeField] private float pickUpDistance;


    [Header("Keybinds")]
    public KeyCode throwKey = KeyCode.F;
    public KeyCode intoPocket = KeyCode.R;
    public KeyCode outOfPocket = KeyCode.Y;
    public KeyCode pickUp = KeyCode.E;

    [Header("Prefab of object that is conjured in PackageZone")]
    public GameObject packagePrefab;

    private bool withinPackageZone = false; //if bool is true (toggled in different code), player is allowed to conjure packages out of thin air
    public AudioClip dogAlert;
    public AudioClip dogPush;

    /// <summary>
    /// Set necessary variables once.
    /// </summary>
    void Start()
    {

        speedChanger = playerParentBody.GetComponent<PlayerMovement>();
        indexInventory = 0;
        inventory = new ObjectGrabbable[sizeOfInventory];
        pickUpDistance = 4f;

        //UIManager script is linked with the PlayerUI component
        uiManager = GameObject.FindWithTag("PlayerUI").GetComponent<UIManager>();
    }

    /// <summary>
    /// Set object in hand to null.
    /// </summary>
    public void objectHandlerNuller()
    {
        objectGrabbable = null;
    }

    /// <summary>
    /// Toggles PackageZoneToggle, whitch indicates if the player is within package area
    /// </summary>
    public void TogglePackageZone(bool active)
    {
        // Debug.Log("PackageZone is: " + active);
        withinPackageZone = active;
    }

    /// <summary>
    /// Player control inputs are detected in Update.
    /// </summary>
    private void Update()
    {
        // Grab or drop object. Depending if player is holding something.
        if (Input.GetKeyDown(pickUp))
        {
            if (objectGrabbable == null)
            {
                // Not carrying an object, try to grab.
                if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycastHit, pickUpDistance, pickUpLayerMask))
                {
                    //Debuging logs if necessary.
                    //Debug.Log("Raycast hit: " + raycastHit.transform.name);
                    //Debug.Log("Raycast hit object with tag: " + raycastHit.transform.tag);
                    //Debug.Log("Raycast hit collider with tag: " + raycastHit.collider.transform.tag);

                    if (raycastHit.collider.transform.CompareTag("pickupCollider"))
                    {
                        parentTransform = raycastHit.collider.transform.parent;
                        objectGrabbable = parentTransform.GetComponent<ObjectGrabbable>();
                        objectGrabbable.Grab(objectGrabPointTransform, parent);
                    }
                }
            }

            // Currently carrying something, drop
            else
            {
                objectGrabbable.Drop();
                objectGrabbable = null;
            }
        }

        // Throw package
        else if (Input.GetKeyDown(throwKey) && objectGrabbable != null)
        {
            Vector3 throwDirection = objectGrabPointTransform.forward;
            objectGrabbable.Drop();
            objectGrabbable.Throw(throwForce, throwDirection);
            objectGrabbable = null;
        }

        // Put package into inventory, if there is space.
        else if (Input.GetKeyDown(intoPocket) && indexInventory < sizeOfInventory)
        {
            // Package from hands into inventroy
            if (objectGrabbable!= null)
            {
                inventory[indexInventory] = objectGrabbable;
                objectGrabbable.gameObject.SetActive(false);
                indexInventory++;
                speedChanger.moveSpeed -= speedChangeOfInventory;
                objectGrabbable = null;
                uiManager.AddPointToBackpackScore();
            }

            //Conjure a package into inventory if within Package Zone
            else if (withinPackageZone)
            {
                GameObject conjuredPackage = Instantiate(packagePrefab);
                conjuredPackage.SetActive(false);
                inventory[indexInventory] = conjuredPackage.GetComponent<ObjectGrabbable>();
                indexInventory++;
                uiManager.AddPointToBackpackScore();
            }

            // Package from ground into inventroy
            else if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycastHit, pickUpDistance, pickUpLayerMask))
            {

                if (raycastHit.collider.transform.CompareTag("pickupCollider"))
                {
                    parentTransform = raycastHit.collider.transform.parent;
                    objectGrabbable = parentTransform.GetComponent<ObjectGrabbable>();
                    inventory[indexInventory] = objectGrabbable;
                    objectGrabbable.gameObject.SetActive(false);
                    indexInventory++;
                    speedChanger.moveSpeed -= speedChangeOfInventory;
                    uiManager.AddPointToBackpackScore();
                    objectGrabbable = null;
                }
            }
        }


        // Get packages out of inventory
        else if (Input.GetKeyDown(outOfPocket) && objectGrabbable == null)
        {
            if (0 < indexInventory)
            {
                objectGrabbable = inventory[indexInventory-1].GetComponent<ObjectGrabbable>();
                objectGrabbable.transform.position = objectGrabPointTransform.transform.position;
                objectGrabbable.transform.rotation = Quaternion.Euler(0, 0, 0);
                objectGrabbable.gameObject.SetActive(true);
                objectGrabbable.Grab(objectGrabPointTransform, parent);
                indexInventory--;
                speedChanger.moveSpeed += speedChangeOfInventory;

                uiManager.RemovePointFromBackpackScore();
            }
        }
    }

    /// <summary>
    /// Increase player throwForce.
    /// </summary>
    public void throwForceBooster(float incrAmount)
    {
        throwForce += incrAmount;
    }

    /// <summary>
    /// A method used for removing packages from player. Checks if the collided NPC dog can steal a package or not. If it can, run other methods.Otherwise return.
    /// </summary>
    /// <param name="collidedDog"> NPC that collided with player </param>
    public void packageThievery(AI_DogSimple collidedDog)
    {
        if (!collidedDog.CanSteal()) return;

        // Npc steals a package from the player's hands, not from inventory. Then turns and throws it away.
        if (objectGrabbable != null)
        {
            //Debugging logs if necessary.
            //Debug.Log("Player has package in hand.");

            objectGrabbable.Grab(collidedDog.dogGrabPoint, collidedDog.transform);
            collidedDog.runAwayAndThrow(objectGrabbable);
            objectGrabbable = null;

            AudioSource.PlayClipAtPoint(dogAlert, collidedDog.transform.position, 0.8f); //play dog alert sound
            AudioSource.PlayClipAtPoint(dogPush, collidedDog.transform.position, 0.8f); //play dog push sound
        }

        // Npc steals a package from the player's inventory. Then turns and throws it away.
        else if (indexInventory != 0)
        {
            //Debugging logs if necessary.
            //Debug.Log("Player has package in inventory. Transform info from dog:" + collidedDog.transform.position);

            objectGrabbable = inventory[indexInventory-1].GetComponent<ObjectGrabbable>();
            objectGrabbable.transform.position = collidedDog.dogGrabPoint.position;
            objectGrabbable.transform.rotation = collidedDog.dogGrabPoint.rotation;
            objectGrabbable.gameObject.SetActive(true);
            objectGrabbable.Grab(collidedDog.dogGrabPoint, collidedDog.transform);
            indexInventory--;
            speedChanger.moveSpeed += speedChangeOfInventory;

            collidedDog.runAwayAndThrow(objectGrabbable);
            objectGrabbable = null;

            uiManager.RemovePointFromBackpackScore();

            AudioSource.PlayClipAtPoint(dogAlert, collidedDog.transform.position, 0.8f); //play dog alert sound
            AudioSource.PlayClipAtPoint(dogPush, collidedDog.transform.position, 0.8f); //play dog push sound
        }
    }
}