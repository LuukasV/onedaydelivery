using UnityEngine;

public class BreakableWindow : MonoBehaviour
{
    public GameObject brokenWindow;
    public AudioClip breakSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Mailable")) // Checks if box hits the window
        {
            BreakWindow();
        }
    }

    void BreakWindow()
    {
        Vector3 spawnPosition = transform.position;
        spawnPosition.y += brokenWindow.transform.localScale.y - 2.5f;

        Instantiate(brokenWindow, spawnPosition, transform.rotation);
        Destroy(gameObject);

        if (breakSound != null)
        {
            AudioSource.PlayClipAtPoint(breakSound, transform.position);
        }
    }
}
