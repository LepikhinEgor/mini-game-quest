using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudsController : MonoBehaviour
{
    //Создает облака

    private float timer = 0;
    private GameObject cloudPrefab;

    private int cloudsCount = 5;

    private void Awake()
    {
        cloudPrefab = (GameObject)Resources.Load("Prefabs/Cloud");
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        if (timer < 0 && SceneInfo.isRun)
        {
            GameObject newCloud = Instantiate(cloudPrefab, transform);
            timer = 1.5F;
        }
    }
}
