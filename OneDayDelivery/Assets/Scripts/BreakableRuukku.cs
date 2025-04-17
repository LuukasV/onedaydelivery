using UnityEngine;

public class BreakableRuukku : MonoBehaviour
{
    public GameObject brokenVase;
    public AudioClip breakSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Mailable")) // Checks if box hits the vase
        {
            BreakVase();
        }
    }

    void BreakVase()
    {
        GameObject broken = Instantiate(brokenVase, transform.position, transform.rotation);
        Destroy(gameObject);

        if (breakSound != null)
        {
            float volume = 0.5f;
            AudioSource.PlayClipAtPoint(breakSound, transform.position, volume);
        }

        // Adds small explotion force to shards
        Rigidbody[] pieces = broken.GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in pieces)
        {
            rb.AddExplosionForce(150f, transform.position, 2f);
        }

        Destroy(broken, 10f); // Remove shards from the game to prevent lag
    }
}
