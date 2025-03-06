using UnityEngine;

//Controller for the Main Menu and its functions
//Includes functions for all main menu buttons
public class MainMenuController : MonoBehaviour
{
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
