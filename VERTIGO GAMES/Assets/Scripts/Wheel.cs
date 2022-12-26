using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Wheel : MonoBehaviour
{
    float angleitem;
    int idx;
    bool spinning = false;
    [System.Serializable]

    public class Data
    {
        public Sprite obj;
        public int droprate;
    }
    public List<Data> Items;
    public List<Data> usingitem;
    public List<Data> specialrewards = new List<Data>();
    public GameObject dropitemprefab, itemcard, exitbutton;
    public Button SpinButton;
    private bool isSlotFinish;
    private Sprite Circle, DropItemSprite;
    private static Wheel _instance;
    public static Wheel Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("Null");
            }
            return _instance;
        }
    }
    private void Awake()
    {
        _instance = this;
    }
    private void Start()
    {
        angleitem = 360 / 8;
        Circle = UIManager.Instance.WheelObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
        SlotItem();
        UIManager.Instance.DesignWheel();
    }
    private void SlotItem()
    {
        Items.AddRange(usingitem);
        usingitem.Clear();
        for (int i = 0; i < UIManager.Instance.WheelObject.transform.childCount; i++)
        {
            UIManager.Instance.WheelObject.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = Circle;
        }
        for (int i = 0; i < 8; i++)
        {
            int Deathindex = Items.IndexOf(Items.FirstOrDefault(i => i.obj.name == "ui_card_icon_death"));
            if (UIManager.Instance.levelindex % 5 != 0 && i == 0)
            {
                UIManager.Instance.WheelObject.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = Items.ElementAt(Deathindex).obj;
                usingitem.Add(Items.ElementAt(Deathindex));
                Items.RemoveAt(Deathindex);
            }
            else if (UIManager.Instance.levelindex % 30 == 0)
            {

                for (int j = 0; j < Items.Count; j++)
                {
                    if (Items.ElementAt(j).droprate <= 10 && Items.ElementAt(j).obj.name != "ui_card_icon_death")
                    {
                        specialrewards.Add(Items.ElementAt(j));
                        Items.RemoveAt(j);
                    }
                }
                int SelectItem = Random.Range(0, specialrewards.Count);
                UIManager.Instance.WheelObject.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = specialrewards.ElementAt(SelectItem).obj;
                usingitem.Add(specialrewards.ElementAt(SelectItem));
                specialrewards.RemoveAt(SelectItem);
            }
            else
            {
                if (UIManager.Instance.levelindex % 5 == 0)
                {
                    int SelectItem;
                    do
                    {
                        SelectItem = Random.Range(0, Items.Count());
                    } while (SelectItem == Deathindex);
                    UIManager.Instance.WheelObject.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = Items.ElementAt(SelectItem).obj;
                    usingitem.Add(Items.ElementAt(SelectItem));
                    Items.RemoveAt(SelectItem);
                }
                else
                {
                    int SelectItem = Random.Range(0, Items.Count());
                    UIManager.Instance.WheelObject.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = Items.ElementAt(SelectItem).obj;
                    usingitem.Add(Items.ElementAt(SelectItem));
                    Items.RemoveAt(SelectItem);
                }
            }
        }
    }
    public int DropItem()
    {
        List<Sprite> items = new List<Sprite>();
        foreach (Data d in usingitem)
        {
            items.AddRange(Enumerable.Repeat(d.obj, d.droprate).ToList());
        }
        int randomNumber = Random.Range(0, items.Count);
        DropItemSprite = items.ElementAt(randomNumber);
        return usingitem.IndexOf(usingitem.First(i => i.obj == DropItemSprite));
    }
    public void DropItemAddList()
    {
        bool itemAdded = false;
        for (int i = 1; i < UIManager.Instance.dropitemlist.childCount; i++)
        {
            if (UIManager.Instance.dropitemlist.GetChild(i).GetChild(0).GetComponent<Image>().sprite == UIManager.Instance.WheelObject.transform.GetChild(idx).GetComponent<SpriteRenderer>().sprite)
            {
                string[] value = UIManager.Instance.dropitemlist.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text.Split(' ');
                UIManager.Instance.dropitemlist.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = "x " + (int.Parse(value[1]) + 1).ToString();
                itemAdded = true;
            }
        }
        if (itemAdded != true)
        {
            GameObject dropitem = Instantiate(dropitemprefab, UIManager.Instance.dropitemlist);
            dropitem.transform.GetChild(0).GetComponent<Image>().sprite = UIManager.Instance.WheelObject.transform.GetChild(idx).GetComponent<SpriteRenderer>().sprite;
        }
        itemcard.SetActive(false);
        SlotItem();
    }

    public void Spin()
    {
        if (spinning == false)
        {
            idx = DropItem();
            float focusangle = 360 * 3 + (idx * angleitem);
            StartCoroutine(SpinWheel(4, focusangle));
        }
    }

    IEnumerator SpinWheel(float time, float maxAngle)
    {
        SpinButton.interactable = false;
        exitbutton.SetActive(false);
        spinning = true;
        float timer = 0.0f;
        float startAngle = UIManager.Instance.WheelObject.transform.eulerAngles.z;
        maxAngle -= startAngle;
        while (timer < time)
        {
            float angle = maxAngle * (timer / time);
            UIManager.Instance.WheelObject.transform.eulerAngles = new Vector3(0.0f, 0.0f, angle + startAngle);
            timer += Time.deltaTime;
            yield return 0;
        }
        UIManager.Instance.WheelObject.transform.eulerAngles = new Vector3(0.0f, 0.0f, maxAngle + startAngle);
        yield return new WaitForSeconds(0.5f);
        itemcard.SetActive(true);
        itemcard.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = UIManager.Instance.WheelObject.transform.GetChild(idx).GetComponent<SpriteRenderer>().sprite;
        if (UIManager.Instance.WheelObject.transform.GetChild(idx).GetComponent<SpriteRenderer>().sprite.name == "ui_card_icon_death")
        {
            itemcard.transform.GetChild(0).transform.GetChild(3).gameObject.SetActive(true);
            itemcard.transform.GetChild(0).transform.GetChild(2).gameObject.SetActive(false);
            itemcard.transform.GetChild(0).transform.GetChild(0).gameObject.SetActive(false);
            itemcard.transform.GetChild(0).GetComponent<RawImage>().color = Color.red;
        }
        else
        {
            itemcard.transform.GetChild(0).transform.GetChild(2).gameObject.SetActive(true);
            itemcard.transform.GetChild(0).transform.GetChild(3).gameObject.SetActive(false);
        }
        spinning = false;
        LevelSwipe.Instance.NextLevel();
        SpinButton.interactable = true;
        exitbutton.SetActive(true);
        UIManager.Instance.levelindex++;
        UIManager.Instance.DesignWheel();
    }
}
