using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VasilisaDialog : MonoBehaviour {

    private GameObject backgroundImage;
    private GameObject rubEarImage;

    private Sprite rubEarSprite1;
    private Sprite rubEarSprite2;
    private Sprite inclineForwardSprite1;
    private Sprite inclineForwardSprite2;
    private Sprite inclineBackSprite1;
    private Sprite inclineBackSprite2;
    private Sprite vasilisaBackSprite;

    private bool reactionIsActive = false;
    public int reactionNum;
    private bool vasilisaSaid;
    private bool fatherSaid;
    private bool dialogIsFinish = false;
    private bool finishRubEar;
    private bool finishInclineForward;
    private bool finishInclineBack;
    private bool vasilisaAgree;

    enum DialogState
    {
        START,
        FIRST_VASILISA,
        FIRST_FATHER,
        END_DIALOG,
        FINISH_RUB_EAR,
        FINISH_INCLINE_FORWARD,
        FINISH_INCLINE_BACK,
        VASILISA_AGREE
    }
    DialogState currentState;

    Vector3 vasilisaSpeechPos;
    Vector3 fatherSpeechPos;
    GameObject speechPrefab;
    GameObject currentSpeech;

    string vasilisaText;
    string fatherText;

    private void Awake()
    {
        rubEarSprite1 = Resources.Load<Sprite>("Sprites/rubEar1");
        rubEarSprite2 = Resources.Load<Sprite>("Sprites/rubEar2");
        inclineForwardSprite1 = Resources.Load<Sprite>("Sprites/inclineForward1");
        inclineForwardSprite2 = Resources.Load<Sprite>("Sprites/inclineForward2");
        inclineBackSprite1 = Resources.Load<Sprite>("Sprites/inclineBack1");
        inclineBackSprite2 = Resources.Load<Sprite>("Sprites/InclineBack2");
        vasilisaBackSprite = Resources.Load<Sprite>("Sprites/VasilisaBack");

        vasilisaText = "Ничего не могу, заклинания забыла," +
            " у меня лапки, мост в отпуске, уходи";
        fatherText = "Василиса, это просто апатия от перенагрузок";

        speechPrefab = (GameObject)Resources.Load("Prefabs/DialogWindow");
    }

    void Start() {

        GameObject UICanvas = GameObject.FindGameObjectWithTag("UI");

        backgroundImage = UICanvas.transform.Find("background").gameObject;
        rubEarImage = UICanvas.transform.Find("rubEar1").gameObject;

        vasilisaSpeechPos = new Vector3(UserInfo.UserScreenSizeX / 2, UserInfo.UserScreenSizeY * 0.7F, 0);
        fatherSpeechPos = new Vector3(UserInfo.UserScreenSizeX / 2, UserInfo.UserScreenSizeY * 0.3F, 0);

        if (SceneInfo.currentState == SceneInfo.States.VASILISA_HELLO)
        {
            rubEarImage.SetActive(false);

            StartCoroutine("ShowFirstSpeech");
        }

        if (SceneInfo.currentState == SceneInfo.States.VASILISA_TRAINS)
        {
            rubEarImage.SetActive(true);
            backgroundImage.SetActive(false);

            currentState = DialogState.FINISH_RUB_EAR;

            StartCoroutine("ShowRubEarInfo");
            SceneInfo.isInteractiveMoment = false;
            StartCoroutine("LetRubEar");
        }
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetMouseButtonDown(0) &&
            (Input.mousePosition.y > UserInfo.UserScreenSizeY * Properties.InventoryShare 
            || SceneInfo.isInteractiveMoment))
        {
            if (currentSpeech != null)
            {
                Destroy(currentSpeech);
            }

            switch (currentState)
            {
                case DialogState.FIRST_VASILISA:
                    CreateNewSpeech(fatherSpeechPos, 1);
                    currentSpeech.transform.Find("Text").GetComponent<Text>().text = fatherText;
                    currentState = DialogState.FIRST_FATHER;
                    break;

                case DialogState.FIRST_FATHER:
                    SceneInfo.isInteractiveMoment = true;
                    currentState = DialogState.END_DIALOG;
                    break;

                case DialogState.FINISH_RUB_EAR:
                    StopCoroutine("LetRubEar");
                    StartCoroutine("ShowInclineForwardInfo");
                    currentState = DialogState.FINISH_INCLINE_FORWARD;
                    break;

                case DialogState.FINISH_INCLINE_FORWARD:
                    StopCoroutine("LetInclineForward");
                    StartCoroutine("ShowInclineBackInfo");
                    currentState = DialogState.FINISH_INCLINE_BACK;
                    break;

                case DialogState.FINISH_INCLINE_BACK:
                    StopCoroutine("LetInclineBack");

                    string speech = "Энергетизация тела. Здорово работает, нужно почитать про неё больше.   *Опускает мост*";
                    CreateNewSpeech(vasilisaSpeechPos, 1.2F);
                    currentSpeech.transform.Find("Text").GetComponent<Text>().text = speech;

                    rubEarImage.GetComponent<Image>().sprite = vasilisaBackSprite;
                    currentState = DialogState.VASILISA_AGREE;
                    break;
                case DialogState.VASILISA_AGREE:
                    vasilisaAgree = false;
                    SceneInfo.isRun = true;
                    SceneInfo.currentState = SceneInfo.States.RUN_AFTER_VASILISA;
                    LoadNextScene("RunnerScene");
                    break;

                default:
                    Debug.Log("Сломался диалог с Василисой");
                    break;

            }

            if (reactionIsActive)
            {
                reactionIsActive = false;
                Destroy(currentSpeech);
                if (reactionNum == 5)
                {
                    SceneInfo.isInteractiveMoment = false;
                    InventoryBar invBar = GameObject.FindGameObjectWithTag("InventoryBar").GetComponent<InventoryBar>();
                    invBar.RemoveItem(5);
                    LoadNextScene("EarMassagaScene");
                }
            }
        }
    }

    IEnumerator StartVasilisaReaction()
    {
        string reactionText;

        switch (reactionNum)
        {
            case 0:
                reactionText = "Очень спасибо, мне стало намного мокрее";
                AchivementsInfo.failures[0, 0] = true;
                break;
            case 3:
                reactionText = "Да, я уже почти пляшу, это заметно";
                AchivementsInfo.failures[0, 1] = true;
                break;
            case 4:
                reactionText = "Ты знаешь, это не помогает";
                AchivementsInfo.failures[0, 2] = true;
                AchivementsInfo.isFailureControlAnger = true;
                break;
            case 5:
                reactionText = "Говоришь, апатия? Ладно, как её развидеть?";
                break;
            default:
                reactionText = "Мне это не нужно";
                break;
        }
        CreateNewSpeech(vasilisaSpeechPos, 1F);

        currentSpeech.transform.Find("Text").GetComponent<Text>().text = reactionText;

        reactionIsActive = true;
        yield return null;
    }

    IEnumerator ShowFirstSpeech()
    {
        yield return new WaitForSeconds(4);

        CreateNewSpeech(vasilisaSpeechPos, 1);
        currentSpeech.transform.Find("Text").GetComponent<Text>().text = vasilisaText;

        currentState = DialogState.FIRST_VASILISA;
    }

    IEnumerator ShowRubEarInfo()
    {
        yield return new WaitForSeconds(2);
        SceneInfo.isInteractiveMoment = false;
        CreateNewSpeech(fatherSpeechPos, 1.6F);

        string rubEarInfo = "Упражнение направлено на энергетизацию тела, " +
            "обеспечивает нужную интенсивность и скорость протекания процессов " +
            "между нервными клетками головного мозга";

        currentSpeech.transform.Find("Text").GetComponent<Text>().text = rubEarInfo;
    }

    IEnumerator LetRubEar()
    {
        finishRubEar = true;
        while (true)
        {
            rubEarImage.GetComponent<Image>().sprite = rubEarSprite2;
            yield return new WaitForSeconds(0.3f);

            rubEarImage.GetComponent<Image>().sprite = rubEarSprite1;
            yield return new WaitForSeconds(0.3f);
        }
    }

    IEnumerator ShowInclineForwardInfo()
    {
        CreateNewSpeech(fatherSpeechPos, 1.6F);

        string rubEarInfo = "Упражнение наклоны вперед направлено на энергетизацию тела, " +
            "обеспечивает нужную интенсивность и скорость протекания процессов " +
            "между нервными клетками головного мозга";

        currentSpeech.transform.Find("Text").GetComponent<Text>().text = rubEarInfo;
        StartCoroutine("LetInclineForward");
        yield return null;
    }

    IEnumerator LetInclineForward()
    {
        finishInclineForward = true;
        while (true)
        {
            rubEarImage.GetComponent<Image>().sprite = inclineForwardSprite1;
            yield return new WaitForSeconds(0.3f);

            rubEarImage.GetComponent<Image>().sprite = inclineForwardSprite2;
            yield return new WaitForSeconds(0.3f);
        }
    }

    IEnumerator ShowInclineBackInfo()
    {
        CreateNewSpeech(fatherSpeechPos, 1.6F);

        string rubEarInfo = "Упражнение наклоны назад направлено на энергетизацию тела, " +
            "обеспечивает нужную интенсивность и скорость протекания процессов " +
            "между нервными клетками головного мозга";

        currentSpeech.transform.Find("Text").GetComponent<Text>().text = rubEarInfo;
        StartCoroutine("LetInclineBack");
        yield return null;
    }

    IEnumerator LetInclineBack()
    {
        finishInclineBack = true;
        while (true)
        {
            rubEarImage.GetComponent<Image>().sprite = inclineBackSprite1;
            yield return new WaitForSeconds(0.3f);

            rubEarImage.GetComponent<Image>().sprite = inclineBackSprite2;
            yield return new WaitForSeconds(0.3f);
        }
    }


    void CreateNewSpeech(Vector3 speechPos, float speechScale)
    {
        if (currentSpeech != null)
        {
            Destroy(currentSpeech);
        }

        GameObject inventoryBar = GameObject.FindGameObjectWithTag("InventoryBar");

        currentSpeech = Instantiate(speechPrefab, inventoryBar.transform.parent.parent);
        currentSpeech.transform.position = speechPos;

        currentSpeech.transform.localScale = new Vector3(speechScale, speechScale, speechScale);
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
