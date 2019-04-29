using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImventoryView : MonoBehaviour {

    private bool isHideAnimation = false;
    private bool isShowAnimation = false;

    Vector3 hidenInventoryPos;
    Vector3 defaultInventoryPos;

	void Start () {
        GetComponent<Button>().onClick.AddListener(HideInventory);
    }
	
    public void FindInventoryPositions()
    {
        //высота кнопки в процентах
        float buttonHeight = GetComponent<RectTransform>().anchorMax.y - GetComponent<RectTransform>().anchorMin.y;
        // в пикселях
        buttonHeight *= UserInfo.UserScreenSizeY * Properties.InventoryShare;

        defaultInventoryPos = transform.parent.position;

        float offset = transform.parent.position.y - 0;
        Vector3 pos = transform.parent.position;
        pos.y -= offset * 2 - buttonHeight;
        hidenInventoryPos = pos;
    }

	// Update is called once per frame
	void Update () {

        if (isHideAnimation)
        {
            transform.parent.position = Vector3.MoveTowards(transform.parent.position, hidenInventoryPos, 500 * Time.deltaTime);

            if (transform.parent.position.y == hidenInventoryPos.y)
                isHideAnimation = false;
        }

        if (isShowAnimation)
        {
            transform.parent.position = Vector3.MoveTowards(transform.parent.position, defaultInventoryPos, 500 * Time.deltaTime);

            if (transform.parent.position.y == defaultInventoryPos.y)
            {
                isShowAnimation = false;
                SceneInfo.inventoryIsHiden = false;
            }
        }
    }

    void HideInventory()
    {
        //передвигает инвентарь вниз
        GetComponent<Button>().onClick.AddListener(ShowInventory);

        SceneInfo.inventoryIsHiden = true;

        isHideAnimation = true;
        isShowAnimation = false;
    }

    void ShowInventory()
    {
        //возвращает инвентарь на место
        GetComponent<Button>().onClick.AddListener(HideInventory);

        isHideAnimation = false;
        isShowAnimation = true;
    }
}
