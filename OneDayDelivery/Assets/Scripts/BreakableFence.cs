using UnityEngine;

public class BreakableFence : MonoBehaviour
{
    public AudioClip breakSound;

    private bool isBroken = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isBroken) return;

        if (other.CompareTag("Mailable") || other.CompareTag("Player"))
        {
            Vector3 impactPoint = other.ClosestPointOnBounds(transform.position);
            BreakFence(impactPoint);
        }
    }

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
