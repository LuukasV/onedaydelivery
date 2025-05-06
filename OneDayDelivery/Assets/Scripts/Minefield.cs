using UnityEngine;
using System.Collections;

public class Minefield : MonoBehaviour
{
    private Vector3 resetPosition = new Vector3(300f, 50f, 275f); // Direction, where player is pushed
    public Rigidbody playerRb;   // Player rigidbody component
    public float throwForce = 100f;
    public float upForce = 50f;

    public PlayerMovement playerMovementScript; // Player movement script to disable moving

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player hit the minefield");

            if (playerRb != null)
            {
                Debug.Log("Throwing the player");

                playerMovementScript.canMove = false;
                StartCoroutine(ReenableMovementAfterDelay(1.0f));

                Vector3 direction = (resetPosition - playerRb.transform.position).normalized;
                Vector3 force = direction * throwForce + Vector3.up * upForce;

                playerRb.linearVelocity = Vector3.zero;
                playerRb.AddForce(force, ForceMode.VelocityChange);
            }
            else
            {
                Debug.LogWarning("playerRb is not set in the inspector!");
            }
        }
    }

    private IEnumerator ReenableMovementAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        playerMovementScript.canMove = true;
    }
}