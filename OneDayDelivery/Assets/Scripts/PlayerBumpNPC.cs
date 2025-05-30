using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Programmer: Milo Hankama
/// Player collisions with npc. When player bumps into npc, npc gets pushed back and plays a sound. Also particle effect activates on collision
/// </summary>
public class PlayerBumpNPC : MonoBehaviour
{
    public AudioClip oof;
    public AudioSource audioSource;
    public ParticleSystem painParticles; //Particle system for pain

    //Player colliding with npc turns off it's AI and pushes it away from player
    void OnTriggerEnter(Collider other)
    {
        CapsuleCollider[] colliders = GetComponents<CapsuleCollider>();
        CapsuleCollider colliderTrigger = colliders[0];
        CapsuleCollider colliderHitbox = colliders[1];

        if (other.CompareTag("Player") || other.CompareTag("Mailable"))
        {
            //Disable NPC AI and activate physics
            gameObject.AddComponent<Rigidbody>();
            GetComponent<Animator>().enabled = false;
            GetComponent<NavMeshAgent>().enabled = false;
            GetComponent<AI_HarmlessWaypoint>().enabled = false;
            colliderHitbox.enabled = true;
            colliderTrigger.enabled = false;

            Vector3 pushDirection = (transform.position - other.transform.position).normalized; //which direction npc moves from the collision

            Vector3 particlePosition = transform.position + Vector3.up * 2f;
            GameObject particleInstance = Instantiate(painParticles.gameObject, particlePosition, Quaternion.identity);

            // start particlesystem instance
            ParticleSystem ps = particleInstance.GetComponent<ParticleSystem>();
            ps.Play();

            // destroy particleInstance after 5 seconds
            Destroy(particleInstance, 5f);

            GetComponent<Rigidbody>().AddForce(pushDirection * 40f, ForceMode.Impulse);

            audioSource.PlayOneShot(oof); //play sound when npc is hit
        }
    }
}
