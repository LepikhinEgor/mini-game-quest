using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour {
    //обрабатывает кнопку "играть" из начальной сцены

	void Start () {
        SceneInfo.currentState = SceneInfo.States.MAIN_MENU;
        GetComponent<Button>().onClick.AddListener(StartPlay);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void StartPlay()
    {
        LoadNextScene("PrologueScene");
        SceneInfo.currentState = SceneInfo.States.PROLOGUE_DIALOG;
    }

    void LoadNextScene(string sceneName)
    {
        GameObject darkPrefab = Resources.Load<GameObject>("Prefabs/dark");

        GameObject UICanvas = GameObject.FindGameObjectWithTag("UI");
        GameObject dark = Instantiate(darkPrefab, UICanvas.transform);

        NextSceneLoader loader = dark.GetComponent<NextSceneLoader>();

        loader.isDuffusing = false;
        loader.StartCoroutine("SetDark");
        loader.sceneName = "PrologueScene";
    }
}
