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
        Instantiate(brokenVase, transform.position, transform.rotation);
        Destroy(gameObject);

        if (breakSound != null)
        {
            float volume = 0.5f;
            AudioSource.PlayClipAtPoint(breakSound, transform.position, volume);
        }
    }
}
