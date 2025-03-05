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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        boxesMailedScore = 0;
        scoreText.text = "Packets Delivered: " + boxesMailedScore.ToString() + "/" + maxBoxes.ToString();
        updateTimer(secondsInTimer);

        timerActive = true;
        negatiivinen = false;
    }

    //Every update, the timer goes down (and updates)
    private void Update()
    {
        if(timerActive)
        {
            secondsInTimer = secondsInTimer - Time.deltaTime;
            updateTimer(secondsInTimer);
        }

        if(secondsInTimer < 0)
        {
            negatiivinen = true;
        }
    }

    //Adds one point to the UI's score system and activates escape zone if the score is full
    public void addPoint()
    {
        boxesMailedScore++;
        scoreText.text = "Packets Delievered: " + boxesMailedScore.ToString() + "/" + maxBoxes.ToString();
        
        if(boxesMailedScore >= maxBoxes)
        {
            escapeText.text = "Return to the postmobile to exit the level!";
            activateEscape();
        }

    }

    //Updates the timer, and changes the time format from pure seconds, to hh:mm:ss:s
    public void updateTimer(float seconds)
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
    public void activateEscape()
    {
        escapeZone.SetActive(true);
    }

    //Stops the count
    public void stopTimer()
    {
        escapeText.text = "Well done :D";
        timerActive = false;
    }
}
