using System;
using UnityEngine;
using UnityEngine.UI;
public class AboutItem : MonoBehaviour
{
    [SerializeField]
    Text title = null;

    [SerializeField]
    Button button;

    public void Init(string title, string fileID)
    {
        this.title.text = title;
        
        button.onClick.AddListener(() =>
        {
            AppData.SelectTargetName = fileID;
            UnderBerMenu.I.ChangeScene("Content");
        });
    }

    public void Init(string title, Action onClick)
    {
        this.title.text = title;

        button.onClick.AddListener(() =>
        {
            if(onClick != null) onClick.Invoke();
        });
    }
}
