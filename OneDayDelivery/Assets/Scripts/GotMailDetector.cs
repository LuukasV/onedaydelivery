using UnityEngine;

//Script that informs a Sound Effect to play sound, when something has entered the Collision/Trigger area,
//Also teleport the detected object inside the mailbox
public class GotMailDetector : MonoBehaviour
{
    public AudioSource audioSource;
    public GameObject whereToTeleport;  //Grabs the x, y, z values to the mail for teleport
    public GameObject uiPointScorer;    //Does nothing yet

    //What happens when something touches the Collider whith onTrigger event
    //Something happens only if the touching object has the Tag "Mailable"
    private void OnTriggerEnter(Collider collider)
    {
        //Play the sound only if the tag is correct
        //Also, by popular demand, grab the object and insert it inside the mailbox
        if(collider.gameObject.tag == "Mailable")
        {
            //Added a point to UI-element
            audioSource.Play();

            //Get the anchor's coordinates
            float xcordinate = whereToTeleport.transform.position.x;
            float ycordinate = whereToTeleport.transform.position.y;
            float zcordinate = whereToTeleport.transform.position.z;
            //Changes the x, y, and z coordinates of the box to (hopefully) inside the mailbox
            collider.gameObject.transform.position = new Vector3(xcordinate, ycordinate, zcordinate);

            //Halts the packtet's speed, and stops the player's grabbing attempt
            collider.attachedRigidbody.linearVelocity = new Vector3(0, 0, 0);
            collider.gameObject.GetComponent<ObjectGrabbable>().Drop();
        }

    }
}
