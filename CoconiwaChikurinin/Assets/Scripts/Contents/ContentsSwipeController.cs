using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContentsSwipeController : SingletonMonoBehaviour<ContentsSwipeController>
{
    [SerializeField]
    float fitSpeed = 3.0f;
    [SerializeField]
    float swipeSpeed = 1.0f;
    [SerializeField]
    ContentsPageControl pageControl = null;

    const float screenWidth = 1080.0f;
    float screenSizeRate;
    TouchManager touchManager;
    RectTransform rec;
    int imageNum;

    int currentSelectIndex = 0;
    Coroutine fitCoroutine;
    float maxPositionX;
    float swipeStartX = 0.0f;

    bool isChangeImage = false;

    void Initialize()
    {
        touchManager = TouchManager.Instance;

        touchManager.Drag += OnSwipe;

        touchManager.TouchEnd += OnTouchEnd;
        touchManager.FlickStart += OnFlickStart;
        touchManager.FlickComplete += OnFlickComplete;

        rec = transform as RectTransform;
        maxPositionX = (screenWidth * (imageNum - 1)) * -1.0f;
        screenSizeRate = 1.0f + (1.0f - (Screen.width / screenWidth));
    }

    void OnDisable()
    {
        if (touchManager != null)
        {
            touchManager.Drag -= OnSwipe;
            touchManager.TouchEnd -= OnTouchEnd;
            touchManager.FlickStart -= OnFlickStart;
            touchManager.FlickComplete -= OnFlickComplete;
        }
    }

    public void SetImageNum(int imageNum)
    {
        //イメージの数が１枚以下ならスワイプする必要はない
        if (imageNum <= 1) return;

        this.imageNum = imageNum;
        Initialize();
    }

    void OnSwipe(object sender, CustomInputEventArgs e)
    {
        Vector2 inputVec = Vector2.zero;
        inputVec.x = e.Input.DeltaPosition.x;
        Vector2 currentPosition = rec.anchoredPosition;
        currentPosition += inputVec * screenSizeRate * swipeSpeed;

        currentPosition.x = Mathf.Clamp(currentPosition.x, maxPositionX, 0.0f);

        rec.anchoredPosition = currentPosition;
    }

    void OnFlickStart(object sender, FlickEventArgs e)
    {
        isChangeImage = false;
        swipeStartX = e.StartInput.ScreenPosition.x;
    }

    void OnFlickComplete(object sender, FlickEventArgs e)
    {
        if (isChangeImage) return;
        //フリックを検出したら今の位置からではなくフリックした方向で選択しているイメージを判断する
        float flickvalue = swipeStartX - e.EndInput.ScreenPosition.x;
        float borderValue = 20.0f;
        if (Mathf.Abs(flickvalue) < borderValue) return;
        
        int targetIndex = currentSelectIndex + (flickvalue > 0 ? 1 : -1);
        FitImage(targetIndex);
    }

    void OnTouchEnd(object sender, CustomInputEventArgs e)
    {
        int index = GetCurrentSelectImageIndex(swipeStartX < e.Input.ScreenPosition.x);
        FitImage(index);
    }

    void FitImage(int index)
    {
        index = Mathf.Clamp(index, 0, imageNum - 1);

        isChangeImage = index != currentSelectIndex;
        currentSelectIndex = index;
        if (fitCoroutine != null)
        {
            StopCoroutine(fitCoroutine);
        }

        float targetX = -screenWidth * index;
        float startX = rec.anchoredPosition.x;
        //何秒かけてtargetXにするかはtargetXまでの距離で判断する
        float duration = (Mathf.Abs(targetX - startX) / screenWidth) / fitSpeed;
        Vector2 currentPosition = rec.anchoredPosition;

        fitCoroutine = StartCoroutine(KKUtilities.FloatLerp(duration, (t) =>
        {
            currentPosition.x = Mathf.Lerp(startX, targetX, t * t);
            rec.anchoredPosition = currentPosition;
        }).OnCompleted(() =>
        {
            fitCoroutine = null;
            pageControl.SetIndex(currentSelectIndex);
        }));
    }

    //今どのイメージを選択しているかを判断する
    int GetCurrentSelectImageIndex(bool isBack, float borderRate = 0.3f)
    {
        //１ページ目の位置からどれだけ離れているか
        float distance = Mathf.Abs(rec.anchoredPosition.x);

        if (isBack) borderRate *= -1.0f;

        for (int i = 0; i < imageNum; i++)
        {
            float borderX = (screenWidth * i) + screenWidth * borderRate;

            if (distance < (borderX))
            {
                if (!isBack) return i;
                else return i - 1;
            }
        }

        //本来ここに来ることはない
        if (!isBack) return imageNum - 1;
        else return imageNum - 2;
    }
}
