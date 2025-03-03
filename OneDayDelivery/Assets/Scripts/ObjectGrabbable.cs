using Unity.VisualScripting;
using UnityEngine;
using Utils.Physics;

// This class determines functionality to objects that the player can grab and move.
public class ObjectGrabbable : MonoBehaviour
{
    private Rigidbody objectRigidBody;
    private Transform objectGrabPointTransform;

    private RigidbodyStorer rigidbodyStorer;


    private void Awake()
    {
        objectRigidBody = GetComponent<Rigidbody>();
        rigidbodyStorer = new RigidbodyStorer();
    }

    /// <summary>
    ///gravity from grabbed objects and sets the position data to the player's grab point.
    /// </summary>
    /// <param name="objectGrabPointTransform">The transform of the grab point.</param>
    /// <param name="parent">The parent transform to set.</param>
    public void Grab(Transform objectGrabPointTransfrom, Transform parent)
    {
        this.objectGrabPointTransform = objectGrabPointTransfrom;
        //objectRigidBody.useGravity = false;

        rigidbodyStorer.StoreValues(objectRigidBody);
        Destroy(objectRigidBody);
        transform.SetParent(parent);
    }

    public void MailFinisher()
    {
        Destroy(objectRigidBody);
    }

    // Drops an object that player is holding.
    public void Drop()
    {
        this.objectGrabPointTransform = null;
        if (objectRigidBody == null)
        {
            objectRigidBody = gameObject.AddComponent<Rigidbody>();
            rigidbodyStorer.CopyValues(objectRigidBody);
        }
        transform.SetParent(null);

        //objectRigidBody.useGravity = true;
        //objectRigidBody.isKinematic = false;
    }

    /// <summary>
    /// Throws the object that the player is holding.
    /// </summary>
    /// <param name="throwForce">The strength of the throw.</param>
    /// <param name="direction">The direction of the throw.</param>
    public void Throw(float throwForce, Vector3 direction)
    {
        objectRigidBody.isKinematic = false;
        objectRigidBody.useGravity = true;

        objectRigidBody.AddForce(direction * throwForce);
    }


    // Moves objects that have been grabbed. Lerp adds smoothness to movement.
    private void FixedUpdate()
    {
        if (objectGrabPointTransform != null)
        {
            float lerpSpeed = 10f;
            Vector3 newPosition = Vector3.Lerp(transform.position, objectGrabPointTransform.position, Time.fixedDeltaTime * lerpSpeed);
            transform.position = newPosition;

            Quaternion newRotation = Quaternion.Lerp(transform.rotation, objectGrabPointTransform.rotation, Time.fixedDeltaTime * lerpSpeed);
            transform.rotation = newRotation;
        }
    }
}
