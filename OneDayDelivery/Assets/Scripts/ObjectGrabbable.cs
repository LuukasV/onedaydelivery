using Unity.VisualScripting;
using UnityEngine;

// This class determines functionality to objects that the player can grab and move.
public class ObjectGrabbable : MonoBehaviour
{
    private Rigidbody objectRigidBody;
    private Transform objectGrabPointTransform;

    private void Awake()
    {
        objectRigidBody = GetComponent<Rigidbody>();
    }

    // Removes gravity from grabbed objects. 
    // parameter objectGrabPointTransfrom sets position data to player's grab point.
    public void Grab(Transform objectGrabPointTransfrom)
    {
        this.objectGrabPointTransform = objectGrabPointTransfrom;
        objectRigidBody.useGravity = false;
    }
    
    // Drops an object that player is holding.
    public void Drop()
    {
        this.objectGrabPointTransform = null;
        objectRigidBody.useGravity = true;
    }

    // Throws an object that the player is holding.
    // throwForce is a parameter that sets strenght of throw
    // direction sets the Vector3 direction of throw.
    public void Throw(float throwForce, Vector3 direction)
    {
        objectRigidBody.AddForce(direction * throwForce);
    }

    public void Pocket()
    {
        //this.objectGrabPointTransform = objectGrabPointTransfrom;
    }

    // Moves objects that have been grabbed. Lerp adds smoothness to movement.
    private void FixedUpdate()
    {
        if (objectGrabPointTransform != null)
        {
            float lerpSpeed = 10f;
            Vector3 newPosition = Vector3.Lerp(transform.position, objectGrabPointTransform.position, Time.deltaTime * lerpSpeed);
            objectRigidBody.MovePosition(newPosition);
        }
    }
}
