using UnityEngine;

public class BreakableRuukku : MonoBehaviour
{
    public GameObject brokenVase;
    public AudioClip breakSound;

    //set minPitch and maxPitch for sound effect
    public float minPitch = 0.9f;
    public float maxPitch = 1.1f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Mailable")) // Checks if box hits the vase
        {
            BreakVase();
        }
    }

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

    //create audiosource for playing the break sound
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
