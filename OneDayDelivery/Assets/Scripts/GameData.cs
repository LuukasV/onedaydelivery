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

    //Here we can also indicate which tools have been purchased from the shop
}
