using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ThetaButton : MonoBehaviour
{
    Button m_button;

    [SerializeField]
    Button popup = null;
    public Button Popup
    {
        set { popup = value; }
    }

    [SerializeField]
    Texture[] texs = null;

    bool isSelect = false;
    
    static ThetaButton currentSelectButton;

    [SerializeField]
    bool canChangePicture = false;

    [SerializeField]
    Vector3 differenceVec = Vector3.zero;

    void Awake()
    {
        m_button = GetComponent<Button>();

        m_button.onClick.AddListener(() =>
        {
            MapManager.I.TappedTheButton();
            if (isSelect)
            {
                TransitionViewTHETAScene();
            }
            else
            {
                isSelect = true;
                if (currentSelectButton != null) currentSelectButton.Hide();
                currentSelectButton = this;
                PopUp();
            }
        });

        popup.onClick.AddListener(() =>
        {
            MapManager.I.TappedTheButton();
            TransitionViewTHETAScene();
        });

    }

    void Start()
    {
        //ボタン以外をタップした時の挙動
        MapManager.I.OnTapped += OnTapped;
    }

    void OnTapped()
    {
        if (currentSelectButton == null) return;

        if (isSelect) Hide();
    }

    void PopUp()
    {
        popup.gameObject.SetActive(true);
    }

    void Hide()
    {
        isSelect = false;
        popup.gameObject.SetActive(false);
    }

    void TransitionViewTHETAScene()
    {
        currentSelectButton = null;
        AppData.SelectThetaPictures = texs;
        AppData.CanChangePicture = canChangePicture;
        AppData.differenceVec = differenceVec;
        UnderBerMenu.I.ChangeScene("ViewTHETA");
    }
}
