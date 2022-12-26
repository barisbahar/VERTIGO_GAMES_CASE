using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Transform dropitemlist,collecteditems;
    public GameObject collectitemprefab;
    [HideInInspector]
    public int levelindex = 1;
    public GameObject  WheelObject,stick,victorypanel;
    public Sprite silverwheel, goldenwheel, bronzewheel,silverstick,goldenstick,bronzestick;
    [SerializeField]
    private Button spinButton,collectButton,exitButton,CollectRestartButton,VictorRestartButton;
    
    private void OnEnable() {
        spinButton.onClick.AddListener(()=>Wheel.Instance.Spin());
        collectButton.onClick.AddListener(()=>Wheel.Instance.DropItemAddList());
        exitButton.onClick.AddListener(()=>LeaveGame());
        CollectRestartButton.onClick.AddListener(()=>RestartGame());
        VictorRestartButton.onClick.AddListener(()=>RestartGame());
    }
    private void OnDisable() {
        spinButton.onClick.RemoveListener(()=>Wheel.Instance.Spin());
        collectButton.onClick.RemoveListener(()=>Wheel.Instance.DropItemAddList());
        exitButton.onClick.RemoveListener(()=>LeaveGame());
        CollectRestartButton.onClick.RemoveListener(()=>RestartGame());
        VictorRestartButton.onClick.RemoveListener(()=>RestartGame());
    }
     private static UIManager _instance;
    public static UIManager Instance
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

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void LeaveGame(){
        victorypanel.gameObject.SetActive(true);
        for (int i = 1; i < dropitemlist.childCount; i++)
        {
           GameObject newcollectitem=  Instantiate(collectitemprefab,collecteditems);
            newcollectitem.transform.GetChild(0).GetComponent<Image>().sprite=dropitemlist.GetChild(i).GetChild(0).GetComponent<Image>().sprite;
            newcollectitem.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text=dropitemlist.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text;
        }
    }
    public void DesignWheel()
    {
        if (levelindex % 30 == 0)
        {
            WheelObject.GetComponent<SpriteRenderer>().sprite = goldenwheel;
            stick.GetComponent<SpriteRenderer>().sprite = goldenstick;
        }
        else if (levelindex % 5 == 0 )
        {
            WheelObject.GetComponent<SpriteRenderer>().sprite = silverwheel;
            stick.GetComponent<SpriteRenderer>().sprite = silverstick;
        }
        else
        {
            WheelObject.GetComponent<SpriteRenderer>().sprite = bronzewheel;
            stick.GetComponent<SpriteRenderer>().sprite = bronzestick;
        }
    }
}
