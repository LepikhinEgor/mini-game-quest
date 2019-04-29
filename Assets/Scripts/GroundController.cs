using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundController : MonoBehaviour
{

    private float timer = 0;
    private GameObject stonePrefab;
    private ArrayList stones;

    private void Awake()
    {
        stonePrefab = (GameObject)Resources.Load("Prefabs/Stone");
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
            GameObject newStone = Instantiate(stonePrefab, transform);
            timer = 0.5F;
        }
    }
}

