using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EarMassageController : MonoBehaviour {
    //управление сценой с массажем ушей

    private bool isWatched = false;
    public int imageNum = 1;

    //кадры которые сменяются
    public GameObject image1;
    public GameObject image2;
    public GameObject image3;

    private Button leftButton;
    private Button rightButton;
    private GameObject exitButton;

	// Use this for initialization
	void Start () {
        image1 = transform.parent.Find("1").gameObject;
        image2 = transform.parent.Find("2").gameObject;
        image2.SetActive(false);
        image3 = transform.parent.Find("3").gameObject;
        image3.SetActive(false);

        leftButton = transform.parent.Find("LeftButton").gameObject.GetComponent<Button>();
        rightButton = transform.parent.Find("RightButton").gameObject.GetComponent<Button>();
        exitButton = transform.parent.Find("ExitButton").gameObject;

        exitButton.GetComponent<Button>().onClick.AddListener(ExitButtonAction);
        exitButton.SetActive(false);
        leftButton.onClick.AddListener(LeftButtonAction);
        rightButton.onClick.AddListener(RightButtonAction);
    }
	
    public void SwitchLeft()
    {
        if (imageNum == 2)
        {
            imageNum = 1;
            image2.SetActive(false);
            image1.SetActive(true);
        }
        else if (imageNum == 3)
        {
            imageNum = 2;
            image3.SetActive(false);
            image2.SetActive(true);
        }
    }

    public void SwitchRight()
    {
        if (imageNum == 1)
        {
            imageNum = 2;
            image2.SetActive(true);
            image1.SetActive(false);
        }
        else if(imageNum == 2)
        {
            imageNum = 3;
            image3.SetActive(true);
            image2.SetActive(false);

            if (isWatched == false)
            {
                exitButton.SetActive(true);
                isWatched = true;
            }
        }
    }

    public void LeftButtonAction()
    {

        GameObject emc =  transform.parent.Find("UIController").gameObject;
        emc.GetComponent<EarMassageController>().SwitchLeft();
    }

    public void RightButtonAction()
    {

        GameObject emc =  transform.parent.Find("UIController").gameObject;
        emc.GetComponent<EarMassageController>().SwitchRight();
    }

    public void ExitButtonAction()
    {
        SceneInfo.currentState = SceneInfo.States.VASILISA_TRAINS;
        LoadNextScene("VasilisaScene");
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

    // Update is called once per frame
    void Update () {
		
	}

}
