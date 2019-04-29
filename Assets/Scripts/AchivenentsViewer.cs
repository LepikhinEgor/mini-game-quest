using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchivenentsViewer : MonoBehaviour {

    private float currAchivementTopAnchor = 0.78F;
    private float achivementHeight = 0.08F;
    GameObject canvas;

    GameObject achivementPrefab;

	void Start () {
        achivementPrefab = Resources.Load<GameObject>("Prefabs/Achivement");
        canvas = GameObject.FindGameObjectWithTag("UI");

        AchivementsInfo.CalculateAchivements();

        StartCoroutine("AddingAchivements");
	}
	
    IEnumerator AddingAchivements()
    {
        yield return new WaitForSeconds(2);

        if (AchivementsInfo.isBustler)
        {
            string achName = "Торопыга";
            string achDescription = "Прошел игру меньше чем за 10 минут";
            Color color = new Color(209.0F / 255, 255.0F / 255, 191.0F / 255); ;

            CreateAchivement(achName, achDescription, color);

            yield return new WaitForSeconds(0.2F);
        }

        if (AchivementsInfo.isKopysha)
        {
            string achName = "Копуша";
            string achDescription = "Прошел игру больше чем за 20 минут";
            Color color = new Color(209.0F / 255, 255.0F / 255, 191.0F / 255);

            CreateAchivement(achName, achDescription, color);

            yield return new WaitForSeconds(0.2F);
        }

        if (AchivementsInfo.isSniper)
        {
            string achName = "Снайпер";
            string achDescription = "Прошел игру не совершив ни одной ошибки";
            Color color = new Color(209.0F / 255, 255.0F / 255, 191.0F / 255);

            CreateAchivement(achName, achDescription, color);

            yield return new WaitForSeconds(0.2F);
        }

        if (AchivementsInfo.isVasilis)
        {
            string achName = "Василис Премудрый";
            string achDescription = "Почти все решил с первого раза";
            Color color = new Color(209.0F / 255, 255.0F / 255, 191.0F / 255);

            CreateAchivement(achName, achDescription, color);

            yield return new WaitForSeconds(0.2F);
        }

        if (AchivementsInfo.isGoodBoy)
        {
            string achName = "Хороший мальчик";
            string achDescription = "Не навредил ни одному персонажу из Сказоземья";
            Color color = new Color(209.0F / 255, 255.0F / 255, 191.0F / 255);

            CreateAchivement(achName, achDescription, color);

            yield return new WaitForSeconds(0.2F);
        }

        if (AchivementsInfo.isVarvaraNose)
        {
            string achName = "Нос Варвары";
            string achDescription = "Прочитал все описания вещей в инвентаре";
            Color color = new Color(209.0F / 255, 255.0F / 255, 191.0F / 255);

            CreateAchivement(achName, achDescription, color);

            yield return new WaitForSeconds(0.2F);
        }

        if (AchivementsInfo.isPedant)
        {
            string achName = "Педант";
            string achDescription = "Попробовал все варианты помощи персонажам Сказаземья";
            Color color = new Color(209.0F/255, 255.0F/255, 191.0F/255);

            CreateAchivement(achName, achDescription, color);

            yield return new WaitForSeconds(0.2F);
        }

        if (AchivementsInfo.fingerInSky)
        {
            string achName = "Тыкатель пальцем в небо";
            string achDescription = "Допустил немного ошибок пока проходил до конца";
            Color color = new Color(240.0F/255, 240.0F / 255, 240.0F / 255);

            CreateAchivement(achName, achDescription, color);

            yield return new WaitForSeconds(0.2F);
        }

        if (AchivementsInfo.isExperimenter)
        {
            string achName = "Эксперементатор";
            string achDescription = "Старался использовать все что есть";
            Color color = new Color(240.0F / 255, 240.0F / 255, 240.0F / 255);

            CreateAchivement(achName, achDescription, color);

            yield return new WaitForSeconds(0.2F);
        }

        if (AchivementsInfo.isNenastoyashik)
        {
            string achName = "Ненастоящик";
            string achDescription = "Живой человек так бы не смог ошибаться. Ты же специалльно, да?";
            Color color = new Color(255.0F/255, 190.0F/255, 190.0F/255);

            CreateAchivement(achName, achDescription, color);

            yield return new WaitForSeconds(0.2F);
        }

        if (AchivementsInfo.isFailureControlAnger)
        {
            string achName = "Неуправление гневом";
            string achDescription = "Дал молоток Василисе";
            Color color = new Color(255.0F / 255, 190.0F / 255, 190.0F / 255);

            CreateAchivement(achName, achDescription, color);

            yield return new WaitForSeconds(0.2F);
        }

        if (AchivementsInfo.dontFeedAnimals)
        {
            string achName = "Зверей не кормить";
            string achDescription = "Связал Кузю из-за чего его съели";
            Color color = new Color(255.0F / 255, 190.0F / 255, 190.0F / 255);

            CreateAchivement(achName, achDescription, color);

            yield return new WaitForSeconds(0.2F);
        }

        if (AchivementsInfo.isCynic)
        {
            string achName = "Жуткий циник";
            string achDescription = "Пытался убить кощея из ружья";
            Color color = new Color(255.0F / 255, 190.0F / 255, 190.0F / 255);

            CreateAchivement(achName, achDescription, color);

            yield return new WaitForSeconds(0.2F);
        }

        if (AchivementsInfo.braveryAndStupidity)
        {
            string achName = "Слабоумие и отвага";
            string achDescription = "Дал смирительную рубашку Горынычу";
            Color color = new Color(255.0F / 255, 190.0F / 255, 190.0F / 255);

            CreateAchivement(achName, achDescription, color);

            yield return new WaitForSeconds(0.2F);
        }

        if (AchivementsInfo.isBadBoy)
        {
            string achName = "Плохиш";
            string achDescription = "Дал всем вредные советы";
            Color color = new Color(255.0F / 255, 190.0F / 255, 190.0F / 255);

            CreateAchivement(achName, achDescription, color);

            yield return new WaitForSeconds(0.2F);
        }

    }

    void CreateAchivement(string name, string description, Color color)
    {
        GameObject newAchivement = Instantiate(achivementPrefab, canvas.transform);

        Debug.Log(color);
        Debug.Log(newAchivement.GetComponent<Image>().color);
        newAchivement.GetComponent<Image>().color = color;
        newAchivement.transform.Find("NameText").GetComponent<Text>().text = name;
        newAchivement.transform.Find("Description").GetComponent<Text>().text = description;
        Debug.Log(newAchivement.GetComponent<Image>().color);

        //сдвиг достижения вниз от предыдущего
        Vector2 anchorMax = newAchivement.GetComponent<RectTransform>().anchorMax;
        Vector2 anchorMin = newAchivement.GetComponent<RectTransform>().anchorMin;

        anchorMax.y = currAchivementTopAnchor;
        anchorMin.y = anchorMax.y - achivementHeight;

        newAchivement.GetComponent<RectTransform>().anchorMax = anchorMax;
        newAchivement.GetComponent<RectTransform>().anchorMin = anchorMin;

        currAchivementTopAnchor -= achivementHeight;
    }
	// Update is called once per frame
	void Update () {
		
	}
}
