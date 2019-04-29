using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMotion : MonoBehaviour {
    //Отвечает за движение облака

    private float screenCoef;
    private float speed = 0.05F;

	void Start () {
        screenCoef = (float)UserInfo.UserScreenSizeX / (float)UserInfo.UserScreenSizeY;

        SetStartPosition();
        SetStartScale();
	}

    void SetStartPosition()
    {
        Vector3 cloudPosition = transform.position;

        cloudPosition.y = Random.Range(1.0F,5.0F);
        cloudPosition.x = 10 * screenCoef + 1;
        transform.position = cloudPosition;
    }

    void SetStartScale()
    {
        float randomScale = Random.Range(0.4F, 1F);
        transform.localScale = new Vector3(randomScale, randomScale, randomScale);
    }

    // Update is called once per frame
    void Update () {

        if (SceneInfo.isRun)
            Move();

        if (transform.position.x < -5 * screenCoef-1)
            Destroy(this.gameObject, 1.0F);
	}
    void Move()
    {
        Vector3 cloudPos = transform.position;

        cloudPos.x -= speed;
        transform.position = cloudPos;
    }
}
