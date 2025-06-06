using System;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Main programmer: Luukas Vuolle
/// 
/// Controller for the Main Menu and its functions
/// Includes functions for all main menu buttons
/// Manages information on player score/money between levels
/// Manages Savings, as PlayerUI is assumed to be disabled
/// </summary>
public class MainMenuController : MonoBehaviour
{
    public GameObject popUpCanvas;

    public GameObject level1_star1;
    public GameObject level1_star2;
    public GameObject level1_star3;
    public GameObject level1_star4;

    [SerializeField]
    private Text bestTime;

    public Slider mouseSensSlider;

    //The Main Menu puts the cursor visible (if it is not already)
    public void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        //We Load the Saved GameData (the Data has been Saved when moving between levels)
        // Debug.Log("Main Menu activated; the Game has been LOADED");
        SaveSystem.LoadGameData();

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
        float mouseSens = PlayerPrefs.GetFloat("CurrentMouseSensitivity", 100); //Mouse sensitivity defaults to 200
        //We set the Mouse settings slider to current values
        mouseSensSlider.value = mouseSens/10;
    }

    //If Esc-button is pressed, we activate the pop-up canvas, which (hopefully) has the "Are you sure you want to Quit?" functionality
    public void Update()
    {
        if(Input.GetKeyDown("escape"))
        {
            popUpCanvas.SetActive(true);
        }

    }

    /// <summary>
    /// Ends the Game
    /// </summary>
    public void ButtonEffect_ConfirmQuit()
    {
        // Debug.Log("Confirm Quit has been pressed");
        Application.Quit();
    }

    /// <summary>
    /// Loads Scene 1 in Unity Build Settings
    /// </summary>
    public void ButtonEffect_Level1()
    {
        // Debug.Log("Level 1 button has been pressed");

        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    /// <summary>
    /// Loads Scene 2 in Unity Build Settings
    /// </summary>
    public void ButtonEffect_Level2()
    {
        // Debug.Log("Level 2 button has been pressed");

        UnityEngine.SceneManagement.SceneManager.LoadScene(2);
    }

    /// <summary>
    /// Orders the saved data to be cleared and loads the main menu again (with empty savegame/GameData)
    /// </summary>
    public void ClearDataButton()
    {
        // Debug.Log("Clear memory pressed");

        SaveSystem.ClearData();

        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    /// <summary>
    /// Saves the game, if player closes the game/application
    /// </summary>
    private void OnApplicationQuit()
    {
        // Debug.Log("Player has quit the game; the Game has been SAVED");
        SaveSystem.SaveGameState();
    }

    /// <summary>
    /// Sets a new mouse sensitivity to Player preferences
    /// </summary>
    public void adjustSensitivity()
    {
        PlayerPrefs.SetFloat("CurrentMouseSensitivity", mouseSensSlider.value * 10);
        // Debug.Log("New Mouse sensitivity is now: " + mouseSensSlider.value * 10);
    }
}
