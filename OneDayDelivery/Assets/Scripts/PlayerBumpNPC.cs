using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class PlayerBumpNPC : MonoBehaviour
{
    public AudioClip oof;
    public AudioSource audioSource;

    //Player colliding with npc turns off it's AI and pushes it away from player
    void OnTriggerEnter(Collider other)
    {

        CapsuleCollider[] colliders = GetComponents<CapsuleCollider>();
        CapsuleCollider colliderTrigger = colliders[0];
        CapsuleCollider colliderHitbox = colliders[1];

        if (other.CompareTag("Player")) {
            Debug.Log("PLAM");
            //Disable NPC AI and activate physics
            gameObject.AddComponent<Rigidbody>();
            GetComponent<Animator>().enabled = false;
            GetComponent<NavMeshAgent>().enabled = false;
            GetComponent<AI_HarmlessWaypoint>().enabled = false;
            colliderHitbox.enabled = true;
            colliderTrigger.enabled = false;

            Vector3 pushDirection = (transform.position - other.transform.position).normalized; //which direction npc moves from the collision
            GetComponent<Rigidbody>().AddForce(pushDirection * 40f, ForceMode.Impulse);

            audioSource.PlayOneShot(oof); //play sound when npc is hit


        }
    }

}
