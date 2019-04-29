using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToolMotion : MonoBehaviour {

    bool isFalling;
    Sprite sprite;
    EventsManager evManager;
    private int id;
    private float screenCoef;
    private float speed = 0.05F;
    private string toolText;
    // Use this for initialization
    void Start()
    {
        evManager = GameObject.FindGameObjectWithTag("EventManager").GetComponent<EventsManager>();
        screenCoef = (float)UserInfo.UserScreenSizeX / (float)UserInfo.UserScreenSizeY;

        SetStartPosition();
    }

    public void SetID(int id)
    {
        this.id = id;

        switch (id)
        {
            case 3:
                sprite = Resources.Load<Sprite>("Sprites/Guitarra");
                toolText = "Василиса часто хандрит, вдруг это поможет";
                break;
            case 4:
                sprite = Resources.Load<Sprite>("Sprites/hammer");
                toolText = "С ними я до кого угодно достучусь";
                break;
            case 5:
                sprite = Resources.Load<Sprite>("Sprites/Cover");
                toolText = "Будет вместо каремата, который я проиграл в алкошашки";
                break;
            case 6:
                sprite = Resources.Load<Sprite>("Sprites/book");
                toolText = "Впереди – парящий мост. Надеюсь, Василиса опустит его для меня, иначе дальше не пройти";
                break;
            case 7:
                sprite = Resources.Load<Sprite>("Sprites/Pillow");
                toolText = "Она напоминает мне о дедушке";
                break;
            case 8:
                sprite = Resources.Load<Sprite>("Sprites/bottles");
                toolText = "Дальше – дом Карлсона, который живет не в доме. Никаких проблем не предвидится";
                break;
            case 9:
                sprite = Resources.Load<Sprite>("Sprites/basket");
                toolText = "Я понятия не имею, как это влезет в мой рюкзак, но не могу пройти мимо";
                break;
            case 10:
                sprite = Resources.Load<Sprite>("Sprites/shirt");
                toolText = "Я знаю много людей, которым она нужнее";
                break;
            case 11:
                sprite = Resources.Load<Sprite>("Sprites/Stumps");
                toolText = "Впереди – поле и старая вырубка. Не должно быть никаких сюрпризов";
                break;
            case 12:
                sprite = Resources.Load<Sprite>("Sprites/gun");
                toolText = "Оно должно выстрелить в третьем акте, я гарантирую это";
                break;
            case 13:
                sprite = Resources.Load<Sprite>("Sprites/expander");
                toolText = "Он просто обязан пригодиться, они все тут такие нервные";
                break;
            case 14:
                sprite = Resources.Load<Sprite>("Sprites/laxative");
                toolText = "Мой план Б, если массажер не сработает";
                break;
            case 15:
                sprite = Resources.Load<Sprite>("Sprites/KosheyForest");
                toolText = "Впереди лес Кощея. С ним точно будут сложности";
                break;
            case 16:
                sprite = Resources.Load<Sprite>("Sprites/Diary");
                toolText = "О, тяжёлые воспоминания, обожаю";
                break;
            case 17:
                sprite = Resources.Load<Sprite>("Sprites/Lair");
                toolText = "Я почти у логова. Надеюсь, меня Горыныч не сожрет";
                break;
            default:
                sprite = Resources.Load<Sprite>("Sprites/Grass");
                break;

        }

        transform.Find("Sprite").GetComponent<SpriteRenderer>().sprite = sprite;
    }

    void SetStartPosition()
    {
        Vector3 stonePosition = transform.position;

        stonePosition.y = -2;
        stonePosition.x = 10 * screenCoef + 1;
        transform.position = stonePosition;
    }


    // Update is called once per frame
    void Update()
    {
        if (SceneInfo.isRun && !isFalling)
            Move();

        if (Camera.main.WorldToScreenPoint(transform.position).x < UserInfo.UserScreenSizeX * 0.9 && SceneInfo.isRun &&!isFalling)
        {
            SceneInfo.isRun = false;

            GameObject father = GameObject.FindGameObjectWithTag("Player");
            father.GetComponent<Animator>().SetBool("IsStay", true);

            Vector3 speechPos = new Vector3(UserInfo.UserScreenSizeX / 2, UserInfo.UserScreenSizeY * 0.7F, 0);
            evManager.CreateNewSpeech(speechPos, 1, toolText);
        }

        if (Input.GetMouseButtonDown(0) && !SceneInfo.isRun)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Collider2D[] findedColliders = Physics2D.OverlapCircleAll(mousePos, 0.1F);

            foreach(Collider2D col in findedColliders)
            {
                if (col.gameObject == gameObject)
                {
                    switch (id)
                    {
                        case 6:
                            SceneInfo.currentState = SceneInfo.States.VASILISA_HELLO;
                            Destroy(evManager.currentSpeech);
                            LoadNextScene("VasilisaScene");
                            break;

                        case 8:
                            SceneInfo.currentState = SceneInfo.States.CARLSON_HELLO;
                            Destroy(evManager.currentSpeech);
                            LoadNextScene("CarlsonScene");
                            break;

                        case 11:
                            SceneInfo.currentState = SceneInfo.States.KYZYA_HELLO;
                            Destroy(evManager.currentSpeech);
                            LoadNextScene("KyzyaScene");
                            break;

                        case 15:
                            SceneInfo.currentState = SceneInfo.States.KOSHEY_HELLO;
                            Destroy(evManager.currentSpeech);
                            LoadNextScene("KosheyScene");
                            break;

                        case 17:
                            SceneInfo.currentState = SceneInfo.States.DRAGON_HELLO;
                            Destroy(evManager.currentSpeech);
                            LoadNextScene("DragonScene");
                            break;

                        default:
                            GameObject inventoryBar = GameObject.FindGameObjectWithTag("InventoryBar");
                            inventoryBar.GetComponent<InventoryBar>().FillEmptyCell(id, sprite, toolText);

                            GameObject father = GameObject.FindGameObjectWithTag("Player");
                            father.GetComponent<Animator>().SetBool("IsStay", false);

                            evManager.StartNextSpawn();
                            SceneInfo.isRun = true;

                            isFalling = true;
                            StartCoroutine("FallDown");
                            break;
                    }
                }
            }
        }
    }

    IEnumerator FallDown()
    {
        while(transform.localScale.x > 0.7)
        {
            Vector3 scale = transform.localScale;
            scale.x -= Time.deltaTime;
            scale.y = scale.z = scale.x;
            transform.localScale = scale;

            Vector3 targetPos = transform.position;
            targetPos.y = -5;

            transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime*5);

            yield return new WaitForSeconds(0.0001F);
        }

        Destroy(evManager.currentSpeech);
        Destroy(gameObject);

        yield return null;
    }
    void Move()
    {
        Vector3 stonePos = transform.position;

        stonePos.x -= speed;
        transform.position = stonePos;
    }

    void LoadNextScene(string sceneName)
    {
        GameObject darkPrefab = Resources.Load<GameObject>("Prefabs/dark");

        GameObject UICanvas = GameObject.FindGameObjectWithTag("UI");
        GameObject dark = Instantiate(darkPrefab, UICanvas.transform);

        NextSceneLoader loader = dark.GetComponent<NextSceneLoader>();

        loader.isDuffusing = false;
        loader.StartCoroutine("SetDark");
        loader.sceneName = sceneName;
    }
}
