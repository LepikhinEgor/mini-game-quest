using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneInfo : MonoBehaviour {
    public static float gameTime = 0;
    public static bool timeRun = true;

    public static bool isInteractiveMoment = false;
    public static int selectItemNum = -1;
    public static bool isRun = true;
    public static bool isSelectItem = false;
    public static GameObject inventoryItemInfo;
    public static bool inventoryIsHiden;
    public static int chapter;
    public static float chapterWayDone;
    public static HashSet<int> inventoryItems = new HashSet<int>() { 0, 1, 2 };
    public static States currentState;
    public enum States {
        MAIN_MENU,
        PROLOGUE_DIALOG,
        RUN_BEFORE_VASILISA,
        VASILISA_HELLO,
        RUB_EAR_TUTORIAL,
        VASILISA_TRAINS,
        RUN_AFTER_VASILISA,
        CARLSON_HELLO,
        BREATHING_TRAINING,
        CARLSON_AGREE,
        RUN_AFTER_CARLSON,
        KYZYA_HELLO,
        SHAKE_SCENE,
        KYZYA_AFTER_TRAIN,
        RUN_AFTER_KYZYA,
        KOSHEY_HELLO,
        SHOULDERS_SCENE,
        KOSHEY_AFTER_TRAIN,
        RUN_AFTER_KOSHEY,
        DRAGON_HELLO,
        PATTING_SCENE,
        DRAGON_AFTER_TRAIN,
        FINAL_SCENE
    }

    public enum Tools
    {
        WATER,
        SCAKALKA,
        DEVIL_IN_BOX,
        HARMONIC,
        HAMMER,
        COVER,
        VASILISA_BOOK,
        PILLOW,
        BOTTLES
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (timeRun)
            gameTime += Time.deltaTime;
	}
}
