using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarlsonDialog : MonoBehaviour {
    // Управляет диалогами в сцене с карлсоном
    GameObject currentSpeech;
    private string carlsonText;
    private string fatherText;
    private string carlsonFirstCheer;
    private string carlsonSecondCheer;

    public int reactionNum;
    bool reactionIsActive;

    Vector3 carlsonSpeechPos;
    Vector3 fatherSpeechPos;
    GameObject speechPrefab;

    enum DialogState
    {
        NOTHING,
        FATHER_FIRST,
        CARLSON_FIRST,
        END_DIALOG,
        FIRST_CORRECT_CHOICE,
        SECOND_CORRECT_CHOICE
    }

    private static DialogState currentDialogState;


    SceneInfo.Tools receivedTool;

    private void Awake()
    {
        fatherText = "Это еще что такое? Мне не перебраться через эти завалы!";
        carlsonText = "Сил нет, в горле ком, даже кушать не могу. Я самый тяжело уставший в мире челове-ек";
        carlsonFirstCheer = "Знаешь, мне и правда лучше. Не хорошо, но лучше";
        carlsonSecondCheer = "Я – самый тяжело здоровый в мире человек с моторчиком, я тебя мигом перенесу на ту сторону";

        speechPrefab = (GameObject)Resources.Load("Prefabs/DialogWindow");
    }
    // Use this for initialization
    void Start () {
        SceneInfo.isRun = false;
        carlsonSpeechPos = new Vector3(UserInfo.UserScreenSizeX / 2, UserInfo.UserScreenSizeY * 0.7F, 0);
        fatherSpeechPos = new Vector3(UserInfo.UserScreenSizeX / 2, UserInfo.UserScreenSizeY * 0.3F, 0);

        Debug.Log(SceneInfo.currentState);
        switch (SceneInfo.currentState)
        {
            case SceneInfo.States.CARLSON_HELLO:
                StartCoroutine("FirstSpeech");
                break;
            case SceneInfo.States.CARLSON_AGREE:
                SceneInfo.isInteractiveMoment = false;
                StartCoroutine("ShowGoodReaction");
                break;
        }
    }

    IEnumerator FirstSpeech()
    {
        yield return new WaitForSeconds(2);
        CreateNewSpeech(fatherSpeechPos, 1.0F, fatherText);
        currentDialogState = DialogState.FATHER_FIRST;
    }

    IEnumerator ShowGoodReaction()
    {
        yield return new WaitForSeconds(2);
        if (currentDialogState == DialogState.FIRST_CORRECT_CHOICE)
        {
            CreateNewSpeech(carlsonSpeechPos, 1.2F, carlsonFirstCheer);
            reactionIsActive = true;
            SceneInfo.isInteractiveMoment = true;
            yield return null;
        }
        if (currentDialogState == DialogState.SECOND_CORRECT_CHOICE)
            CreateNewSpeech(carlsonSpeechPos, 1.2F, carlsonSecondCheer);
        yield return null;
    }

    public void StartCarlsonReaction()
    {
        string reactionText;

        switch (reactionNum)
        {
            case 2:
                reactionText = "*С чертом мотают головами*";
                AchivementsInfo.failures[1, 0] = true;
                break;
            case 3:
                reactionText = "Говоришь, чтобы убрать ком в горле, нужно петь?" +
                    " Это мне по нраву! Ка-алинка, малинка…";
                SceneInfo.isInteractiveMoment = false;
                StartCoroutine("ShowGoodReaction");
                break;
            case 4:
                reactionText = "Душу я отвёл, но ком в горле остался";
                AchivementsInfo.failures[1, 1] = true;
                break;
            case 7:
                reactionText = "Дыхательные прак-ти… что? Практики? Ну ладно, я никогда не прочь подышать";
                break;
            default:
                reactionText = "Мне это не нужно";
                break;
        }
        CreateNewSpeech(carlsonSpeechPos, 1F, reactionText);

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
                case DialogState.FATHER_FIRST:
                    CreateNewSpeech(carlsonSpeechPos, 1.0F, carlsonText);
                    currentDialogState = DialogState.CARLSON_FIRST;
                    break;
                case DialogState.CARLSON_FIRST:
                    SceneInfo.isInteractiveMoment = true;
                    currentDialogState = DialogState.END_DIALOG;
                    break;
                default:
                    break;

            }

            if (currentDialogState == DialogState.SECOND_CORRECT_CHOICE)
            {
                SceneInfo.currentState = SceneInfo.States.RUN_AFTER_CARLSON;
                LoadNextScene("RunnerScene");
            }

            if (reactionIsActive)
            {
                reactionIsActive = false;
                Destroy(currentSpeech);

                if (reactionNum == 3)
                {
                    if (currentDialogState == DialogState.END_DIALOG)
                        currentDialogState = DialogState.FIRST_CORRECT_CHOICE;
                    else if
                        (currentDialogState == DialogState.FIRST_CORRECT_CHOICE)
                        currentDialogState = DialogState.SECOND_CORRECT_CHOICE;
                    SceneInfo.isInteractiveMoment = false;
                    reactionNum = 0;

                    InventoryBar invBar = GameObject.FindGameObjectWithTag("InventoryBar").GetComponent<InventoryBar>();
                    invBar.RemoveItem(3);
                }
                if (reactionNum == 7)
                {
                    if (currentDialogState == DialogState.END_DIALOG)
                        currentDialogState = DialogState.FIRST_CORRECT_CHOICE;
                    else if
                        (currentDialogState == DialogState.FIRST_CORRECT_CHOICE)
                        currentDialogState = DialogState.SECOND_CORRECT_CHOICE;
                    SceneInfo.isInteractiveMoment = false;
                    reactionNum = 0;
                    LoadNextScene("BreathTrainScene");
                    SceneInfo.currentState = SceneInfo.States.BREATHING_TRAINING;

                    InventoryBar invBar = GameObject.FindGameObjectWithTag("InventoryBar").GetComponent<InventoryBar>();
                    invBar.RemoveItem(7);
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
