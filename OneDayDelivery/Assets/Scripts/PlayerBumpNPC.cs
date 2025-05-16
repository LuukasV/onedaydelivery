using UnityEngine;
using UnityEngine.AI;

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

        if (other.CompareTag("Player") || other.CompareTag("Mailable")) {
            Debug.Log("PLAM");

            //Disable NPC AI and activate physics
            gameObject.AddComponent<Rigidbody>();
            GetComponent<Animator>().enabled = false;
            GetComponent<NavMeshAgent>().enabled = false;
            GetComponent<AI_HarmlessWaypoint>().enabled = false;
            colliderHitbox.enabled = true;
            colliderTrigger.enabled = false;

            Vector3 pushDirection = (transform.position - other.transform.position).normalized; //which direction npc moves from the collision

            //Play pain particle system
            //Instantiate(painParticles.gameObject, gameObject.transform.position, Quaternion.identity);
            //painParticles.Play();
            //Destroy(painParticles, 5f);
            // Instansioidaan partikkeli NPC:n yläpuolelle
            Vector3 particlePosition = transform.position + Vector3.up * 2f; // 2 yksikköä ylös (säädä tarpeen mukaan)
            GameObject particleInstance = Instantiate(painParticles.gameObject, particlePosition, Quaternion.identity);

            // Käynnistetään instanssin partikkelijärjestelmä
            ParticleSystem ps = particleInstance.GetComponent<ParticleSystem>();
            ps.Play();

            // Tuhoaa instanssin 5 sekunnin päästä
            Destroy(particleInstance, 5f);

            GetComponent<Rigidbody>().AddForce(pushDirection * 40f, ForceMode.Impulse);

            audioSource.PlayOneShot(oof); //play sound when npc is hit
        }
    }
}
