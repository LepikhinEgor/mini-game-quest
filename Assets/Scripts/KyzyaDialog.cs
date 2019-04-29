using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KyzyaDialog : MonoBehaviour {

    GameObject currentSpeech;
    private string kyzyaText;
    private string fatherText;
    private string kyzyaCheer;

    public int reactionNum;
    bool reactionIsActive;

    Vector3 kyzyaSpeechPos;
    Vector3 fatherSpeechPos;
    GameObject speechPrefab;

    enum DialogState
    {
        NOTHING,
        FATHER_FIRST,
        KYZYA_FIRST,
        END_DIALOG,
        CORRECT_CHOICE
    }

    private static DialogState currentDialogState;


    SceneInfo.Tools receivedTool;

    private void Awake()
    {
        fatherText = "ПАЗик, кажется, отстал. Нужно помочь этой жертве тревожности, а то пропадёт весь";
        kyzyaText = "Тревоги, тревоги-то какие, убытки не считаны, запасы не меряны, хлеб не черствеет";
        kyzyaCheer = "Кузя распутывается из колобка в домовёнка, бьет морду лисе";

        speechPrefab = (GameObject)Resources.Load("Prefabs/DialogWindow");
    }
    // Use this for initialization
    void Start()
    {
        SceneInfo.isRun = false;
        kyzyaSpeechPos = new Vector3(UserInfo.UserScreenSizeX / 2, UserInfo.UserScreenSizeY * 0.7F, 0);
        fatherSpeechPos = new Vector3(UserInfo.UserScreenSizeX / 2, UserInfo.UserScreenSizeY * 0.3F, 0);

        Debug.Log(SceneInfo.currentState);
        switch (SceneInfo.currentState)
        {
            case SceneInfo.States.KYZYA_HELLO:
                StartCoroutine("FirstSpeech");
                break;
            case SceneInfo.States.KYZYA_AFTER_TRAIN:
                StartCoroutine("ShowGoodReaction");
                break;
        }
    }

    IEnumerator FirstSpeech()
    {
        yield return new WaitForSeconds(2);
        CreateNewSpeech(kyzyaSpeechPos, 1.3F, kyzyaText);
        currentDialogState = DialogState.KYZYA_FIRST;
    }

    IEnumerator ShowGoodReaction()
    {
        yield return new WaitForSeconds(2);

        CreateNewSpeech(kyzyaSpeechPos, 1.2F, kyzyaCheer);
        reactionIsActive = false;
        SceneInfo.isInteractiveMoment = false;
        currentDialogState = DialogState.CORRECT_CHOICE;
    }

    public void StartCarlsonReaction()
    {
        string reactionText;

        switch (reactionNum)
        {
            case 1:
                reactionText = "Я тебе чего, коняшка, что ли?";
                AchivementsInfo.failures[2, 0] = true;
                break;
            case 2:
                reactionText = "Снижение тревожности? Это кто тут тревожный? Ладно, давай, показывай";
                break;
            case 9:
                reactionText = "Когда я говорил, что люблю играть, я не это имел в виду!";
                AchivementsInfo.failures[2, 1] = true;
                break;
            case 10:
                reactionText = "*Лиса сжирает Кузю*";
                AchivementsInfo.failures[2, 2] = true;
                AchivementsInfo.dontFeedAnimals = true;
                break;
            default:
                reactionText = "Мне это не нужно";
                break;
        }
        CreateNewSpeech(kyzyaSpeechPos, 1.2F, reactionText);

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
                case DialogState.KYZYA_FIRST:
                    CreateNewSpeech(fatherSpeechPos, 1.2F, fatherText);
                    currentDialogState = DialogState.FATHER_FIRST;
                    break;
                case DialogState.FATHER_FIRST:
                    SceneInfo.isInteractiveMoment = true;
                    currentDialogState = DialogState.END_DIALOG;
                    break;
                case DialogState.CORRECT_CHOICE:
                    SceneInfo.currentState = SceneInfo.States.RUN_AFTER_KYZYA;
                    LoadNextScene("RunnerScene");
                    break;
                default:
                    break;

            }

            if (reactionIsActive)
            {
                reactionIsActive = false;
                Destroy(currentSpeech);

                if (reactionNum == 2)
                { 
                    SceneInfo.isInteractiveMoment = false;
                    reactionNum = 0;

                    InventoryBar invBar = GameObject.FindGameObjectWithTag("InventoryBar").GetComponent<InventoryBar>();
                    invBar.RemoveItem(2);

                    SceneInfo.currentState = SceneInfo.States.SHAKE_SCENE;

                    LoadNextScene("ShakeScene");
                }

                if (reactionNum == 10)
                {
                    SceneInfo.isInteractiveMoment = false;
                    reactionNum = 0;

                    InventoryBar invBar = GameObject.FindGameObjectWithTag("InventoryBar").GetComponent<InventoryBar>();
                    invBar.RemoveItem(2);

                    SceneInfo.currentState = SceneInfo.States.RUN_AFTER_KYZYA;

                    LoadNextScene("RunnerScene");
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
