using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShouldersController : MonoBehaviour {

    Button exitButton;

    // Use this for initialization
    void Start()
    {
        exitButton = transform.parent.Find("ExitButton").GetComponent<Button>();
        exitButton.onClick.AddListener(ExitButtonAction);

    }

    // Update is called once per frame


    public void ExitButtonAction()
    {
        SceneInfo.currentState = SceneInfo.States.KOSHEY_AFTER_TRAIN;
        LoadNextScene("KosheyScene");
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
