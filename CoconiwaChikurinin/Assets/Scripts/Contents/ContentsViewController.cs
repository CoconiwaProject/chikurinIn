using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContentsViewController : MonoBehaviour
{
    [SerializeField]
    Image controlImage = null;

    [SerializeField]
    float maxYSize = 0;

    [SerializeField]
    CanvasGroup contentGroup = null;

    bool upMade = false;

    public void SetInit(Sprite sprite)
    {
        controlImage.sprite= sprite;
        //サイズ位置調整する
        controlImage.SetNativeSize();//もともとのにしてから
        //サイズ変更
        float x = controlImage.rectTransform.sizeDelta.x * maxYSize / controlImage.rectTransform.sizeDelta.y;
        //controlImage.rectTransform.sizeDelta *=  0.5f;
        controlImage.rectTransform.sizeDelta = new Vector2(x,maxYSize);
       // Debug.Log(controlImage.rectTransform.sizeDelta);
        //位置変更
        controlImage.rectTransform.anchoredPosition = Vector2.zero;
    }

    /// <summary>
    /// モード切替
    /// </summary>
    public void PushUpBotton()
    {
        if(upMade==false)
        {
            controlImage.gameObject.SetActive(true);
            StartCoroutine(KKUtilities.FloatLerp(0.3f, (t) =>
            {
                contentGroup.alpha = Mathf.Lerp(1, 0, t);
            }));
            upMade = true;
        }
        else
        {
            controlImage.gameObject.SetActive(false);
            StartCoroutine(KKUtilities.FloatLerp(0.3f, (t) =>
            {
                contentGroup.alpha = Mathf.Lerp(0, 1, t);
            }));
            upMade = false;
        }
    }
}
