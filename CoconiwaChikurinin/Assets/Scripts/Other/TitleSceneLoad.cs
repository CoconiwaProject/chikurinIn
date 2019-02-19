using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleSceneLoad : MonoBehaviour
{
    public Image Panel;
    public string SceneName;
    public float duration = 1.0f;

    private void Start()
    {
         //日本語以外は英語に
        if (Application.systemLanguage==SystemLanguage.Japanese)
        {
            AppData.UsedLanguage = SystemLanguage.Japanese;
        }
        else
        {
            AppData.UsedLanguage = SystemLanguage.English;
        }
        //  AppData.UsedLanguage = SystemLanguage.Japanese;
    }

    public void NextScene()
    {
        if (SceneLoadManager.I.IsFading) return;

        UnderBerMenu menu = UnderBerMenu.I;
        //menuをpanelの後ろに持ってくる
        menu.transform.SetSiblingIndex(0);

        bool isFirst = false;
        if (!PlayerPrefs.HasKey("Init"))
        {
            isFirst = true;
            SceneName = "Tutorial";
        }

        SceneLoadManager.I.SceneTransition(SceneName, () =>
        {
            if (isFirst)
                menu.SetUnderBerActive(false);
            else
                menu.SetUnderBerActive(true);

        }, () =>
        {
            if (!isFirst)
                menu.transform.SetSiblingIndex(1);
        }, duration);
    }
}
