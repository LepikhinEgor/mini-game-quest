using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryBar : MonoBehaviour {
    private int cellsFilled = 0;
    private GameObject inventory;
    private GameObject firstButton;
    public GameObject[] items;
    private GameObject itemPrefab;

    //расстояние между первым касанием, и позицией инвернтаря
    private float oldInventoryPosX;
    private float newInventoryPosX;
    private float deltaInventoryPosX;
    //количество кардров затухания
    private int dampingFrameCount;
    private int currentDampingFrame;
    private bool isMoving = false;
    private bool isDamping = false;

    private int firstItemNum = -1;

    private void Awake()
    {
        itemPrefab = (GameObject)Resources.Load("Prefabs/InventoryItem");
    }

    void Start () {

        SetBarSizeFromProperties();

        transform.Find("Button").GetComponent<ImventoryView>().FindInventoryPositions();

        items = GameObject.FindGameObjectsWithTag("InventoryItem");
        SortItems();

        firstButton = items[0];

        inventory = transform.Find("Inventory").gameObject;

        dampingFrameCount = 60;

        SetItemsStartPos();
        FillInventory();
    }

    void ClearInventory()
    {
        for (int i = 0; i < items.Length; i++)
            items[i].GetComponent<InventoryItem>().isEmpty = true;
    }

    public void RemoveItem(int id)
    {
        SceneInfo.inventoryItems.Remove(id);
        for (int i = 0; i < items.Length; i++)
            if (items[i].GetComponent<InventoryItem>().id == id)
            {
                items[i].GetComponent<InventoryItem>().isEmpty = true;
                items[i].transform.Find("ThingSprite(Clone)").gameObject.SetActive(false);
            }
    }

    private void FillInventory()
    {
        ClearInventory();
        // Заполняет инвентарь предметами
        Sprite toolSprite;
        string toolText;

        foreach (int toolId in SceneInfo.inventoryItems)
        {
            switch (toolId)
            {
                case 0:
                    toolSprite = Resources.Load<Sprite>("Sprites/WaterCase");
                    toolText = "Вода. Мокрая и в бутылке. Можно на кого-нибудь вылить, кого-нибудь напоить или помыть.";
                    break;
                case 1:
                    toolSprite = Resources.Load<Sprite>("Sprites/Scacalka");
                    toolText = "Скакалка.Три дня и три ночи скакал на ней Иван-дурак.";
                    break;
                case 2:
                    toolSprite = Resources.Load<Sprite>("Sprites/boxDevil");
                    toolText = "Чёртик в коробочке. Посмотри, как он мотает головой. Напоминает счастливую собаку.";
                    break;
                case 3:
                    toolSprite = Resources.Load<Sprite>("Sprites/Guitarra");
                    toolText = "Гитара. Песни, пляски, как красиво.";
                    break;
                case 4:
                    toolSprite = Resources.Load<Sprite>("Sprites/hammer");
                    toolText = "Молоток и гвозди. Можно что-нибудь прибить или на что-нибудь забить.";
                    break;
                case 5:
                    toolSprite = Resources.Load<Sprite>("Sprites/Cover");
                    toolText = "Коврик для фитнеса. Добром заставим их полюбить физнагрузки!";
                    break;
                case 6:
                    toolSprite = Resources.Load<Sprite>("Sprites/book");
                    toolText = "Впереди – парящий мост. Надеюсь, Василиса опустит его для меня, иначе дальше не пройти";
                    break;
                case 7:
                    toolSprite = Resources.Load<Sprite>("Sprites/Pillow");
                    toolText = "Кислородная подушка. Иногда лучше дышать, чем не дышать.";
                    break;
                case 8:
                    toolSprite = Resources.Load<Sprite>("Sprites/bottles");
                    toolText = "Дальше – дом Карлсона, который живет не в доме. Никаких проблем не предвидится";
                    break;
                case 9:
                    toolSprite = Resources.Load<Sprite>("Sprites/basket");
                    toolText = "Баскетбольное кольцо. Если нужно что-нибудь бросить, подбросить и не добросить.";
                    break;
                case 10:
                    toolSprite = Resources.Load<Sprite>("Sprites/shirt");
                    toolText = "Смирительная рубашка. Твоё последнее слово в споре с тем, кто слабее.";
                    break;
                case 11:
                    toolSprite = Resources.Load<Sprite>("Sprites/Stumps");
                    toolText = "Впереди – поле и старая вырубка. Не должно быть никаких сюрпризов";
                    break;
                case 12:
                    toolSprite = Resources.Load<Sprite>("Sprites/gun");
                    toolText = "Ружье.Твоё последнее слово в споре с тем, кто сильнее.";
                    break;
                case 13:
                    toolSprite = Resources.Load<Sprite>("Sprites/expander");
                    toolText = "Массажер. М-м-мы-ы-ы-у-у-у-ру-ру…";
                    break;
                case 14:
                    toolSprite = Resources.Load<Sprite>("Sprites/laxative");
                    toolText = "Упаковка слабительного. Ша, уже никто никуда не спешит.";
                    break;
                case 15:
                    toolSprite = Resources.Load<Sprite>("Sprites/KosheyForest");
                    toolText = "Впереди лес Кощея. С ним точно будут сложности";
                    break;
                case 16:
                    toolSprite = Resources.Load<Sprite>("Sprites/Diary");
                    toolText = "Дневник злодея. Что это за бурые пятна на страницах? Бр-р, волосы в жилах стынут";
                    break;
                case 17:
                    toolSprite = Resources.Load<Sprite>("Sprites/Lair");
                    toolText = "Я почти у логова. Надеюсь, меня Горыныч не сожрет";
                    break;
                default:
                    toolSprite = Resources.Load<Sprite>("Sprites/Grass");
                    toolText = "Undefined";
                    break;

            }

            FillEmptyCell(toolId, toolSprite, toolText);
        }
    }

    public void SetBarSizeFromProperties()
    {
        RectTransform barTranform = GetComponent<RectTransform>();

        Vector2 leftBottomCorner = barTranform.anchorMin;
        leftBottomCorner.y = 0;
        barTranform.anchorMin = leftBottomCorner;

        Vector2 rightTopCorner = barTranform.anchorMax;
        rightTopCorner.y = Properties.InventoryShare;
        barTranform.anchorMax = rightTopCorner;
    }

    public void AddItem()
    {
        //Добавляет новую ячейку в инвентрарь
        GameObject newItem = Instantiate(itemPrefab, transform.Find("Inventory"));

        Vector3 newItemPos = newItem.transform.position;
        newItemPos.x = 9999999;
        newItem.transform.position = newItemPos;

        items = GameObject.FindGameObjectsWithTag("InventoryItem");

        SetItemsStartPos();
    }


    void FindFirstItemNum()
    {
        // находит ближайший к левой стороне экрана элемент в инвентаре
        SortItems();

        Debug.Log("FindFirstItemNum");

        firstItemNum = -1;
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == firstButton)
            {
                firstItemNum = i;
            }
        }

        if (firstItemNum == -1)
        {
            Debug.Log("Ошибка, не удалось определить первый элемент в инвентаре");
        }
    }

    void SetItemsStartPos()
    {
        //Ровно расставляет элементы инвентаря сначала

        SortItems();
        FindFirstItemNum();

        Vector3 itemPos;
        for (int i = 0; i < items.Length; i++)
        {
            if (i <= firstItemNum)
            {
                itemPos = items[i].transform.position;
                itemPos.y = firstButton.transform.position.y;
                itemPos.x = i * UserInfo.UserScreenSizeX / 4 + UserInfo.UserScreenSizeX / 8;
                items[i].transform.position = itemPos;
            }
            else
            {
                itemPos = items[i].transform.position;
                itemPos.y = firstButton.transform.position.y;
                itemPos.x = i * UserInfo.UserScreenSizeX / 4 + UserInfo.UserScreenSizeX / 8;
                items[i].transform.position = itemPos;
            }
            //items[i].GetComponent<InventoryItem>().id = i;
        }
    }


    void SortItems()
    {
        //сортирует элементы в инвенторе в порядке возрастания координаты x
        for (int i = 0; i < items.Length; i++)
            for (int j = 0; j < items.Length - i - 1; j++)
            {
                if (items[j].transform.position.x > items[j + 1].transform.position.x)
                {
                    GameObject temp = items[j];
                    items[j] = items[j + 1];
                    items[j + 1] = temp;
                }
            }
    }
	
    void Motion()
    {
        //Движение инвентаря за пальцем при касании
        Vector3 currInventoryPosition = inventory.transform.position;

        //движение за пальцем
        deltaInventoryPosX = oldInventoryPosX - newInventoryPosX;
        currInventoryPosition.x -= deltaInventoryPosX;

        //дальше идут проверки на выход за границы
        //если выходит, то возвращается в максимально допустимую
        int lastItemNum = items.Length - 1;
        float lastItemPosXOld = items[lastItemNum].transform.position.x + UserInfo.UserScreenSizeX/8;
        float firstItemPosXOld = items[0].transform.position.x - UserInfo.UserScreenSizeX / 8;

        Vector3 tempPos = inventory.transform.position;

        inventory.transform.position = currInventoryPosition;

        if (items[lastItemNum].transform.position.x + UserInfo.UserScreenSizeX / 8 <= UserInfo.UserScreenSizeX &&
            deltaInventoryPosX > 0)
        {
            float distanse = UserInfo.UserScreenSizeX - lastItemPosXOld;
            tempPos.x += distanse;
            inventory.transform.position = tempPos;
        }

        if (items[0].transform.position.x - UserInfo.UserScreenSizeX / 8 >= 0 &&
            deltaInventoryPosX < 0)
        {
            float distanse = 0 - firstItemPosXOld;
            tempPos.x += distanse;
            inventory.transform.position = tempPos;
        }

        oldInventoryPosX = newInventoryPosX;
    }

    void Damping()
    {
        //плавное затухание движения инвентаря
        if (currentDampingFrame == 0)
        {
            //окончание затухания
            isDamping = false;
            SetFixedPosition();
            return;
        }
        //уменьшение скорости движения
        deltaInventoryPosX -= deltaInventoryPosX / dampingFrameCount;

        //изменение положение
        Vector3 currInventoryPosition = inventory.transform.position;
        currInventoryPosition.x -= deltaInventoryPosX;
        inventory.transform.position = currInventoryPosition;

        //проверки на выход за пределы
        int lastItemNum = items.Length - 1;
        float lastItemPosXOld = items[lastItemNum].transform.position.x + UserInfo.UserScreenSizeX / 8;
        float firstItemPosXOld = items[0].transform.position.x - UserInfo.UserScreenSizeX / 8;

        Vector3 tempPos = inventory.transform.position;

        inventory.transform.position = currInventoryPosition;

        if (items[lastItemNum].transform.position.x + UserInfo.UserScreenSizeX / 8 <= UserInfo.UserScreenSizeX &&
            deltaInventoryPosX > 0)
        {
            float distanse = UserInfo.UserScreenSizeX - lastItemPosXOld;
            tempPos.x += distanse;
            inventory.transform.position = tempPos;
            isDamping = false;
        }

        if (items[0].transform.position.x - UserInfo.UserScreenSizeX / 8 >= 0 &&
            deltaInventoryPosX < 0)
        {
            float distanse = 0 - firstItemPosXOld;
            tempPos.x += distanse;
            inventory.transform.position = tempPos;
            isDamping = false;
        }

        currentDampingFrame--;
    }

    void SetFixedPosition()
    {
        //Выравнивает элементы в инвентаре после их движения
        items =  GameObject.FindGameObjectsWithTag("InventoryItem");

        float minPosx = 999999;
        foreach (GameObject item in items)
        {
            if (System.Math.Abs(item.transform.position.x - UserInfo.UserScreenSizeX / 8) < System.Math.Abs(minPosx))
            {
                firstButton = item;
                minPosx = item.transform.position.x;
            }
        }

        FindFirstItemNum();

        float offset = firstButton.transform.position.x - UserInfo.UserScreenSizeX/8;

        Vector3 inventoryPos = inventory.transform.position;
        inventoryPos.x -= offset;
        inventory.transform.position = inventoryPos;
    }

    public void FillEmptyCell(int itemNum, Sprite sprite, string description)
    {
        //добавляет инструмент в первую пустую ячейку инвентаря
        if (cellsFilled >= 4)
            AddItem();

        cellsFilled++;

        SortItems();
        int emptyCellNum = GetEmptyCellNum();
        Debug.Log(emptyCellNum);
        items[emptyCellNum].GetComponent<InventoryItem>().AddThing(itemNum, sprite, description, emptyCellNum);

        Debug.Log(itemNum + " заполнен");
    }

    int GetEmptyCellNum()
    {
        //Ищет номер пустой ячейки в инвентаре
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].GetComponent<InventoryItem>().isEmpty)
                return i;
        }

        Debug.Log("Не удалось найти пустую ячейку");

        return 0;
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Touch in" + Input.mousePosition);

            Vector2 touchPos = Input.mousePosition;

            if (touchPos.y < UserInfo.UserScreenSizeY * Properties.InventoryShare 
                && !SceneInfo.inventoryIsHiden && !SceneInfo.isSelectItem)
            {
                isMoving = true;

                oldInventoryPosX = touchPos.x;
                newInventoryPosX = oldInventoryPosX;
            }

        }

        if (Input.GetMouseButton(0))
        {
            if (isMoving)
                newInventoryPosX = Input.mousePosition.x;
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (isMoving)
            {
                FindFirstItemNum();

                if (System.Math.Abs(deltaInventoryPosX) > 1)
                    isDamping = true;
                else
                    SetFixedPosition();

                currentDampingFrame = dampingFrameCount;

                isMoving = false;
            }
        }

        if (isMoving)
            Motion();

        if (isDamping) 
            Damping();
	}
}
