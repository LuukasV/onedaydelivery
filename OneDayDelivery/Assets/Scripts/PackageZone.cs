using UnityEngine;

public class PackageZone : MonoBehaviour
{
    public GameObject playerCapsule;
    private PlayerPickUpDrop playerPickupScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //We find the players' inventory script
        playerPickupScript = playerCapsule.GetComponent<PlayerPickUpDrop>();
    }

    //When enterining the Zone (we toggle on Inventory special rules)
    private void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            // Debug.Log("Player is within PackageZone");
            playerPickupScript.TogglePackageZone(true);
        }
    }

    //When leaving the Zone (we toggle off Inventory special rules)
    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            // Debug.Log("Player is outside PackageZone");
            playerPickupScript.TogglePackageZone(false);
        }
    }
}
