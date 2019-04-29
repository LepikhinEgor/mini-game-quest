using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour {

    GameObject[] parts;
    GameObject slider;
    private const int partsNum = 8;
    private int sceneNum = 0;
    private float sliderSpeed;
	// Use this for initialization
	void Start () {
        parts = new GameObject[6];
        parts[0] = transform.Find("GrayPart").gameObject;
        parts[1] = transform.Find("GreenPart").gameObject;
        parts[2] = transform.Find("RedPart").gameObject;
        parts[3] = transform.Find("BluePart").gameObject;
        parts[4] = transform.Find("PinkPart").gameObject;
        parts[5] = transform.Find("OrangePart").gameObject;

        slider = transform.Find("Slider").gameObject;

        SetPartsPositions();
        SetStartSliderPos();
        SetIconsStartPos();
	}
	

    void SetPartsPositions()
    {
        //задает кускам линии позиции в зависимости от текущей сцены
        switch (SceneInfo.currentState)
        {
            case SceneInfo.States.RUN_BEFORE_VASILISA:
                sceneNum = 1;
                break;
            case SceneInfo.States.RUN_AFTER_VASILISA:
                sceneNum = 2;
                break;
            case SceneInfo.States.RUN_AFTER_CARLSON:
                sceneNum = 3;
                break;
            case SceneInfo.States.RUN_AFTER_KYZYA:
                sceneNum = 4;
                break;
            case SceneInfo.States.RUN_AFTER_KOSHEY:
                sceneNum = 5;
                break;
        }

        for (int i = 0; i < parts.Length; i++)
        {
            Vector2 leftLowerCorner = parts[i].GetComponent<RectTransform>().anchorMin;
            Vector2 rightTopCorner = parts[i].GetComponent<RectTransform>().anchorMax;

            if (i < sceneNum)
            {
                leftLowerCorner.x = i * (1.0F / partsNum);
                rightTopCorner.x = (i + 1) * (1.0F / partsNum);
            }

            if (i == sceneNum)
            {
                leftLowerCorner.x = i * (1.0F / partsNum);
                rightTopCorner.x = (i + 3) * (1.0F / partsNum);
            }

            if (i > sceneNum)
            {
                leftLowerCorner.x = (i + 2) * (1.0F / partsNum);
                rightTopCorner.x = (i + 3) * (1.0F / partsNum);
            }

            parts[i].GetComponent<RectTransform>().anchorMin = leftLowerCorner;
            parts[i].GetComponent<RectTransform>().anchorMax = rightTopCorner;
        }
    }

    void SetStartSliderPos()
    {
        //задает стартовую позицию бегунку в зависимости от текущей сцены

        Vector2 sliderAnchorMin = slider.GetComponent<RectTransform>().anchorMin;
        Vector2 sliderAnchorMax = slider.GetComponent<RectTransform>().anchorMax;

        sliderAnchorMin.x = parts[sceneNum].GetComponent<RectTransform>().anchorMin.x;
        Debug.Log(sliderAnchorMin);
        sliderAnchorMax.x = sliderAnchorMin.x + 0.01F;

        slider.GetComponent<RectTransform>().anchorMin = sliderAnchorMin;
        slider.GetComponent<RectTransform>().anchorMax = sliderAnchorMax;

        int wayLenght = 0;
        switch (sceneNum)
        {
            case 1:
                wayLenght = 4;
                break;
            case 2:
                wayLenght = 2;
                break;
            case 3:
                wayLenght = 3;
                break;
            case 4:
                wayLenght = 4;
                break;
            case 5:
                wayLenght = 2;
                break;
        }

       sliderSpeed = 3 * (1.0F / partsNum) / ((Properties.timeBetweenTools+1.6F) * wayLenght);
    }

    void SetIconsStartPos()
    {
        GameObject icons = transform.parent.Find("Icons").gameObject;

        Vector3 partPos = parts[0].transform.position;
        partPos.y = icons.transform.Find("Prolog").position.y;
        icons.transform.Find("Prolog").position = partPos;

        partPos = parts[1].transform.position;
        partPos.y = icons.transform.Find("Vasilisa").position.y;
        icons.transform.Find("Vasilisa").position = partPos;

        partPos = parts[2].transform.position;
        partPos.y = icons.transform.Find("Carlson").position.y;
        icons.transform.Find("Carlson").position = partPos;

        partPos = parts[3].transform.position;
        partPos.y = icons.transform.Find("Kyzya").position.y;
        icons.transform.Find("Kyzya").position = partPos;

        partPos = parts[4].transform.position;
        partPos.y = icons.transform.Find("Koshey").position.y;
        icons.transform.Find("Koshey").position = partPos;

        partPos = parts[5].transform.position;
        partPos.y = icons.transform.Find("Dragon").position.y;
        icons.transform.Find("Dragon").position = partPos;
    }

    // Update is called once per frame
    void Update () {
		if (SceneInfo.isRun)
        {
            Vector2 sliderAnchorMin = slider.GetComponent<RectTransform>().anchorMin;
            Vector2 sliderAnchorMax = slider.GetComponent<RectTransform>().anchorMax;

            sliderAnchorMin.x += sliderSpeed*Time.deltaTime;
            sliderAnchorMax.x += sliderSpeed*Time.deltaTime;

            slider.GetComponent<RectTransform>().anchorMin = sliderAnchorMin;
            slider.GetComponent<RectTransform>().anchorMax = sliderAnchorMax;
        }
	}
}
