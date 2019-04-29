using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KosheyDialog : MonoBehaviour {

    GameObject currentSpeech;
    private string kosheyText;
    private string fatherText;
    private string kosheyCheer;

    public int reactionNum;
    bool reactionIsActive;

    Vector3 kosheySpeechPos;
    Vector3 fatherSpeechPos;
    GameObject speechPrefab;

    enum DialogState
    {
        NOTHING,
        FATHER_FIRST,
        KOSHEY_FIRST,
        END_DIALOG,
        CORRECT_CHOICE
    }

    private static DialogState currentDialogState;


    public SceneInfo.Tools receivedTool;

    private void Awake()
    {
        fatherText = "Всё ясно, переволновался за свои коварные планы, заклинило суставы. Я знаю, как тебе помочь";
        kosheyText = "*Кощей изображает кусок частокола среди других деревьев, у которых" +
            " несчастно-обалделые глаза, Кощей стоит среди них с поднятыми руками.*";
        kosheyCheer = "*Кощея расклинивает, он учит той же технике деревья, после чего" +
            " они дружно умаршировывают с тропы и заваливают собой ПАЗик*";

        speechPrefab = (GameObject)Resources.Load("Prefabs/DialogWindow");
    }
    // Use this for initialization
    void Start()
    {
        SceneInfo.isRun = false;
        kosheySpeechPos = new Vector3(UserInfo.UserScreenSizeX / 2, UserInfo.UserScreenSizeY * 0.7F, 0);
        fatherSpeechPos = new Vector3(UserInfo.UserScreenSizeX / 2, UserInfo.UserScreenSizeY * 0.3F, 0);

        Debug.Log(SceneInfo.currentState);
        switch (SceneInfo.currentState)
        {
            case SceneInfo.States.KOSHEY_HELLO:
                StartCoroutine("FirstSpeech");
                break;
            case SceneInfo.States.KOSHEY_AFTER_TRAIN:
                StartCoroutine("ShowGoodReaction");
                break;
        }
    }

    IEnumerator FirstSpeech()
    {
        yield return new WaitForSeconds(2);
        CreateNewSpeech(kosheySpeechPos, 1.5F, kosheyText);
        currentDialogState = DialogState.KOSHEY_FIRST;
    }

    IEnumerator ShowGoodReaction()
    {
        yield return new WaitForSeconds(2);

        CreateNewSpeech(kosheySpeechPos, 1.3F, kosheyCheer);
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
                reactionText = "Какой умелый садовод-любитель, а как насчет помочь мне?";
                AchivementsInfo.failures[3, 0] = true;
                break;
            case 12:
                reactionText = "«О, что это промчалось, такое чёрное? Это твой юмор побежал на рэп-баттл?";
                AchivementsInfo.isCynic = true;
                AchivementsInfo.failures[3, 1] = true;
                break;
            case 13:
                reactionText = "Техника похлопывания для подвижности суставов? Ну давай, заноси!";
                break;
            case 14:
                reactionText = "Если бы у меня сохранились внутренние органы, это было бы смешно. А потом бы я тебе врезал";
                AchivementsInfo.failures[3, 2] = true;
                break;
            default:
                reactionText = "Мне это не нужно";
                break;
        }
        CreateNewSpeech(kosheySpeechPos, 1.3F, reactionText);

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
                case DialogState.KOSHEY_FIRST:
                    CreateNewSpeech(fatherSpeechPos, 1.5F, fatherText);
                    currentDialogState = DialogState.FATHER_FIRST;
                    break;
                case DialogState.FATHER_FIRST:
                    SceneInfo.isInteractiveMoment = true;
                    currentDialogState = DialogState.END_DIALOG;
                    break;
                case DialogState.CORRECT_CHOICE:
                    SceneInfo.currentState = SceneInfo.States.RUN_AFTER_KOSHEY;
                    LoadNextScene("RunnerScene");
                    break;
                default:
                    break;

            }

            if (reactionIsActive)
            {
                reactionIsActive = false;
                Destroy(currentSpeech);

                if (reactionNum == 13)
                {
                    SceneInfo.isInteractiveMoment = false;
                    reactionNum = 0;

                    InventoryBar invBar = GameObject.FindGameObjectWithTag("InventoryBar").GetComponent<InventoryBar>();
                    invBar.RemoveItem(13);

                    SceneInfo.currentState = SceneInfo.States.SHOULDERS_SCENE;

                    LoadNextScene("ShouldersScene");
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
