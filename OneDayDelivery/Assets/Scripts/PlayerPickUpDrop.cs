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


    void Start()
    {

        speedChanger = playerParentBody.GetComponent<PlayerMovement>();
        indexInventory = 0;
        inventory = new ObjectGrabbable[sizeOfInventory];
        pickUpDistance = 4f;

        //UIManager script is linked with the PlayerUI component
        uiManager = GameObject.FindWithTag("PlayerUI").GetComponent<UIManager>();
    }


    public void objectHandlerNuller()
    {
        objectGrabbable = null;
    }

    private void Update()
    {
        //Debug.Log("WITHIN UPDATE: " + objectGrabbable);

        // Grab or drop object. Depending if player is holding something.
        if (Input.GetKeyDown(pickUp))
        {
            if (objectGrabbable == null)
            {
                // Not carrying an object, try to grab.
                if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycastHit, pickUpDistance, pickUpLayerMask))
                {
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

        else if (Input.GetKeyDown(intoPocket) && indexInventory < sizeOfInventory)
        {

            if (objectGrabbable!= null)
            {
                inventory[indexInventory] = objectGrabbable;
                objectGrabbable.gameObject.SetActive(false);
                indexInventory++;
                speedChanger.moveSpeed -= speedChangeOfInventory;
                objectGrabbable = null;
                uiManager.AddPointToBackpackScore();
            }

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
        // Alternative method for key input: (Input.GetKeyDown(KeyCode.Y)
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


    public void throwForceBooster(float incrAmount)
    {
        throwForce += incrAmount;
    }

    public void packageThievery(AI_DogSimple collidedDog)
    {
        // Npc steals a package from the player's hands, not from inventory. Then turns and throws it away.
        if (objectGrabbable != null)
        {
            //Debug.Log("Player has package in hand.");
            objectGrabbable.Grab(collidedDog.dogGrabPoint, collidedDog.transform);
            collidedDog.runAwayAndThrow(objectGrabbable);
            objectGrabbable = null;
        }

        // Npc steals a package from the player's inventory. Then turns and throws it away.
        else if (indexInventory != 0)
        {
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
        }
    }
}