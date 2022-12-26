using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class LevelSwipe : MonoBehaviour
{
    private static LevelSwipe _instance;
    public static LevelSwipe Instance
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
    public GameObject scrollbar, levelprefab;
    public Texture whitepanel, greenpanel;

    float scroll_pos = 0;
    float[] pos;
    private void Start()
    {
        for (int i = 1; i <= 80; i++)
        {
            GameObject currentlevel = Instantiate(levelprefab, this.gameObject.transform);
            currentlevel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = i.ToString();
            if (i % 5 == 0)
            {
                currentlevel.GetComponent<RawImage>().texture = greenpanel;
            currentlevel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color=Color.white;
            }
            else
                currentlevel.GetComponent<RawImage>().texture = whitepanel;
        }
    }
    public void NextLevel()
    {
        scroll_pos = scrollbar.GetComponent<Scrollbar>().value += 0.0125f;
    }
    private void Update()
    {
        pos = new float[transform.childCount];
        float distance = 1f / (pos.Length - 1f);
        for (int i = 0; i < pos.Length; i++)
        {
            pos[i] = distance * i;
        }
        if (Input.GetMouseButton(5))
        {
            scroll_pos = scrollbar.GetComponent<Scrollbar>().value;
        }
        else
        {
            for (int i = 0; i < pos.Length; i++)
            {
                if (scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2))
                {
                    scrollbar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollbar.GetComponent<Scrollbar>().value, pos[i], 0.1f);
                }
            }
        }
        for (int i = 0; i < pos.Length; i++)
        {
            if (scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2))
            {
                transform.GetChild(i).localScale = Vector2.Lerp(transform.GetChild(i).localScale, new Vector2(1f, 1f), 0.1f);
                for (int a = 0; a < pos.Length; a++)
                {
                    if (a != i)
                    {
                        transform.GetChild(a).localScale = Vector2.Lerp(transform.GetChild(a).localScale, new Vector2(0.8f, 0.8f), 0.1f);
                    }
                }
            }
        }
    }
}

