using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour {

    private string toolDescription;
    private GameObject movableTool;
    private bool isMovingTool = false;
    public int id;
    public int cellNum;
    public bool isEmpty = true;
    private Sprite thingSprite;

    private Color defaultColor;
    private GameObject itemInfoPrefab;
    private void Awake()
    {
        itemInfoPrefab = (GameObject)Resources.Load("Prefabs/InventoryItemInfo");
    }

    // Use this for initialization
    void Start () {
        defaultColor = GetComponent<Image>().color;
        GetComponent<Button>().onClick.AddListener(Select);
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetMouseButtonDown(0))
        {
            float halfButtonX = UserInfo.UserScreenSizeX / 8;
            float halfButtonY = UserInfo.UserScreenSizeY*Properties.InventoryShare/2;

            Vector3 mousePos = Input.mousePosition;
            if (SceneInfo.selectItemNum == cellNum)
            {
                if (mousePos.x < transform.position.x + halfButtonX
                && mousePos.x > transform.position.x - halfButtonX
                && mousePos.y < transform.position.y + halfButtonY
                && mousePos.y > 0)
                {
                    GameObject thingPrefab = (GameObject)Resources.Load("Prefabs/ThingSprite");
                    movableTool = Instantiate(thingPrefab, transform);

                    movableTool.GetComponent<Image>().sprite = thingSprite;

                    isMovingTool = true;
                }
                else
                    SelectOff();
            }
        }

        if (Input.GetMouseButton(0) && isMovingTool == true)
        {
            movableTool.transform.position = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0) && isMovingTool == true)
        {
            isMovingTool = false;

            Destroy(movableTool);
            movableTool.transform.position = Input.mousePosition;
            SelectOff();

            if(Input.mousePosition.y > UserInfo.UserScreenSizeY*Properties.InventoryShare)
            {
                string curSceneName = SceneManager.GetActiveScene().name;
                GameObject personDialog;
                switch (curSceneName)
                {
                    case "VasilisaScene":
                        personDialog = GameObject.FindGameObjectWithTag("VasDialog");

                        personDialog.GetComponent<VasilisaDialog>().reactionNum = id;
                        personDialog.GetComponent<VasilisaDialog>().StartCoroutine("StartVasilisaReaction");
                        break;
                    case "CarlsonScene":
                        personDialog = GameObject.FindGameObjectWithTag("CarlsonDialog");

                        personDialog.GetComponent<CarlsonDialog>().reactionNum = id;
                        personDialog.GetComponent<CarlsonDialog>().StartCarlsonReaction();
                        break;
                    case "KyzyaScene":
                        personDialog = GameObject.FindGameObjectWithTag("KyzyaDialog");

                        personDialog.GetComponent<KyzyaDialog>().reactionNum = id;
                        personDialog.GetComponent<KyzyaDialog>().StartCarlsonReaction();
                        break;
                    case "KosheyScene":
                        personDialog = GameObject.FindGameObjectWithTag("KosheyDialog");

                        personDialog.GetComponent<KosheyDialog>().reactionNum = id;
                        personDialog.GetComponent<KosheyDialog>().StartCarlsonReaction();
                        break;
                    case "DragonScene":
                        personDialog = GameObject.FindGameObjectWithTag("DragonDialog");

                        personDialog.GetComponent<DragonDialog>().reactionNum = id;
                        personDialog.GetComponent<DragonDialog>().StartCarlsonReaction();
                        break;
                }
            }
        }
    }

    void Select()
    {
        //выделяет выбранный элемент
        if (SceneInfo.isInteractiveMoment && !isEmpty)
        {
            if (SceneInfo.inventoryItemInfo)
                Destroy(SceneInfo.inventoryItemInfo);

            if(SceneInfo.selectItemNum != -1)
            {
                GameObject invBar = GameObject.FindGameObjectWithTag("InventoryBar");
                if (SceneInfo.isSelectItem)
                {
                    GameObject oldSelItem = invBar.GetComponent<InventoryBar>().items[SceneInfo.selectItemNum];
                    oldSelItem.GetComponent<InventoryItem>().SelectOff();
                }
            }

            SceneInfo.selectItemNum = cellNum;
            SceneInfo.isSelectItem = true;

            AchivementsInfo.inventoryItemsChecked[id] = true;
            
            string s = "";
            for (int i = 0; i < AchivementsInfo.inventoryItemsChecked.Length; i++)
            {
                if (AchivementsInfo.inventoryItemsChecked[i])
                    s += 1.ToString();
                else
                    s += 0.ToString();
            }

            Debug.Log(s);

            Debug.Log(id + "-" + cellNum);

            GetComponent<Image>().color = Color.yellow;

            //окно с описанием инструмента
            SceneInfo.inventoryItemInfo = (GameObject)Instantiate(itemInfoPrefab, transform.parent.parent.parent);
            SceneInfo.inventoryItemInfo.transform.Find("Text").GetComponent<Text>().text = toolDescription;
        }
    }

    public void SelectOff()
    {
        // убирает выделение
        SceneInfo.selectItemNum = -1;
        SceneInfo.isSelectItem = false;

        GetComponent<Image>().color = defaultColor;
        Destroy(SceneInfo.inventoryItemInfo);
    }

    public void AddThing(int thingID, Sprite sprite, string description, int cellNum)
    {
        //добавляет инструмент в эту ячейку
        GameObject thingPrefab = (GameObject)Resources.Load("Prefabs/ThingSprite");

        GameObject spriteGO = Instantiate(thingPrefab, transform);

        thingSprite = sprite;
        spriteGO.GetComponent<Image>().sprite = thingSprite;

        SceneInfo.inventoryItems.Add(thingID);
        id = thingID;
        toolDescription = description;

        this.cellNum = cellNum;

        isEmpty = false;

        Debug.Log(thingID + "-" + cellNum);
    }

    public void Clear()
    {
        GameObject tool;
        if (transform.Find("ThingSprite(Clone)") != null)
        {
            tool = transform.Find("ThingSprite(Clone)").gameObject;
            Destroy(tool);
        }
        cellNum = 0;
        id = 0;
        isEmpty = true;
    }
}
