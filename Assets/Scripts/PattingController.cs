using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PattingController : MonoBehaviour {

    public GameObject imageText1;
    public GameObject imageText2;
    public GameObject imageText3;

    Button firstButton;
    Button secondButton;
    Button thirdButton;
    Button exitButton;

    // Use this for initialization
    void Start()
    {
        firstButton = transform.parent.Find("FirstButton").GetComponent<Button>();
        secondButton = transform.parent.Find("SecondButton").GetComponent<Button>();
        thirdButton = transform.parent.Find("ThirdButton").GetComponent<Button>();
        exitButton = transform.parent.Find("ExitButton").GetComponent<Button>();

        firstButton.onClick.AddListener(FirstButtonAction);
        secondButton.onClick.AddListener(SecondButtonAction);
        thirdButton.onClick.AddListener(ThirdButtonAction);
        exitButton.onClick.AddListener(ExitButtonAction);

        imageText1 = transform.parent.Find("ImageText1").gameObject;
        imageText2 = transform.parent.Find("ImageText2").gameObject;
        imageText3 = transform.parent.Find("ImageText3").gameObject;

        imageText2.SetActive(false);
        imageText3.SetActive(false);

        SceneInfo.currentState = SceneInfo.States.SHAKE_SCENE;
    }

    // Update is called once per frame


    public void FirstButtonAction()
    {
        PattingController pattingController;
        pattingController = transform.parent.Find("ButtonsController").GetComponent<PattingController>();

        pattingController.imageText1.SetActive(true);
        pattingController.imageText2.SetActive(false);
        pattingController.imageText3.SetActive(false);
    }

    public void SecondButtonAction()
    {
        PattingController pattingController;
        pattingController = transform.parent.Find("ButtonsController").GetComponent<PattingController>();

        pattingController.imageText1.SetActive(false);
        pattingController.imageText2.SetActive(true);
        pattingController.imageText3.SetActive(false);
    }

    public void ThirdButtonAction()
    {
        PattingController pattingController;
        pattingController = transform.parent.Find("ButtonsController").GetComponent<PattingController>();

        pattingController.imageText1.SetActive(false);
        pattingController.imageText2.SetActive(false);
        pattingController.imageText3.SetActive(true);
    }

    public void ExitButtonAction()
    {
        SceneInfo.currentState = SceneInfo.States.DRAGON_AFTER_TRAIN;
        LoadNextScene("DragonScene");
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
