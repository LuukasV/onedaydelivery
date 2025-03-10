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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //The Cursor is made invisible to guarantee the working of certain menu elements
        UnityEngine.Cursor.visible = false;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;     //Tells the game to run, just in case

        boxesMailedScore = 0;
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
        scoreText.text = "Packets Delievered: " + boxesMailedScore.ToString() + "/" + maxBoxes.ToString();
        
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

    //End the level, initiates scoring, and returns the player to Main Menu
    public void EndLevel()
    {
        escapeText.text = "Well done :D";
        timerActive = false;

        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    //Goes back to main menu
    public void BackToMenu()
    {
        Debug.Log("Quit button has been pressed");
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
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
}
