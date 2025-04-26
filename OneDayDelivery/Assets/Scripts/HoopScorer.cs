using UnityEngine;

//Detects if something hits the collision area, and tells the game to score the fourth objective
public class HoopScorer : MonoBehaviour
{
    public AudioSource audioSource;
    private UIManager uiManager;
    private bool maxAmountPerBox;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //UIManager script is linked with the PlayerUI component
        uiManager = GameObject.FindWithTag("PlayerUI").GetComponent<UIManager>();

        maxAmountPerBox = true;
    }

    //What happens when something touches the Collider whith onTrigger event
    //Something happens only if the touching object has the Tag "Mailable"
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Mailable" && maxAmountPerBox)
        {
            audioSource.Play();

            uiManager.ScoreGoal();

            maxAmountPerBox = false;    //We only score once
        }
    }

}
