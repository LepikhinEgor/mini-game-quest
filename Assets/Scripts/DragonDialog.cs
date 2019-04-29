using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragonDialog : MonoBehaviour {

    GameObject currentSpeech;
    private string dragonText1;
    private string dragonText2;
    private string fatherText;
    private string dragonCheer;

    public int reactionNum;
    bool reactionIsActive;

    Vector3 dragonSpeechPos;
    Vector3 fatherSpeechPos;
    GameObject speechPrefab;

    enum DialogState
    {
        NOTHING,
        FATHER_FIRST,
        DRAGON_FIRST,
        DRAGON_SECOND,
        END_DIALOG,
        CORRECT_CHOICE
    }

    private static DialogState currentDialogState;


    public SceneInfo.Tools receivedTool;

    private void Awake()
    {
        fatherText = "Да у тебя, драконище, стресс от монотонной работы и непризнания заслуг. Я расскажу, как его побороть!»";
        dragonText1 = "А, привет. Пришел рассказать мне, что жизнь хороша? Иди к чёрту. " +
            "Ничего хорошего. Только знай – катай детишек и страшно рычи, хотя никто не пугается." +
            " Никакого этого, профессионального роста!";
        dragonText2 = "А я ж всегда хотел быть настоящим драконом!" +
            " Наводить жуть, собирать дань, понимаешь? Вот теперь я сбыл свою мечту." +
            " Правда, мне всё равно тошно";
        dragonCheer = "Знаешь, ты был прав. Я переосмыслил свои трудности и скучности." +
            " И вот что решил: сменю работу! Пойду в кино сниматься в роли Годзиллыча! Как тебе, а?";

        speechPrefab = (GameObject)Resources.Load("Prefabs/DialogWindow");
    }
    // Use this for initialization
    void Start()
    {
        SceneInfo.isRun = false;
        dragonSpeechPos = new Vector3(UserInfo.UserScreenSizeX / 2, UserInfo.UserScreenSizeY * 0.7F, 0);
        fatherSpeechPos = new Vector3(UserInfo.UserScreenSizeX / 2, UserInfo.UserScreenSizeY * 0.3F, 0);

        Debug.Log(SceneInfo.currentState);
        switch (SceneInfo.currentState)
        {
            case SceneInfo.States.DRAGON_HELLO:
                StartCoroutine("FirstSpeech");
                break;
            case SceneInfo.States.DRAGON_AFTER_TRAIN:
                StartCoroutine("ShowGoodReaction");
                break;
        }
    }

    IEnumerator FirstSpeech()
    {
        yield return new WaitForSeconds(2);
        CreateNewSpeech(dragonSpeechPos, 1.7F, dragonText1);
        currentDialogState = DialogState.DRAGON_FIRST;
    }

    IEnumerator ShowGoodReaction()
    {
        yield return new WaitForSeconds(2);

        CreateNewSpeech(dragonSpeechPos, 1.7F, dragonCheer);
        reactionIsActive = false;
        SceneInfo.isInteractiveMoment = false;
        currentDialogState = DialogState.CORRECT_CHOICE;
    }

    public void StartCarlsonReaction()
    {
        string reactionText;

        Debug.Log(reactionNum);
        switch (reactionNum)
        {
            case 0:
                reactionText = "Теперь он заржавеет, дальше что?";
                AchivementsInfo.failures[4, 0] = true;
                break;
            case 10:
                reactionText = "Если настаиваешь";
                AchivementsInfo.failures[4, 1] = true;
                AchivementsInfo.braveryAndStupidity = true;
                break;
            case 12:
                reactionText = "Убери эту пукалку, а то найдешь её где-нибудь не в том месте";
                AchivementsInfo.failures[4, 2] = true;
                break;
            case 16:
                reactionText = "Ты хочешь, чтобы я вспомнил всё, что мне не хочется воспоминать? Ладно, но если это не сработает, я на тебя сяду, так и знай";
                break;
            default:
                reactionText = "Мне это не нужно";
                break;
        }
        CreateNewSpeech(dragonSpeechPos, 1.4F, reactionText);

        reactionIsActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) &&
            (Input.mousePosition.y > UserInfo.UserScreenSizeY * Properties.InventoryShare
            || SceneInfo.isInteractiveMoment))
        {
            if (currentSpeech != null)
            {
                Destroy(currentSpeech);
            }

            switch (currentDialogState)
            {
                case DialogState.DRAGON_FIRST:
                    CreateNewSpeech(dragonSpeechPos, 1.5F, dragonText2);
                    currentDialogState = DialogState.DRAGON_SECOND;
                    break;
                case DialogState.DRAGON_SECOND:
                    CreateNewSpeech(fatherSpeechPos, 1.4F, fatherText);
                    currentDialogState = DialogState.FATHER_FIRST;
                    break;
                case DialogState.FATHER_FIRST:
                    SceneInfo.isInteractiveMoment = true;
                    currentDialogState = DialogState.END_DIALOG;
                    break;
                case DialogState.CORRECT_CHOICE:
                    SceneInfo.currentState = SceneInfo.States.FINAL_SCENE;
                    SceneInfo.timeRun = false;
                    LoadNextScene("FinalScene");
                    break;
                default:
                    break;

            }

            if (reactionIsActive)
            {
                reactionIsActive = false;
                Destroy(currentSpeech);

                if (reactionNum == 16)
                {
                    SceneInfo.isInteractiveMoment = false;
                    reactionNum = 0;

                    InventoryBar invBar = GameObject.FindGameObjectWithTag("InventoryBar").GetComponent<InventoryBar>();
                    invBar.RemoveItem(16);

                    SceneInfo.currentState = SceneInfo.States.SHAKE_SCENE;

                    LoadNextScene("PattingScene");
                }

                SceneInfo.isInteractiveMoment = true;
            }
        }
    }

    void CreateNewSpeech(Vector3 speechPos, float speechScale, string speech)
    {
        if (currentSpeech != null)
        {
            Destroy(currentSpeech);
        }

        GameObject inventoryBar = GameObject.FindGameObjectWithTag("InventoryBar");

        currentSpeech = Instantiate(speechPrefab, inventoryBar.transform.parent.parent);
        currentSpeech.transform.position = speechPos;

        currentSpeech.transform.localScale = new Vector3(speechScale, speechScale, speechScale);

        currentSpeech.transform.Find("Text").GetComponent<Text>().text = speech;
    }

    void LoadNextScene(string sceneName)
    {
        GameObject darkPrefab = Resources.Load<GameObject>("Prefabs/dark");

        GameObject UICanvas = GameObject.FindGameObjectWithTag("UI");
        GameObject dark = Instantiate(darkPrefab, UICanvas.transform);

        NextSceneLoader loader = dark.GetComponent<NextSceneLoader>();

        loader.isDuffusing = false;
        loader.StartCoroutine("SetDark");
        loader.sceneName = sceneName;
    }
}
