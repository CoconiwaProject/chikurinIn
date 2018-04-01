using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContentListItem : MonoBehaviour
{

    public Image BGImage;
    public Text text;
    public Button button;

    public Color activeTextColor;
    public Color disabeleTextColor;

    ContentsData.Params m_params;

    //発見フラグ
    bool isActive = false;

    void Start()
    {
        button = GetComponent<Button>();
    }

    public void ContentSet(ContentsData.Params param)
    {
        if (PlayerPrefs.GetInt("GetContents" + param.FileID) != 0)
        {
            isActive = true;
            button.interactable = true;
            text.color = activeTextColor;
        }
        else
        {
            text.color = disabeleTextColor;
        }

        text.text = param.ContentsName;
        m_params = param;
    }

    public void JumpScene()
    {
        AppData.SelectTargetName = m_params.FileID;
        UnderBerMenu.I.ChangeScene("Content");
    }
}
