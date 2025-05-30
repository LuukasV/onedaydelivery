using UnityEngine;

/// <summary>
/// Main programmer: Luukas Vuolle
/// 
/// Mechanics when the Player Enters the Escape Zone
/// </summary>
public class EscapeManager : MonoBehaviour
{
    private UIManager uiManager;

    //At start the script finds the UImanager script
    private void Start()
    {
        //UIManager script is linked with the PlayerUI component
        uiManager = GameObject.FindWithTag("PlayerUI").GetComponent<UIManager>();
    }

    //When the Player touches the assigned collisonZone, the UI Manager is told to end the level
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            uiManager.EndLevel();
        }
    }

}
