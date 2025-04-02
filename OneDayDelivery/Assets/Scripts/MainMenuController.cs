using System;
using UnityEngine;
using UnityEngine.UI;

//Controller for the Main Menu and its functions
//Includes functions for all main menu buttons
//Manages information on player score/money between levels
public class MainMenuController : MonoBehaviour
{
    public GameObject popUpCanvas;

    public GameObject level1_star1;
    public GameObject level1_star2;
    public GameObject level1_star3;
    public GameObject level1_star4;

    [SerializeField]
    private Text bestTime;

    //The Main Menu puts the cursor visible (if it is not already)
    public void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        //We check if any of the level's stars are achieved from GameData
        if(GameData.level1_star1) level1_star1.SetActive(true);
        if(GameData.level1_star2) level1_star2.SetActive(true);
        if(GameData.level1_star3) level1_star3.SetActive(true);
        if(GameData.level1_star4) level1_star4.SetActive(true);

        if (GameData.level1_bestTime != 2147483647)
        {
            TimeSpan timerB = TimeSpan.FromSeconds(GameData.level1_bestTime);
            string bestInFormat = timerB.ToString(@"hh\:mm\:ss\:f");
            bestTime.text = "Best time: " + bestInFormat;
        }
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
