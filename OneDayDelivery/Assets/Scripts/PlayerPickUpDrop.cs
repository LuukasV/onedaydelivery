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
    [SerializeField] private float throwForce;
    
    [Header("Keybinds")]
    public KeyCode throwKey = KeyCode.F;
    public KeyCode intoPocket = KeyCode.R;
    public KeyCode outOfPocket = KeyCode.Y;
    public KeyCode pickUp = KeyCode.E;

    private ObjectGrabbable objectGrabbable;
    private int packageCarry;
    [SerializeField] private GameObject packagePrefab;

    private PlayerMovement speedChanger;

    void Start()
    {
        speedChanger = playerParentBody.GetComponent<PlayerMovement>();
        packageCarry = 0;
    }

    private void Update()
    {
        // Grab or drop object. Depending if player is holding something.
        if (Input.GetKeyDown(pickUp))
        {
            if (objectGrabbable == null)
            {
                // Not carrying an object, try to grab.
                float pickUpDistance = 4f;
                if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycastHit, pickUpDistance, pickUpLayerMask))
                {
                    if (raycastHit.transform.TryGetComponent(out objectGrabbable))
                    {
                        objectGrabbable.Grab(objectGrabPointTransform);
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
            objectGrabbable.Throw(throwForce, throwDirection);
            objectGrabbable.Drop();
            objectGrabbable = null;
        }

        // Put it in inventory
        else if (Input.GetKeyDown(intoPocket) && objectGrabbable != null)
        {
            // Alterantive method for inventory system? 
            //objectGrabbable.gameObject.SetActive(false);

            speedChanger.moveSpeed -= 2;
            Destroy(objectGrabbable.gameObject);
            objectGrabbable = null;
            packageCarry +=1;
        }

        // Get packages out of inventory
        // Alternative method for key input: (Input.GetKeyDown(KeyCode.Y)
        else if (Input.GetKeyDown(outOfPocket) && objectGrabbable == null && packageCarry > 0)
        {
            // Alterantive method for inventory system? 
            //objectGrabbable.gameObject.SetActive(true);

            packageCarry -=1;
            speedChanger.moveSpeed += 2;

            // Creates a new package into players hand.
            GameObject newPackage =  Instantiate(packagePrefab, objectGrabPointTransform.position, Quaternion.identity);

            objectGrabbable = newPackage.GetComponent<ObjectGrabbable>();
            if (objectGrabbable != null)
            {
                // Grab the newly instantiated object
                objectGrabbable.Grab(objectGrabPointTransform);
            }
        }
    }
}