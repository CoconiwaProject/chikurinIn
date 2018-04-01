using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoadManager : SingletonMonoBehaviour<SceneLoadManager>
{
    public enum FadeType { FadeOut, FadeIn }

    [SerializeField]
    Image panel = null;

    Coroutine fadeCoroutine = null;

    Color clearColor = new Color(1.0f, 1.0f, 1.0f, 0.0f);
    [SerializeField]
    Color fadeColor = Color.white;

    const float m_transitionTime = 0.3f;

    public bool IsFading { get; private set;}

    Action onSceneLoaded;

    public string CurrentSceneName { get; private set; }
    public string OldSceneName { get; private set; }

    protected override void WasLoaded(Scene sceneName, LoadSceneMode sceneMode)
    {
        base.WasLoaded(sceneName, sceneMode);

        CurrentSceneName = sceneName.name;
        //FadeOutが必要だということ
        if(panel.gameObject.activeSelf)
        {
            Fade(FadeType.FadeOut, m_transitionTime, () =>
             {
                 if(onSceneLoaded != null) onSceneLoaded.Invoke();
                 onSceneLoaded = null;
            });
        }
    }

    /// <summary>
    /// ただSceneManager.LoadSceneを呼ぶだけ
    /// </summary>
    public void LoadScene(string sceneName, LoadSceneMode mode = LoadSceneMode.Single)
    {
        OldSceneName = CurrentSceneName;
        try
        {
            SceneManager.LoadScene(sceneName, mode);
        }
        catch
        {
            Debug.LogError(sceneName + " is not found");
        }
    }

    /// <summary>
    /// ただSceneManager.LoadSceneAsyncを呼ぶだけ
    /// </summary>
    public AsyncOperation LoadSceneAsync(string sceneName, LoadSceneMode mode = LoadSceneMode.Single)
    {
        OldSceneName = CurrentSceneName;
        try
        {
            return SceneManager.LoadSceneAsync(sceneName, mode);
        }
        catch
        {
            Debug.LogError(sceneName + " is not found");
            return null;
        }
    }

    /// <summary>
    /// フェード付きのSceneLoad
    /// </summary>
    public void SceneTransition(string sceneName, float transitionTime = m_transitionTime, LoadSceneMode mode = LoadSceneMode.Single)
    {
        Fade(FadeType.FadeIn, transitionTime, () =>
        {
            LoadScene(sceneName, mode);
        });
    }

    /// <summary>
    /// フェード付きのSceneLoad(callback付き)
    /// </summary>
    public void SceneTransition(string sceneName, Action onFadeInCompleted, Action onFadeOutCompleted, float transitionTime = m_transitionTime, LoadSceneMode mode = LoadSceneMode.Single)
    {
        this.onSceneLoaded = onFadeOutCompleted;
        Fade(FadeType.FadeIn, transitionTime, () =>
        {
            if(onFadeInCompleted != null) onFadeInCompleted.Invoke();
            LoadScene(sceneName, mode);
        });
    }

    /// <summary>
    /// FadeIn(徐々にFadeColorにする)もしくはFadeOut(徐々にClearColorに)する
    /// </summary>
    public void Fade(FadeType type, float duration, Action callBack = null)
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        panel.gameObject.SetActive(true);
        IsFading = true;
        MyCoroutine fadeAnimation;

        if (type == FadeType.FadeIn)
        {
            fadeAnimation = GetFadeAnimation(clearColor, fadeColor, duration);
        }
        else
        {
            fadeAnimation = GetFadeAnimation(fadeColor, clearColor, duration).OnCompleted(()=>
            {
                panel.gameObject.SetActive(false);
                //fade outするまではIsFadingはtrueで
                IsFading = false;
            });
        }

        fadeAnimation.OnCompleted(() =>
        {
            if (callBack != null) callBack.Invoke();
            fadeCoroutine = null;
        });

        fadeCoroutine = StartCoroutine(fadeAnimation);
    }

    MyCoroutine GetFadeAnimation(Color startColor, Color endColor, float duration)
    {
        return KKUtilities.FloatLerp(duration, (t) =>
        {
            panel.color = Color.Lerp(startColor, endColor, t * t);
        });
    }
}
