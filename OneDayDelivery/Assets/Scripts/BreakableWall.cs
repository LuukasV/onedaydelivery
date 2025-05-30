using UnityEngine;

/// <summary>
/// Programmer: Jari-Pekka Riihinen
/// Script for breaking corcrete walls. The walls can be destroyed by packages or by player
/// </summary>
public class BreakableWall : MonoBehaviour
{
    public AudioClip breakSound;

    private bool isBroken = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isBroken) return;

        if (other.CompareTag("Mailable") || other.CompareTag("Player"))
        {
            Vector3 impactPoint = other.ClosestPointOnBounds(transform.position);
            BreakWall(impactPoint);
        }
    }

    /// <summary>
    /// Breaks the wall by making parts from the wall loose. Then adding some force to push pieces from the impactPoint
    /// </summary>
    /// <param name="impactPoint">What part of the wall got hit</param>
    void BreakWall(Vector3 impactPoint)
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
