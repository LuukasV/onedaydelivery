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
    public float secondsInTimer;
    private bool negatiivinen;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        boxesMailedScore = 0;
        scoreText.text = "Packets Mailed: " + boxesMailedScore.ToString();
        updateTimer(secondsInTimer);

        negatiivinen = false;
    }

    //Every update, the timer goes down (and updates)
    private void Update()
    {
        secondsInTimer = secondsInTimer - Time.deltaTime;
        updateTimer(secondsInTimer);

        if(secondsInTimer < 0)
        {
            negatiivinen = true;
        }
    }

    //Adds one point to the UI's score system
    public void addPoint()
    {
        boxesMailedScore++;
        scoreText.text = "Packets Mailed: " + boxesMailedScore.ToString();
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
}
