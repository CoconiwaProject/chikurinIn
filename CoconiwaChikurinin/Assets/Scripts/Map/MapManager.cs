using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : SingletonMonoBehaviour<MapManager>
{
    public float screenSizeRate = 1.0f;

    ContentsData contentsData = null;

    public Image namePopUp = null;
    [SerializeField]
    List<Image> namePopUpList = new List<Image>();
    public List<Sprite> balloonImageList = new List<Sprite>();

    List<MapMaker> makerList = new List<MapMaker>();
    MapMaker currentMaker;
    public Sprite notFindImage = null;

    bool isTap = false;

    public Action OnTapped;
    float startDraggingTime = 0.0f;

    protected override void Awake()
    {
        base.Awake();
        //とりあえず横だけ
        screenSizeRate = 1.0f + (1.0f - ((float)Screen.width / 1080));
        PlayerPrefs.SetInt("GetContents" + "I-01", 1);
    }

    protected override void Start()
    {
        base.Start();
        contentsData = AppData.ContentsData;
        List<GameObject> tempList = new List<GameObject>();
        tempList.AddRange(GameObject.FindGameObjectsWithTag("Maker"));
        for (int i = 0; i < tempList.Count; i++)
        {
            makerList.Add(tempList[i].GetComponent<MapMaker>());
        }
        TouchManager.Instance.TouchEnd += OnTouchEnd;
        TouchManager.Instance.TouchStart += OnTouchStart;
    }


    protected override void OnDestroy()
    {
        base.OnDestroy();
        OnTapped = null;
#if !UNITY_EDITOR
        TouchManager.Instance.TouchEnd -= OnTouchEnd;
        TouchManager.Instance.TouchStart -= OnTouchStart;
#endif
    }


    public string GetContentName(string name)
    {
        int index = contentsData.Elements.FindIndex(n => n.FileID == name);

        if (index == -1) return "エラー";

        return contentsData.Elements[index].ContentsName;
    }

    public void TouchMaker(string fileID, MapMaker maker)
    {
        TappedTheButton();
        currentMaker = maker;
        for (int i = 0; i < makerList.Count; i++)
        {
            makerList[i].IsSelect = false;
        }

        for (int i = 0; i < namePopUpList.Count; i++)
        {
            namePopUpList[i].gameObject.SetActive(false);
        }

        int index = (int)NamePopUp.GetMakerSize(GetContentName(fileID));
        namePopUp = namePopUpList[index];
    }

    public void TappedTheButton()
    {
        isTap = false;
    }

    void OnTouchStart(object sender, CustomInputEventArgs e)
    {
        isTap = true;

        startDraggingTime = Time.time;
    }

    void OnTouchEnd(object sender, CustomInputEventArgs e)
    {
        //スワイプを検出されていればisTapはfalseになる
        if (Time.time - startDraggingTime > 0.2f) return;
        if (!isTap) return;

        Tapped();
    }
    
    void Tapped()
    {
        if (OnTapped != null) OnTapped.Invoke();
        if (currentMaker == null) return;
        if (!currentMaker.IsSelect) return;

        currentMaker.IsSelect = false;
        namePopUp.gameObject.SetActive(false);
    }
}
