using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneMotion : MonoBehaviour {

    private float screenCoef;
    private float speed = 0.05F;
    // Use this for initialization
    void Start()
    {
        screenCoef = (float)UserInfo.UserScreenSizeX / (float)UserInfo.UserScreenSizeY;

        SetStartPosition();
        SetStartScale();
        SetRandAlphaChannel();
    }

    void SetRandAlphaChannel()
    {
        Color color = GetComponent<SpriteRenderer>().color;

        color.a = Random.Range(0.3F,1.0F);
        GetComponent<SpriteRenderer>().color = color;
    }

    void SetStartPosition()
    {
        Vector3 stonePosition = transform.position;

        stonePosition.y = Random.Range(-2.0F, -4.0F);
        stonePosition.x = 10 * screenCoef + 1;
        transform.position = stonePosition;
    }

    void SetStartScale()
    {
        float randomScale = Random.Range(0.1F, 0.6F);
        transform.localScale = new Vector3(randomScale, randomScale, randomScale);
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneInfo.isRun)
            Move();

        if (transform.position.x < -5 * screenCoef - 1)
            Destroy(this.gameObject, 1.0F);
    }
    void Move()
    {
        Vector3 stonePos = transform.position;

        stonePos.x -= speed;
        transform.position = stonePos;
    }
}
