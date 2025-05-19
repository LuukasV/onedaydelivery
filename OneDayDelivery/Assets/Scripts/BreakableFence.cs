using UnityEngine;

/// <summary>
/// Simple script for wooden fence asset. The fence will break if player or package touches it
/// </summary>
public class BreakableFence : MonoBehaviour
{
    public AudioClip breakSound;

    private bool isBroken = false;

    /// <summary>
    /// Checks if package or player touches the fence. Calls BreakFence method with the impact point vector.
    /// </summary>
    /// <param name="other">What comes in contact with the collider</param>
    private void OnTriggerEnter(Collider other)
    {
        if (isBroken) return;

        if (other.CompareTag("Mailable") || other.CompareTag("Player"))
        {
            Vector3 impactPoint = other.ClosestPointOnBounds(transform.position);
            BreakFence(impactPoint);
        }
    }

    /// <summary>
    /// Breaks the fence by making wooden planks loose and making them fly with force
    /// </summary>
    /// <param name="impactPoint">Vector3 for the point of contact</param>
    void BreakFence(Vector3 impactPoint)
    {
        isBroken = true;

        Rigidbody[] fragments = GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in fragments)
        {
            rb.isKinematic = false;
            rb.useGravity = true;

            Vector3 pushDir = (rb.transform.position - impactPoint).normalized;
            float force = 250f;
            rb.AddForce(pushDir * force);
        }

        if (breakSound != null)
        {
            AudioSource.PlayClipAtPoint(breakSound, transform.position, 0.6f);
        }
    }
}
