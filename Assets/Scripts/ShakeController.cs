using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShakeController : MonoBehaviour {

    public GameObject image1;
    public GameObject image2;
    public GameObject imageText1;
    public GameObject imageText2;

    Button leftButton;
    Button rightButton;
    Button exitButton;

	// Use this for initialization
	void Start () {
        leftButton = transform.parent.Find("LeftButton").GetComponent<Button>();
        rightButton = transform.parent.Find("RightButton").GetComponent<Button>();
        exitButton = transform.parent.Find("ExitButton").GetComponent<Button>();

        leftButton.onClick.AddListener(LeftButtonAction);
        rightButton.onClick.AddListener(RightButtonAction);
        exitButton.onClick.AddListener(ExitButtonAction);

        image1 = transform.parent.Find("Image1").gameObject;
        image2 = transform.parent.Find("Image2").gameObject;
        imageText1 = transform.parent.Find("TextImage1").gameObject;
        imageText2 = transform.parent.Find("TextImage2").gameObject;

        image2.SetActive(false);
        imageText2.SetActive(false);

        SceneInfo.currentState = SceneInfo.States.SHAKE_SCENE;
	}
	
	// Update is called once per frame


    public void LeftButtonAction()
    {
        ShakeController shakeController;
        shakeController = transform.parent.Find("ButtonsController").GetComponent<ShakeController>();

        shakeController.image1.SetActive(true);
        shakeController.imageText1.SetActive(true);

        shakeController.image2.SetActive(false);
        shakeController.imageText2.SetActive(false);
    }

    public void RightButtonAction()
    {
        ShakeController shakeController;
        shakeController = transform.parent.Find("ButtonsController").GetComponent<ShakeController>();

        shakeController.image1.SetActive(false);
        shakeController.imageText1.SetActive(false);

        shakeController.image2.SetActive(true);
        shakeController.imageText2.SetActive(true);
    }

    public void ExitButtonAction()
    {
        SceneInfo.currentState = SceneInfo.States.KYZYA_AFTER_TRAIN;
        LoadNextScene("KyzyaScene");
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
