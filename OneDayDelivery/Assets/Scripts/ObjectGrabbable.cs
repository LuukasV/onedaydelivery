using UnityEngine;
using Utils.Physics;

/// <summary>
/// Main programmer: Jussi Kolehmainen
/// Other progammers: Luukas Vuolle
/// Majority of the programming was done by the main programmer. Other programmer added MailFinisher method.
/// 
/// This class determines functionality to objects that the player can grab and move.
/// </summary>
public class ObjectGrabbable : MonoBehaviour
{
    /// Variables for object grabbing and throwing
    private Rigidbody objectRigidBody;
    private Transform objectGrabPointTransform;
    private RigidbodyStorer rigidbodyStorer;

    /// <summary>
    /// Sets the rigidbody and rigidbody storer variables at game start.
    /// </summary>
    private void Awake()
    {
        objectRigidBody = GetComponent<Rigidbody>();
        rigidbodyStorer = new RigidbodyStorer();
    }

    /// <summary>
    /// Gravity from grabbed objects and sets the position data to the player's grab point.
    /// </summary>
    /// <param name="objectGrabPointTransform">The transform of the grab point.</param>
    /// <param name="parent">The parent transform to set.</param>
    public void Grab(Transform objectGrabPointTransfrom, Transform parent)
    {
        this.objectGrabPointTransform = objectGrabPointTransfrom;

        rigidbodyStorer.StoreValues(objectRigidBody);
        Destroy(objectRigidBody);
        transform.SetParent(parent);
    }

    /// <summary>
    /// Destroys the rigidbody of the object when it is no longer needed.
    /// </summary>
    public void MailFinisher()
    {
        Destroy(objectRigidBody);
    }

    /// <summary>
    /// Drops an object that player is holding.
    /// </summary>
    public void Drop()
    {
        this.objectGrabPointTransform = null;
        if (objectRigidBody == null)
        {
            objectRigidBody = gameObject.AddComponent<Rigidbody>();
            rigidbodyStorer.CopyValues(objectRigidBody);
        }
        transform.SetParent(null);
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

    /// <summary>
    /// Moves objects that have been grabbed. Lerp adds smoothness to movement.
    /// </summary>
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
