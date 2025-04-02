using UnityEngine;

//This class contains all data that we want toc arry between scenes
//It can be accessed anywhere with GameData.value = orange;
//As a default the GameData updates itself from SavedData
public static class GameData
{
    public static bool level1_star1;
    public static bool level1_star2;
    public static bool level1_star3;
    public static bool level1_star4;

    public static int money;

    public static float level1_bestTime = 2147483647; //Default is so ludicrously high, it must be replaced

    //Here we can also indicate which tools have been purchased from the shop
}
