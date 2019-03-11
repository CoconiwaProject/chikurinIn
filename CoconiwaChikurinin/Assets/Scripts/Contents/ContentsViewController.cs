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

    //0=押してない状態、1=押している
    [SerializeField]
    List<Sprite> upButtonSprite = null;

    [SerializeField]
    List<Sprite> outButtonSprite = null;

    [SerializeField]
    Button scaleButton = null;

    [SerializeField]
    RectTransform canTapRange = null;

    const float TapRangeRatio = 1.9f;

    bool canTapScaleButton = false;

    bool upMade = false;

    float downStartTime = 0;

    public void SetInit(Sprite sprite)
    {
        controlImage.sprite = sprite;
        //サイズ位置調整する
        controlImage.SetNativeSize();//もともとのにしてから
        //サイズ変更
        float x = controlImage.rectTransform.sizeDelta.x * maxYSize / controlImage.rectTransform.sizeDelta.y;
        //controlImage.rectTransform.sizeDelta *=  0.5f;
        controlImage.rectTransform.sizeDelta = new Vector2(x, maxYSize);
        // Debug.Log(controlImage.rectTransform.sizeDelta);
        //位置変更
        controlImage.rectTransform.anchoredPosition = Vector2.zero;
    }

    /// <summary>
    /// モード切替
    /// </summary>
    public void PushUpBotton()
    {
        if (upMade == false)
        {
            upMade = true;
            canTapScaleButton = true;
            TapScaleImage(true);

            canTapRange.sizeDelta = new Vector2(canTapRange.sizeDelta.x, canTapRange.sizeDelta.y * TapRangeRatio);

            scaleButton.GetComponent<Image>().sprite = outButtonSprite[0];
            SpriteState sprite;
            sprite.pressedSprite = outButtonSprite[1];
            sprite.highlightedSprite = outButtonSprite[0];
            sprite.disabledSprite = outButtonSprite[0];
            scaleButton.spriteState = sprite;

            controlImage.gameObject.SetActive(true);
            StartCoroutine(KKUtilities.FloatLerp(0.3f, (t) =>
            {
                contentGroup.alpha = Mathf.Lerp(1, 0, t);
            }));

        }
        else
        {
            TapScaleImage(false);
            canTapRange.sizeDelta = new Vector2(canTapRange.sizeDelta.x, canTapRange.sizeDelta.y / TapRangeRatio);

            scaleButton.GetComponent<Image>().sprite = upButtonSprite[0];
            SpriteState sprite;
            sprite.pressedSprite = upButtonSprite[1];
            sprite.highlightedSprite = upButtonSprite[0];
            sprite.disabledSprite = upButtonSprite[0];
            scaleButton.spriteState = sprite;

            controlImage.gameObject.SetActive(false);
            StartCoroutine(KKUtilities.FloatLerp(0.3f, (t) =>
            {
                contentGroup.alpha = Mathf.Lerp(0, 1, t);
            }));
            upMade = false;
        }
    }

    //移動演出、
    public void TapScaleImage(bool changeMode = false)
    {
        if (upMade == false)
        {
            return;
        }

        if (changeMode == false)
        {
            float tapJudgeTime = 0.4f;
            if (Time.time - downStartTime > tapJudgeTime)
                return;
        }
        float drawButtonPosition = 478;

        if (canTapScaleButton)
        {
            canTapScaleButton = false;
            StartCoroutine(KKUtilities.FloatLerp(0.3f, (t) =>
            {
                scaleButton.GetComponent<RectTransform>().anchoredPosition
                = new Vector2(Mathf.Lerp(drawButtonPosition, drawButtonPosition + scaleButton.GetComponent<RectTransform>().sizeDelta.x, t),
                scaleButton.GetComponent<RectTransform>().anchoredPosition.y);
            }));
        }
        else
        {
            canTapScaleButton = true;
            StartCoroutine(KKUtilities.FloatLerp(0.3f, (t) =>
            {
                scaleButton.GetComponent<RectTransform>().anchoredPosition
                = new Vector2(Mathf.Lerp(drawButtonPosition + scaleButton.GetComponent<RectTransform>().sizeDelta.x, drawButtonPosition, t),
                scaleButton.GetComponent<RectTransform>().anchoredPosition.y);
            }));
        }
        downStartTime = 0;
    }

    public void StartDawn()
    {
        //時間が長いとスライド時も出てしまうので
        downStartTime = Time.time;
    }
}
