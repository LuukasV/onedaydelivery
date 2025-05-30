using UnityEngine;

/// <summary>
/// Programmer: Jari-Pekka Riihinen
/// Simple class for breaking windows. Windows have intact and broken variant. After contact the window will be replaced by the broken variant.
/// </summary>
public class BreakableWindow : MonoBehaviour
{
    public GameObject brokenWindow; // Broken variant of the window
    public AudioClip breakSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Mailable")) // Checks if box hits the window
        {
            BreakWindow();
        }
    }

    /// <summary>
    /// Breaks the window by replacing it with the brokenWindow GameObject
    /// </summary>
    void BreakWindow()
    {
        Vector3 spawnPosition = transform.position;
        spawnPosition.y += brokenWindow.transform.localScale.y - 2.5f;

        GameObject broken = Instantiate(brokenWindow, spawnPosition, transform.rotation);
        Destroy(gameObject);

        if (breakSound != null)
        {
            float volume = 0.5f;
            AudioSource.PlayClipAtPoint(breakSound, transform.position, volume);
        }

        Destroy(broken, 10f);
    }
}