using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UnderBerMenu : SingletonMonoBehaviour<UnderBerMenu>
{
    [SerializeField]
    Button[] m_Icons = null;

    AudioSource m_audioSource;

    [SerializeField]
    GameObject rootObj = null;

    SceneLoadManager sceneLoadManager;

    protected override void Start()
    {
        base.Start();
        m_audioSource = GetComponent<AudioSource>();
        sceneLoadManager = SceneLoadManager.I;
    }

    public void ChangeIconActive(string nextSceneName)
    {
        //カメラ起動画面に飛んだ場合書き換える。　めんどくさいんで許してちょ
        if (nextSceneName == "CameraStarting")
        {
            nextSceneName = "Camera";
        }
        //めんどくさいんで許してちょ
        if (nextSceneName == "ContentList")
        {
            nextSceneName = "Content";
        }

        for (int i = 0; i < m_Icons.Length; i++)
        {
            if (m_Icons[i].gameObject.name == nextSceneName)
            {
                //選択できないようにする
                m_Icons[i].interactable = false;
            }
            else
            {
                //選択できるようにする
                m_Icons[i].interactable = true;
            }
        }
    }

    /// <summary>
    /// Iconの切り替えも行う
    /// </summary>
    public void ChangeScene(string loadSceneName)
    {
        if (sceneLoadManager.IsFading) return;

        if (loadSceneName == "Content" && string.IsNullOrEmpty(AppData.SelectTargetName))
        {
            loadSceneName = "ContentList";
        }

        m_audioSource.Play();
        sceneLoadManager.SceneTransition(loadSceneName, () =>
        {
            ChangeIconActive(loadSceneName);
        }, null);
    }

    public void ChangeTutorialScene(string loadSceneName)
    {
        if (sceneLoadManager.IsFading) return;
        SceneLoadManager.I.SceneTransition(loadSceneName, () => UnderBerMenu.I.SetUnderBerActive(false), () => UnderBerMenu.I.transform.SetSiblingIndex(1));
        m_audioSource.Play();
    }

    public void ChangeHomeScene(string loadSceneName)
    {
        if (sceneLoadManager.IsFading) return;
        UnderBerMenu.I.transform.SetSiblingIndex(0);

        SceneLoadManager.I.SceneTransition(loadSceneName, () => UnderBerMenu.I.SetUnderBerActive(true),()=> UnderBerMenu.I.transform.SetSiblingIndex(1));

        m_audioSource.Play();
    }


    /// <summary>
    /// 前のシーンをロードする
    /// </summary>
    public void LoadPreviousScene()
    {
        if(sceneLoadManager.OldSceneName == "Load" || sceneLoadManager.OldSceneName == "Camera")
        {
            ChangeScene("CameraStarting");
            return;
        }

        ChangeScene(sceneLoadManager.OldSceneName);
    }

    /// <summary>
    /// 表示、非表示はこちらで
    /// </summary>
    public void SetUnderBerActive(bool isActive)
    {
        rootObj.SetActive(isActive);
    }
}
