using UnityEngine;

/// <summary>
/// Script for breaking 3D object Vase. Script replaces intact vase with the broken variant with loose shards
/// </summary>
public class BreakableRuukku : MonoBehaviour
{
    public GameObject brokenVase; // Broken variant of the vase
    public AudioClip breakSound;

    //set minPitch and maxPitch for sound effect
    public float minPitch = 0.9f;
    public float maxPitch = 1.1f;

    /// <summary>
    /// Collider box for the vase. If Collider other with the correct tag touches the collider box, BreakVase() method will be called
    /// </summary>
    /// <param name="other">What comes in contact with the collider</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Mailable")) // Checks if box hits the vase
        {
            BreakVase();
        }
    }

    /// <summary>
    /// Breaks the vase by replacing it with the broken variant
    /// </summary>
    void BreakVase()
    {
        if (breakSound != null)
        {
            //set random pitch
            float randomPitch = Random.Range(minPitch, maxPitch);
            //play sound
            PlayClipAtPointWithPitch(breakSound, transform.position, randomPitch);
        }

        GameObject broken = Instantiate(brokenVase, transform.position, transform.rotation);
        Destroy(gameObject);

        // Adds small explotion force to shards
        Rigidbody[] pieces = broken.GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in pieces)
        {
            rb.AddExplosionForce(150f, transform.position, 2f);
        }

        Destroy(broken, 10f); // Remove shards from the game to prevent lag
    }

    /// <summary>
    /// Create audiosource for playing the break sound
    /// </summary>
    /// <param name="clip">Audio Clip</param>
    /// <param name="position">Where the audio will be played</param>
    /// <param name="pitch">Random pitch for the break sound</param>
    /// <param name="volume">Volume for the audio clip</param>
    public void PlayClipAtPointWithPitch(AudioClip clip, Vector3 position, float pitch = 1f, float volume = 0.5f)
    {
        GameObject tempGO = new GameObject("TempAudio");
        tempGO.transform.position = position;

        AudioSource source = tempGO.AddComponent<AudioSource>();
        source.clip = clip;
        source.pitch = pitch;
        source.volume = volume;
        source.spatialBlend = 1f; // 3D sound
        source.Play();

        Destroy(tempGO, clip.length / pitch); // Adjust for pitch speed
    }
}
