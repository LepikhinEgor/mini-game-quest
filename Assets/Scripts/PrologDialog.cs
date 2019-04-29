using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PrologDialog : MonoBehaviour {
    // отвечает за диалог в завязке
    Vector3 tvSpeechPos;
    Vector3 fatherSpeechPos;
    Vector3 daughterSpeechPos;

    GameObject speechPrefab;
    GameObject currentSpeech;
    private string[] speeches;
    private int currSpeech;

    private void Awake()
    {
        speechPrefab = (GameObject)Resources.Load("Prefabs/DialogWindow");
    }

    void Start () {

        tvSpeechPos = new Vector3(UserInfo.UserScreenSizeX / 2.0F, UserInfo.UserScreenSizeY * 0.7F, 0);
        daughterSpeechPos = new Vector3(UserInfo.UserScreenSizeX / 4.0F, UserInfo.UserScreenSizeY * 0.6F, 0);
        fatherSpeechPos = new Vector3(UserInfo.UserScreenSizeX / 4.0F*3, UserInfo.UserScreenSizeY * 0.6F, 0);

        speeches = new string[10];
        speeches[0] = "А теперь тревожные новости из дружественных земель." +
            " В Сказоземье ЧП: Змей Горыныч совершает набеги на стада овец" +
            " и рыцарские обители! Не счесть безвинно съеденных! ";
        speeches[1] = "Доктор Айболит дистанционно диагностирует драконье" +
            " бешенство! ПАЗик-эвтаназик готовится выехать и уничтожить" +
            " этого оборотня в чешуе! Жителей близлежащих деревень" +
            " эвакуируют и просят сохранять невозмутимость";
        speeches[2] = "Папа, папа, это же неправда, Горыныч никакой не" +
            " бешеный, он хороший и меня катал на шее!";
        speeches[3] = "О_о";
        speeches[4] = "Спаси его от ПАЗика-эвтаназика!";
        speeches[5] = "Не-не-не!";
        speeches[6] = "Ну, папа, пожалуйста, ты же можешь всё!";
        speeches[7] = "*Лезет в телевизор*";
        speeches[8] = "Я положила туда спасительного мишку и всякое другое";

        StartCoroutine("CreateFirstSpeech");
    }

    public string GetNewSpeech()
    {
        //возвращает следующую реплику
        if (currSpeech >= speeches.Length)
        {
            Debug.Log("Ошибка при получении реплики");
            return null;
        }

        string newSpeech = speeches[currSpeech];
        currSpeech++;

        return newSpeech;
    }

    IEnumerator LoadRunnerScene()
    {
        //загружает следующую сцену
        yield return new WaitForSeconds(0.2F);
        SceneInfo.currentState = SceneInfo.States.RUN_BEFORE_VASILISA;
        LoadNextScene("RunnerScene");
    }

    IEnumerator CreateFirstSpeech()
    {
        //создает первое диалоговое окно
        yield return new WaitForSeconds(1.5F);

        DialogNext();

        yield return null;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
            DialogNext();
	}

    void DialogNext()
    {
        //создает следующее диалоговое окно
        float speechScale = 0;
        Vector3 speechPos = new Vector3();

        if (currSpeech > 8)
            StartCoroutine("LoadRunnerScene");

        switch (currSpeech)
        {
            case 0:
                speechScale = 1.6F;
                speechPos = tvSpeechPos;
                break;

            case 1:
                speechScale = 1.6F;
                speechPos = tvSpeechPos;
                break;
            case 2:
                speechScale = 1.2F;
                speechPos = daughterSpeechPos;
                break;
            case 3:
                speechScale = 0.7F;
                speechPos = fatherSpeechPos;
                break;
            case 4:
                speechScale = 1.2F;
                speechPos = daughterSpeechPos;
                break;
            case 5:
                speechScale = 0.7F;
                speechPos = fatherSpeechPos;
                break;
            case 6:
                speechScale = 1F;
                speechPos = daughterSpeechPos;
                break;
            case 7:
                speechScale = 0.7F;
                speechPos = fatherSpeechPos;
                break;
            case 8:
                speechScale = 1F;
                speechPos = daughterSpeechPos;
                break;
            default:
                if (currentSpeech != null)
                    Destroy(currentSpeech);
                return;
        }

        CreateNewSpeech(speechPos, speechScale);
        currentSpeech.transform.Find("Text").GetComponent<Text>().text = GetNewSpeech();
    }



    void CreateNewSpeech(Vector3 speechPos, float speechScale)
    {
        //создает диалоговое окно
        if (currentSpeech != null)
        {
            Destroy(currentSpeech);
        }

        currentSpeech = Instantiate(speechPrefab, transform.parent);
        currentSpeech.transform.position = speechPos;

        currentSpeech.transform.localScale = new Vector3(speechScale, speechScale, speechScale);
    }

    void LoadNextScene(string sceneName)
    {
        //загружает следующую сцену
        GameObject darkPrefab = Resources.Load<GameObject>("Prefabs/dark");

        GameObject UICanvas = GameObject.FindGameObjectWithTag("UI");
        GameObject dark = Instantiate(darkPrefab, UICanvas.transform);

        NextSceneLoader loader = dark.GetComponent<NextSceneLoader>();

        loader.isDuffusing = false;
        loader.StartCoroutine("SetDark");
        loader.sceneName = sceneName;
    }
}
