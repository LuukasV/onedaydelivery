using System;
using UnityEngine;
using System.IO;

//This class contains all data that we want toc arry between scenes
//It can be accessed anywhere with GameData.value = orange;
//As a default the GameData updates itself from SavedData
public static class GameData
{
    public static bool level1_star1;
    public static bool level1_star2;
    public static bool level1_star3;
    public static bool level1_star4;

    //public static int money;

    public static float level1_bestTime = 2147483647; //Default is so ludicrously high, it must be replaced

    //Here we can also indicate which tools have been purchased from the shop
}

//Controls the Saving of GameData, takes all savable data from GameData
public static class SaveSystem
{
    public const string FILENAME_SAVEDATA = "/savedata.txt";

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

        //string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(filePathSaveData, saveData);
    }
}

//On command, loads/writes the (possible) saved data from a json file to GameData
public static class LoadSystem
{
    //We try to load GameData from saved file
    public static void LoadGameData()
    {
        try
        {
            string filePath = Application.persistentDataPath + SaveSystem.FILENAME_SAVEDATA;
            string fileContent = File.ReadAllText(filePath);
            string[] strLines = fileContent.Split(System.Environment.NewLine);

            //We set the GameData from the file in the same order as in the SaveData
            GameData.level1_star1 = bool.Parse(strLines[0]);
            GameData.level1_star2 = bool.Parse(strLines[1]);
            GameData.level1_star3 = bool.Parse(strLines[2]);
            GameData.level1_star4 = bool.Parse(strLines[3]);

            GameData.level1_bestTime = float.Parse(strLines[4]);

        } catch
        {
            Debug.Log("No Saved Data/File found");
        }
    }
}