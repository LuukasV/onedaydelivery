using UnityEngine;

//Controller for the Main Menu and its functions
//Includes functions for all main menu buttons
public class MainMenuController : MonoBehaviour
{
    public GameObject popUpCanvas;

    //The Main Menu puts the cursor visible (if it is not already)
    public void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    //If Esc-button is pressed, we activate the pop-up canvas, which (hopefully) has the "Are you sure you want to Quit?" functionality
    public void Update()
    {
        if(Input.GetKeyDown("escape"))
        {
            popUpCanvas.SetActive(true);
        }

    }

    //Ends the Game
    public void ButtonEffect_ConfirmQuit()
    {
        Debug.Log("Confirm Quit has been pressed");
        Application.Quit();
    }

    //Loads Scene 1 in Unity Build Settings
    public void ButtonEffect_Level1()
    {
        Debug.Log("Level 1 button has been pressed");

        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    //Loads Scene 2 in Unity Build Settings
    public void ButtonEffect_Level2()
    {
        Debug.Log("Level 2 button has been pressed");

        UnityEngine.SceneManagement.SceneManager.LoadScene(2);
    }
}
