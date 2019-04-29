using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EventsManager : MonoBehaviour {
    //Отвечает за события в сцене RunnerScene
    public static string nextSceneName;

    //номер следующего спавнивающегося предмета
    private int wayPartNum = 0;
    public GameObject currentSpeech;
    GameObject speechPrefab;
    GameObject newToolPrefab;
    // Use this for initialization

    private void Awake()
    {
        speechPrefab = (GameObject)Resources.Load("Prefabs/DialogWindow");
        newToolPrefab = (GameObject)Resources.Load("Prefabs/newTool");
    }
    void Start () {
        SceneInfo.isInteractiveMoment = false;
        SceneInfo.isRun = true;
        Debug.Log(SceneInfo.currentState);
        switch (SceneInfo.currentState)
        {
            case SceneInfo.States.RUN_BEFORE_VASILISA:
                StartCoroutine("SpawnHarmonic");
                break;
            case SceneInfo.States.RUN_AFTER_VASILISA:
                StartCoroutine("SpawnPillow");
                wayPartNum = 3;
                break;
            case SceneInfo.States.RUN_AFTER_CARLSON:
                StartCoroutine("SpawnBasket");
                wayPartNum = 4;
                break;
            case SceneInfo.States.RUN_AFTER_KYZYA:
                StartCoroutine("SpawnGun");
                wayPartNum = 6;
                break;
            case SceneInfo.States.RUN_AFTER_KOSHEY:
                StartCoroutine("SpawnDiary");
                wayPartNum = 9;
                break;
        }
	}


    public void StartNextSpawn()
    {
        switch (wayPartNum)
        {
            case 0:
                StartCoroutine("SpawnHammer");
                break;
            case 1:
                StartCoroutine("SpawnCover");
                break;
            case 2:
                StartCoroutine("SpawnBook");
                break;
            case 3:
                StartCoroutine("SpawnBottles");
                break;
            case 4:
                StartCoroutine("SpawnShirt");
                break;
            case 5:
                StartCoroutine("SpawnStump");
                break;
            case 6:
                StartCoroutine("SpawnExpander");
                break;
            case 7:
                StartCoroutine("SpawnLaxative");
                break;
            case 8:
                StartCoroutine("SpawnForest");
                break;
            case 9:
                StartCoroutine("SpawnLair");
                break;
        }
        wayPartNum++;
    }
	
    IEnumerator SpawnHarmonic()
    {
        yield return new WaitForSeconds(Properties.timeBetweenTools);

        GameObject harmonic = Instantiate(newToolPrefab, transform.parent);
        harmonic.GetComponent<ToolMotion>().SetID(3);
    }

    IEnumerator SpawnHammer()
    {
        yield return new WaitForSeconds(Properties.timeBetweenTools);

        GameObject hammer = Instantiate(newToolPrefab, transform.parent);
        hammer.GetComponent<ToolMotion>().SetID(4);
    }

    IEnumerator SpawnCover()
    {
        yield return new WaitForSeconds(Properties.timeBetweenTools);

        GameObject hammer = Instantiate(newToolPrefab, transform.parent);
        hammer.GetComponent<ToolMotion>().SetID(5);
    }

    IEnumerator SpawnBook()
    {
        yield return new WaitForSeconds(Properties.timeBetweenTools);

        GameObject book = Instantiate(newToolPrefab, transform.parent);
        book.GetComponent<ToolMotion>().SetID(6);
    }

    IEnumerator SpawnPillow()
    {
        yield return new WaitForSeconds(Properties.timeBetweenTools);

        GameObject pillow = Instantiate(newToolPrefab, transform.parent);
        pillow.GetComponent<ToolMotion>().SetID(7);
    }

    IEnumerator SpawnBottles()
    {
        yield return new WaitForSeconds(Properties.timeBetweenTools);

        GameObject bottles = Instantiate(newToolPrefab, transform.parent);
        bottles.GetComponent<ToolMotion>().SetID(8);
    }

    IEnumerator SpawnBasket()
    {
        yield return new WaitForSeconds(Properties.timeBetweenTools);

        GameObject basket = Instantiate(newToolPrefab, transform.parent);
        basket.GetComponent<ToolMotion>().SetID(9);
    }

    IEnumerator SpawnShirt()
    {
        yield return new WaitForSeconds(Properties.timeBetweenTools);

        GameObject basket = Instantiate(newToolPrefab, transform.parent);
        basket.GetComponent<ToolMotion>().SetID(10);
    }

    IEnumerator SpawnStump()
    {
        yield return new WaitForSeconds(Properties.timeBetweenTools);

        GameObject stump = Instantiate(newToolPrefab, transform.parent);
        stump.GetComponent<ToolMotion>().SetID(11);
    }

    IEnumerator SpawnGun()
    {
        yield return new WaitForSeconds(Properties.timeBetweenTools);

        GameObject stump = Instantiate(newToolPrefab, transform.parent);
        stump.GetComponent<ToolMotion>().SetID(12);
    }

    IEnumerator SpawnExpander()
    {
        yield return new WaitForSeconds(Properties.timeBetweenTools);

        GameObject stump = Instantiate(newToolPrefab, transform.parent);
        stump.GetComponent<ToolMotion>().SetID(13);
    }

    IEnumerator SpawnLaxative()
    {
        yield return new WaitForSeconds(Properties.timeBetweenTools);

        GameObject stump = Instantiate(newToolPrefab, transform.parent);
        stump.GetComponent<ToolMotion>().SetID(14);
    }

    IEnumerator SpawnForest()
    {
        yield return new WaitForSeconds(Properties.timeBetweenTools);

        GameObject stump = Instantiate(newToolPrefab, transform.parent);
        stump.GetComponent<ToolMotion>().SetID(15);
    }

    IEnumerator SpawnDiary()
    {
        yield return new WaitForSeconds(Properties.timeBetweenTools);

        GameObject stump = Instantiate(newToolPrefab, transform.parent);
        stump.GetComponent<ToolMotion>().SetID(16);
    }

    IEnumerator SpawnLair()
    {
        yield return new WaitForSeconds(Properties.timeBetweenTools);

        GameObject stump = Instantiate(newToolPrefab, transform.parent);
        stump.GetComponent<ToolMotion>().SetID(17);
    }

    public void StartSpawnHammer()
    {
        StartCoroutine("SpawnHammer");
    }


    public void CreateNewSpeech(Vector3 speechPos, float speechScale, string text)
    {
        if (currentSpeech != null)
        {
            Destroy(currentSpeech);
        }

        currentSpeech = Instantiate(speechPrefab, GameObject.FindGameObjectWithTag("UI").transform);

        currentSpeech.transform.position = speechPos;;
        currentSpeech.transform.localScale = new Vector3(speechScale, speechScale, speechScale);
        currentSpeech.transform.Find("Text").GetComponent<Text>().text = text;
    }


    // Update is called once per frame
    void Update () {
		
	}
}
