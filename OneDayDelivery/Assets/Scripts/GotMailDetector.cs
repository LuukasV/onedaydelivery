using UnityEngine;

/// <summary>
/// Main programmer: Luukas Vuolle
/// Other progammers: Jussi Kolehmainen and Jari-Pekka Riihinen
/// Other programmers contribution was better communication with player's inventory scripts, and activation of confetti on package delivery event
/// 
/// Script that informs a Sound Effect to play sound, when something has entered the Collision/Trigger area,
/// Also teleport the detected object inside the mailbox
/// </summary>
public class GotMailDetector : MonoBehaviour
{
    public AudioSource audioSource; //Package delivered sound effect
    public ParticleSystem confetti; //Particle system for confetti
    public GameObject whereToTeleport;  //Grabs the x, y, z values to the mail for teleport
    private UIManager uiManager;
    private bool maxAmountPerBox;

    public int id;
    public CompassIcon myIcon;
    private CompassManager uiCompassManager;

    [SerializeField] private Transform playerObjectHandler;
    private Transform temp;

    //At start the script finds the UImanager script, so it can be updated when Mail is succesfully mailed
    private void Start()
    {
        //UIManager script is linked with the PlayerUI component
        uiManager = GameObject.FindWithTag("PlayerUI").GetComponent<UIManager>();
        //Checks if newer version of UI is available, if old was not found
        maxAmountPerBox = true;
        //Gives the boxes ID to its CompassIcon element (so its specific icon in the compass can be identified)
        myIcon.setID(id);
        uiCompassManager = GameObject.FindWithTag("PlayerUI").GetComponentInChildren<CompassManager>();
    }

    //The Player scores a point and the mailed package is rendered unmailable/ungrabbable
    //Something happens only if the touching object has the Tag "Mailable"
    private void OnTriggerEnter(Collider collider)
    {
        //Play the sound only if the tag is correct
        //Also, by popular demand, grab the object and insert it inside the mailbox
        if(collider.gameObject.tag == "Mailable" && maxAmountPerBox)
        {
            //Play package delivered sound effect
            audioSource.Play();

            //Play confetti particle system
            confetti.Play();

            temp = playerObjectHandler.transform.Find("PlayerObj_Capsule");
            temp.GetComponent<PlayerPickUpDrop>().objectHandlerNuller();

            //Get the anchor's coordinates
            float xcordinate = whereToTeleport.transform.position.x;
            float ycordinate = whereToTeleport.transform.position.y;
            float zcordinate = whereToTeleport.transform.position.z;
            //Changes the x, y, and z coordinates of the box to (hopefully) inside the mailbox
            collider.gameObject.transform.position = new Vector3(xcordinate, ycordinate, zcordinate);

            //Halts the packtet's speed, and stops the player's grabbing attempt and disables grabbing the object permanently
            collider.attachedRigidbody.linearVelocity = new Vector3(0, 0, 0);
            ObjectGrabbable paketti = collider.gameObject.GetComponent<ObjectGrabbable>();
            if(paketti != null)
            {
                paketti.Drop();
            }       

            //Add point to UIscoreSystem
            uiManager.AddPoint();
            maxAmountPerBox = false;

            collider.gameObject.tag = "Untagged";
            collider.transform.Find("pickupCollider").tag = "Untagged";

            //Removes the postboxe's icon in the Player's Compass
            uiCompassManager.disableSpecificIcon(id);
        }
    }
}
