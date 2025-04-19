using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;
using UnityEngine.UI;
using System;

//Manages the Player's Point system, and Timer
public class UIManager : MonoBehaviour
{
    private int boxesMailedScore;
    [SerializeField]
    private Text scoreText;
    [SerializeField]
    private Text timerText;
    [SerializeField]
    private Text escapeText;
    public float secondsInTimer;
    private bool negatiivinen;
    public int maxBoxes;
    public GameObject escapeZone; //Object that is activated when score is full
    private bool timerActive;
    public GameObject popUpCanvas;

    [SerializeField]
    private Text backpackScore;
    private int boxesInBackpack;
    private float originalTime;

    //This runs earned postmarks (named stars for simplicity)
    //private bool star1;
    //private bool star2;
    public float secondaryTimeAchievementInSeconds;
    private bool star4;

    public GameObject level1_star1;
    public GameObject level1_star2;
    public GameObject level1_star3;
    public GameObject level1_star4;
    public GameObject levelCompletedCanvas;
    [SerializeField]
    private Text yourTime;
    [SerializeField]
    private Text bestTime;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        originalTime = secondsInTimer;
        //The Cursor is made invisible to guarantee the working of certain menu elements
        UnityEngine.Cursor.visible = false;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;     //Tells the game to run, just in case

        boxesMailedScore = 0;
        boxesInBackpack = 0;
        backpackScore.text = boxesInBackpack.ToString();
        scoreText.text = "Packages Delivered: " + boxesMailedScore.ToString() + "/" + maxBoxes.ToString();
        UpdateTimer(secondsInTimer);

        timerActive = true;
        negatiivinen = false;

    }

    //Every update, the timer goes down (and updates)
    //We also check if esc-button is pressed, if it is. We activate the pop-up window and pause the game (and timer)
    private void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            //Also button makes the cursor visible
            UnityEngine.Cursor.visible = true;
            UnityEngine.Cursor.lockState = CursorLockMode.None;

            popUpCanvas.SetActive(true);
            Time.timeScale = 0;
            timerActive = false;
        }

        if (timerActive)
        {
            secondsInTimer = secondsInTimer - Time.deltaTime;
            UpdateTimer(secondsInTimer);
        }

        if(secondsInTimer < 0)
        {
            negatiivinen = true;
        }
    }

    //Adds one point to the UI's score system and activates escape zone if the score is full
    public void AddPoint()
    {
        boxesMailedScore++;
        scoreText.text = "Packages Delivered: " + boxesMailedScore.ToString() + "/" + maxBoxes.ToString();
        
        if(boxesMailedScore >= maxBoxes)
        {
            escapeText.text = "Return to the postmobile to exit the level!";
            ActivateEscape();
        }

    }

    //Updates the timer, and changes the time format from pure seconds, to hh:mm:ss:s
    public void UpdateTimer(float seconds)
    {
        TimeSpan timer = TimeSpan.FromSeconds(seconds);

        string timerInFormat = timer.ToString(@"hh\:mm\:ss\:f");

        timerText.text = timerInFormat;
        if (negatiivinen)
        {
            timerText.color = Color.red;
        }
    }

    //Activates the GameObject assigned to escapeZone
    public void ActivateEscape()
    {
        escapeZone.SetActive(true);
    }

    //Brings up end level screen, initiates scoring and sets earned stars visible in the end level screen
    public void EndLevel()
    {
        escapeText.text = "Well done :D";
        timerActive = false;

        //Also button makes the cursor visible
        UnityEngine.Cursor.visible = true;
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;

        //If we have achieved any goals, they are saved to GameData
        GameData.level1_star1 = true;   //If we have activated EscapeZone, we have delivered all packages
        if (!negatiivinen) GameData.level1_star2 = true;
        if (secondsInTimer >= secondaryTimeAchievementInSeconds) GameData.level1_star3 = true;
        if (star4) GameData.level1_star4 = true;

        //We check if any of the level's stars are achieved from GameData/Just now
        if (GameData.level1_star1) level1_star1.SetActive(true);
        if (GameData.level1_star2) level1_star2.SetActive(true);
        if (GameData.level1_star3) level1_star3.SetActive(true);
        if (GameData.level1_star4) level1_star4.SetActive(true);

        //We set current and best time in the end level canvas
        float spentSeconds = originalTime - secondsInTimer;
        TimeSpan timer = TimeSpan.FromSeconds(spentSeconds);
        string timerInFormat = timer.ToString(@"hh\:mm\:ss\:f");
        yourTime.text = "Your time: " + timerInFormat;
        if(spentSeconds < GameData.level1_bestTime)
        {
            GameData.level1_bestTime = spentSeconds;
            bestTime.text = "Best time: " + timerInFormat;
        } else {
            TimeSpan timerB = TimeSpan.FromSeconds(GameData.level1_bestTime);
            string bestInFormat = timerB.ToString(@"hh\:mm\:ss\:f");
            bestTime.text = "Best time: " + bestInFormat;
        }

        //We save the GameData
        Debug.Log("Player has completed the level; the Game has been SAVED");
        SaveSystem.SaveGameState();

        //We activate End Level screen
        levelCompletedCanvas.SetActive(true);
    }

    //Goes back to main menu
    public void BackToMenu()
    {
        Debug.Log("Quit button has been pressed");
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    //Restarts the current level
    public void Restart()
    {
        Debug.Log("Restart button has been pressed");
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }

    //Deactivates Quit menu and resumes game/timer
    public void ContinueGame()
    {
        //Also button makes the cursor invisible again
        UnityEngine.Cursor.visible = false;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;

        Debug.Log("Cancel button has been pressed");
        popUpCanvas.SetActive(false);
        Time.timeScale = 1;
        timerActive = true;
    }

    //Increases the backpack score on the right side bottom by one
    public void AddPointToBackpackScore()
    {
        boxesInBackpack++;
        backpackScore.text = boxesInBackpack.ToString();
    }

    //Decreases the backpack score on the right side bottom by one
    public void RemovePointFromBackpackScore()
    {
        boxesInBackpack--;
        backpackScore.text = boxesInBackpack.ToString();
    }

}
