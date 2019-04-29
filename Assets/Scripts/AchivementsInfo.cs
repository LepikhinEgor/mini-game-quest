using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AchivementsInfo {
    //класс с достижениями
    public static int failuresCount;
    public static bool[] inventoryItemsChecked = new bool[18];
    public static bool[,] failures = new bool[5,3];

    //"Неуправление гневом", за выбор молотка для Василисы
    public static bool isFailureControlAnger;
    public static bool dontFeedAnimals;
    public static bool isCynic;
    public static bool braveryAndStupidity;
    public static bool isBustler;
    public static bool isKopysha;
    public static bool isSniper;
    public static bool isVasilis;
    public static bool fingerInSky;
    public static bool isExperimenter;
    public static bool isNenastoyashik;
    public static bool isPedant;
    public static bool isVarvaraNose = true;
    public static bool isGoodBoy;
    public static bool isBadBoy;

    public static void CalculateAchivements()
    {
        foreach (bool isFail in failures)
            if (isFail)
                failuresCount++;

        if (failuresCount == 0)
            isSniper = true;
        if (failuresCount > 0 && failuresCount <= 4)
            isVasilis = true;
        if (failuresCount > 4 && failuresCount <= 7)
            isExperimenter = true;
        if (failuresCount > 7 && failuresCount <= 15)
            isNenastoyashik = true;
        if (failuresCount > 15)
            isPedant = true;

        if (isFailureControlAnger && dontFeedAnimals && isCynic && braveryAndStupidity)
            isBadBoy = true;

        if (!isFailureControlAnger && !dontFeedAnimals && !isCynic && !braveryAndStupidity)
            isGoodBoy = true;

        inventoryItemsChecked[6] = true;
        inventoryItemsChecked[8] = true;
        inventoryItemsChecked[11] = true;
        inventoryItemsChecked[15] = true;
        inventoryItemsChecked[17] = true;

        for(int i = 0; i < inventoryItemsChecked.Length; i++)
        {
            if (!inventoryItemsChecked[i])
                isVarvaraNose = false;
        }

        if (SceneInfo.gameTime < 600)
            isBustler = true;
        if (SceneInfo.gameTime > 1200)
            isKopysha = true;
    }


}
