using System;
using UnityEngine;
using System.IO;

//This class contains all data that we want to carry between scenes
//It can be accessed anywhere with GameData.value = orange;
//As a default the GameData must be updates with SaveSystem every Application restart/start
public static class GameData
{
    public static bool level1_star1 = false;
    public static bool level1_star2 = false;
    public static bool level1_star3 = false;
    public static bool level1_star4 = false;

    //public static int money;

    public static float level1_bestTime = 2147483647; //Default is so ludicrously high, it must be replaced

    //Here we can also indicate which tools have been purchased from the shop
    public static bool item1Active = false;
    public static bool item2Active = false;

    public static int starsEarned = 0;
    public static int starsSpent = 0;
}

//Controls the Saving/Loading of GameData, takes all savable data from GameData
//On command, loads/writes the (possible) saved data from a file to GameData
//On command, cleares saved data from a file
public static class SaveSystem
{
    public const string FILENAME_SAVEDATA = "/savedata.txt";

    /// <summary>
    /// Writes the current GameData to a dedicated File to be saved between Applications Uses
    /// </summary>
    public static void SaveGameState()
    {
        string filePathSaveData = Application.persistentDataPath + FILENAME_SAVEDATA;
        string saveData = "";

        //We put all GameData values to string, each one in their own line
        saveData += GameData.level1_star1.ToString() + System.Environment.NewLine;
        saveData += GameData.level1_star2.ToString() + System.Environment.NewLine;
        saveData += GameData.level1_star3.ToString() + System.Environment.NewLine;
        saveData += GameData.level1_star4.ToString() + System.Environment.NewLine;

        saveData += GameData.level1_bestTime + System.Environment.NewLine;

        saveData += GameData.item1Active.ToString() + System.Environment.NewLine;
        saveData += GameData.item2Active.ToString() + System.Environment.NewLine;
        saveData += GameData.starsEarned + System.Environment.NewLine;
        saveData += GameData.starsSpent + System.Environment.NewLine;

        //string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(filePathSaveData, saveData);
    }

    /// <summary>
    /// Tries to load GameData from a dedicated File to be used between Applications Uses
    /// If no previous savegames/Files exist, nothing happens and GameData will use defaults in the session
    /// </summary>
    public static void LoadGameData()
    {
        try
        {
            string filePath = Application.persistentDataPath + FILENAME_SAVEDATA;
            string fileContent = File.ReadAllText(filePath);
            string[] strLines = fileContent.Split(System.Environment.NewLine);

            //We set the GameData from the file in the same order as in the SaveData
            GameData.level1_star1 = bool.Parse(strLines[0]);
            GameData.level1_star2 = bool.Parse(strLines[1]);
            GameData.level1_star3 = bool.Parse(strLines[2]);
            GameData.level1_star4 = bool.Parse(strLines[3]);

            GameData.level1_bestTime = float.Parse(strLines[4]);

            GameData.item1Active = bool.Parse(strLines[5]);
            GameData.item2Active = bool.Parse(strLines[6]);
            GameData.starsEarned = int.Parse(strLines[7]);
            GameData.starsSpent = int.Parse(strLines[8]);

            //We calculate the amount of earned postmarks (here to allow compatibility with earlier game versions)
            GameData.starsEarned = 0;
            if (GameData.level1_star1) GameData.starsEarned++;
            if (GameData.level1_star2) GameData.starsEarned++;
            if (GameData.level1_star3) GameData.starsEarned++;
            if (GameData.level1_star4) GameData.starsEarned++;

        }
        catch   //If the save file is empty, we will change GameData to starting defaults (in the case of willfull reset)
        {
            // Debug.Log("No Saved Data/File found");

            GameData.level1_star1 = false;
            GameData.level1_star2 = false;
            GameData.level1_star3 = false;
            GameData.level1_star4 = false;

            GameData.item1Active = false;
            GameData.item2Active = false;
            GameData.starsEarned = 0;
            GameData.starsSpent = 0;

            GameData.level1_bestTime = 2147483647;
        }
    }

    /// <summary>
    /// Cleares saved data from saved file
    /// ATTENTION: GameData must be LOADED (from SaveData) with empty values/File to restore it also to defaults
    /// </summary>
    public static void ClearData()
    {
        string filePathSaveData = Application.persistentDataPath + FILENAME_SAVEDATA;
        string saveData = "";

        File.WriteAllText(filePathSaveData, saveData);  //We overwrite the save file with an empty string
    }
}