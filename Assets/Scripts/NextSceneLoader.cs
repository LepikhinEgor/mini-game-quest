using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NextSceneLoader : MonoBehaviour {
    //Загружает следующую сцену с эффектом затемнения

    public bool isDuffusing = true;
    public string sceneName;

    Image darkImage;
    Color currColor;

    // Use this for initialization
    void Start () {
        if (isDuffusing)
            StartCoroutine("DiffuseDark");
	}


    IEnumerator SetDark()
    {
        //постепенно затемняет экран
        currColor = Color.clear;

        darkImage = GetComponent<Image>();
        darkImage.color = currColor;

        while (currColor.a < 1)
        {
            currColor.a += Time.deltaTime/1.5F;
            darkImage.color = currColor;
            yield return new WaitForEndOfFrame();
        }

        SceneManager.LoadScene(sceneName);
        yield return null;
    }

    IEnumerator DiffuseDark()
    {
        //постепенно рассеивает затеменение
        currColor = Color.black;

        darkImage = GetComponent<Image>();
        darkImage.color = currColor;

        while (currColor.a > 0)
        {
            if (!isDuffusing)
                yield return null;
            currColor.a -= Time.deltaTime/1.5F;
            darkImage.color = currColor;
            yield return new WaitForEndOfFrame();
        }
        Destroy(this.gameObject);
        yield return null;

    }

    // Update is called once per frame
    void Update () {
		
	}
}
