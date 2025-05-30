using UnityEngine;

/// <summary>
/// Programmer: Jari-Pekka Riihinen
/// Player can stomp flowers and make them flat. Script replaces the original flower with the stomped variant with random rotation
/// </summary>
public class Flowers : MonoBehaviour
{
    public GameObject stompedFlower;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Mailable")) // Checks if player or box hits the flower
        {
            StompFlower();
        }
    }

    // Spawns the stomped flower with random rotation and destroys the original
    private void StompFlower()
    {
        Quaternion randomRotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);
        Instantiate(stompedFlower, transform.position, randomRotation);
        Destroy(gameObject);
    }
}
