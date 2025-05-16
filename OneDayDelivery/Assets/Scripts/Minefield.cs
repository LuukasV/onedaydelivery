using UnityEngine;
using System.Collections;

//Minefield acts as a boundary to the level. When activated, the minefield throws the player towards to the center of the level
public class Minefield : MonoBehaviour
{
    private Vector3 resetPosition = new Vector3(300f, 50f, 275f); // Direction, where player is pushed
    public Rigidbody playerRb;   // Player rigidbody component
    public float throwForce = 100f;
    public float upForce = 50f;

    public PlayerMovement playerMovementScript; // Player movement script to disable moving

    [Header("AUDIO")]
    public AudioSource audioSource;

    public AudioClip[] audioClips;

    [Header("Explotion effect")]
    public ParticleSystem explotion;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Debug.Log("Player hit the minefield");

            PlayExplotionSound();

            GameObject explosion = Instantiate(explotion.gameObject, other.transform.position, Quaternion.identity);

            // Play all effects at once
            foreach (ParticleSystem ps in explosion.GetComponentsInChildren<ParticleSystem>())
            {
                ps.Play();
            }

            // Destroy effect after the duration
            Destroy(explosion, 5f);

            if (playerRb != null)
            {
                // Debug.Log("Throwing the player");

                playerMovementScript.canMove = false;
                StartCoroutine(ReenableMovementAfterDelay(2.5f));

                Vector3 direction = (resetPosition - playerRb.transform.position).normalized;
                Vector3 force = direction * throwForce + Vector3.up * upForce;

                playerRb.linearVelocity = Vector3.zero;
                playerRb.AddForce(force, ForceMode.VelocityChange);
            }
            else
            {
                // Debug.LogWarning("playerRb is not set in the inspector!");
                return;
            }
        }
    }

    private IEnumerator ReenableMovementAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        playerMovementScript.canMove = true;
    }

    //Plays random explotion sound when player hits the minefield
    private void PlayExplotionSound()
    {
        if (audioClips.Length > 0) 
        {
            //choose a random audioclip to play
            AudioClip randomClip = audioClips[Random.Range(0, audioClips.Length)];
            
            //play the audioclip with audiosource
            audioSource.PlayOneShot(randomClip);
        }
    }
}