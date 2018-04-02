using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [SerializeField]
    Transform tutorialImagesTransform=null;

    [SerializeField]
    Transform textsTransform = null;


    [SerializeField]
     Button nextButtone = null;
    [SerializeField]
    Text buttoneText = null;

    [SerializeField]
    Sprite[] circleSprites = null;


    [SerializeField]
   Image[] circleImages = null;

    private int nowSelectCount = 0;
    private const int EndSelectCount = 5;
    private const float interval = 5.63f;
    private const float PushButtonSpeed = -1.0f;
    private const float LowestSpeed = -0.2f;

    private float moveSpeed = 0;
    private bool nowMove;
    private bool isFlick;

    private string sceneName = "Home";

    private Vector3 oldPosition;
    private Vector3 imagePosition;
    private Vector3 mousePos;
    private Vector2 flickStartPosition;

    public string[] JapaneseButtoneText;
    public string[] EnglishButtoneText;
    public string[] ChinaSimplificationButtoneText;
    public string[] ChinaTraditionalButtoneText;
    public string[] KoreaButtoneText;

    public string[] useButtoneText;


    void Start()
    {
        PlayerPrefs.SetString("Init", "");
        // UnityEngine.SceneManagement.SceneManager.LoadScene(SceneName);

            SetStartButtone();

        isFlick = nowMove = false;
        moveSpeed = nowSelectCount = 0;
    }
    // Update is called once per frame
    void Update()
    {
        ImageControl();
        if (nowMove)
        {
            Vector3 positio = tutorialImagesTransform.localPosition;
            float judge = positio.x;
            float judge2 = -interval * nowSelectCount;
            if (moveSpeed > 0)
            {
                float copy = judge;
                judge = judge2;
                judge2 = copy;
            }

            if (judge > judge2)
            {
                if (moveSpeed > 0)
                {
                    positio.x += moveSpeed;
                    judge2 = positio.x;
                }
                else
                {
                    positio.x += moveSpeed;
                    judge = positio.x;

                }

                if (judge > judge2)
                    textsTransform.transform.position = tutorialImagesTransform.localPosition = positio;
                else
                {
                    nowMove = false;
                    textsTransform.transform.position = tutorialImagesTransform.localPosition = new Vector3(-interval * nowSelectCount, positio.y, positio.z);
                }
            }
            else
            {
                nowMove = false;
                textsTransform.transform.position = tutorialImagesTransform.localPosition = new Vector3(-interval * nowSelectCount, positio.y, positio.z);
            }
        }
    }

    private void ImageControl()
    {

        if (Input.GetMouseButtonDown(0))
        {
            imagePosition = tutorialImagesTransform.position;
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 texPos = textsTransform.position;
            Vector3 prePos = tutorialImagesTransform.position;

            Vector3 diff = Camera.main.ScreenToWorldPoint(Input.mousePosition) - mousePos;

            if (Input.touchSupported)
            {
                diff = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position) - mousePos;
            }

            diff.z = diff.y = 0.0f;

            //範囲外修正
            if (imagePosition.x + diff.x > 0)
            {
                textsTransform.position = tutorialImagesTransform.position = Vector3.zero;
            }
            else if (imagePosition.x + diff.x < -interval * (EndSelectCount - 1))
            {
                Vector3 resetPosition = new Vector3(-interval * (EndSelectCount - 1), 0, 0);
                textsTransform.position = tutorialImagesTransform.position = resetPosition;
            }
            else//通常移動
            {
                textsTransform.position = tutorialImagesTransform.position = imagePosition + diff;
            }
        }
        if (Input.GetMouseButtonUp(0))
            imagePosition = Vector3.zero;
    }

    void OnEnable()
    {
        TouchManager.Instance.Drag += OnSwipe;
        TouchManager.Instance.TouchEnd += OnTouchEnd;
        TouchManager.Instance.FlickStart += OnFlickStart;
        TouchManager.Instance.FlickComplete += OnFlickComplete;
    }

    void OnDisable()
    {
        if (TouchManager.Instance != null)
        {
            TouchManager.Instance.Drag -= OnSwipe;
            TouchManager.Instance.TouchEnd -= OnTouchEnd;
            TouchManager.Instance.FlickStart -= OnFlickStart;
            TouchManager.Instance.FlickComplete -= OnFlickComplete;
        }
    }

    void OnTouchEnd(object sender, CustomInputEventArgs e)
    {
        if (nowMove == true) return;
        Vector3 acceleration = Camera.main.ScreenToWorldPoint(Input.mousePosition) - Camera.main.ScreenToWorldPoint(oldPosition);
        nowMove = true;

        float imagesPositionX = tutorialImagesTransform.position.x;
        int nearCount = (int)((-imagesPositionX) / interval);
        if ((-imagesPositionX) - nearCount * interval > interval / 2)//奥に近い
        {
            if (acceleration.x < 0)
                moveSpeed = acceleration.x;
            else
                moveSpeed = -acceleration.x;
            nearCount++;
        }
        else
        {
            if (acceleration.x < 0)
                moveSpeed = -acceleration.x;
            else
                moveSpeed = acceleration.x;
        }

        if (nearCount >= 0 && nearCount < EndSelectCount)
        {
            ChangeSprite(nowSelectCount, nearCount);
            nowSelectCount = nearCount;
        }
        if (0 == moveSpeed)
        {
            if ((-imagesPositionX) - nearCount * interval < 0)
                moveSpeed = LowestSpeed;
            else
                moveSpeed = -LowestSpeed;
        }
        else if (moveSpeed > 0 && moveSpeed < -LowestSpeed)
            moveSpeed = -LowestSpeed;
        else if (moveSpeed < 0 && moveSpeed > LowestSpeed)
            moveSpeed = LowestSpeed;
    }

    void OnSwipe(object sender, CustomInputEventArgs e)
    {
        oldPosition = Input.mousePosition;
    }

    void OnFlickStart(object sender, FlickEventArgs e)
    {
        flickStartPosition = Input.mousePosition;
    }

    void OnFlickComplete(object sender, FlickEventArgs e)
    {
        if (flickStartPosition.x < Input.mousePosition.x)
        {
            if (nowSelectCount == 0) return;
            if (moveSpeed <= 0)
            {
                ChangeSprite(nowSelectCount, nowSelectCount - 1);
                nowSelectCount--;
            }
            moveSpeed = -PushButtonSpeed;
        }
        else
        {
            if (nowSelectCount == EndSelectCount - 1) return;
            if (moveSpeed >= 0)
            {
                ChangeSprite(nowSelectCount, nowSelectCount + 1);
                nowSelectCount++;
            }
            moveSpeed = PushButtonSpeed;
        }
        nowMove = true;
    }

    public void OnNextButtone()
    {
        if (nowSelectCount == EndSelectCount - 1)
        {
            UnderBerMenu.I.ChangeHomeScene(sceneName);
        }
        else
        {
            //制限つける
            ChangeSprite(nowSelectCount, nowSelectCount + 1);
            nowSelectCount++;
            nowMove = true;
            moveSpeed = PushButtonSpeed;
        }
    }

    public void MovePosition(Vector3 offset)
    {
        var p = tutorialImagesTransform.position;
        tutorialImagesTransform.position = new Vector3(p.x + offset.x, p.y, p.z);
    }

    private void ChangeSprite(int oldSelect,int nowSelect)
    {
        int Select = 0,notSelect=1;

        if(oldSelect<nowSelect)
        {
            //まとめてスワイプしてしまった場合
            for(int i=oldSelect+1;i<=nowSelect;i++)
            circleImages[i].sprite = circleSprites[Select];
        }
        else if(oldSelect>nowSelect)
        {
            for (int i = oldSelect ; i > nowSelect; i--)
                circleImages[i].sprite = circleSprites[notSelect];
        }

        if (nowSelect == EndSelectCount - 1)
        {
                buttoneText.text = useButtoneText[1];
        }
        else if (oldSelect == EndSelectCount - 1)
        {
                buttoneText.text = useButtoneText[0];
        }
    }

    private void SetStartButtone()
    {
        useButtoneText = new string[EnglishButtoneText.Length];
        if(AppData.UsedLanguage==SystemLanguage.Chinese|| AppData.UsedLanguage == SystemLanguage.ChineseSimplified)
        {
            for (int i = 0; i < useButtoneText.Length; i++)
                useButtoneText[i] = ChinaSimplificationButtoneText[i];
        }
        else if (AppData.UsedLanguage == SystemLanguage.Japanese)
        {
            for (int i = 0; i < useButtoneText.Length; i++)
                useButtoneText[i] = JapaneseButtoneText[i];
        }
        else if (AppData.UsedLanguage == SystemLanguage.ChineseTraditional)
        {
            for (int i = 0; i < useButtoneText.Length; i++)
                useButtoneText[i] = ChinaTraditionalButtoneText[i];
        }
        else if (AppData.UsedLanguage == SystemLanguage.Korean)
        {
            for (int i = 0; i < useButtoneText.Length; i++)
                useButtoneText[i] = KoreaButtoneText[i];
        }
        else 
        {
            for (int i = 0; i < useButtoneText.Length; i++)
                useButtoneText[i] = EnglishButtoneText[i];
        }

        buttoneText.text = useButtoneText[0];

    }
}
