using UnityEngine;

//Does things when the player touches objects collision zone
public class EscapeManager : MonoBehaviour
{
    private UIManager uiManager;

    //At start the script finds the UImanager script
    private void Start()
    {
        //UIManager script is linked with the PlayerUI component
        uiManager = GameObject.Find("PlayerUI").GetComponent<UIManager>();
    }

    //When the Player touches the assigned collisonZone, something happens
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            uiManager.stopTimer();
        }
    }

}
