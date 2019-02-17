using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditController : MonoBehaviour
{
    [SerializeField]
    RectTransform textBox = null;


    [SerializeField]
    Image Header = null;
    [SerializeField]
    Sprite EnglishHeaderSprite = null;


    [SerializeField]
    RectTransform japaneseTextBox = null;
    [SerializeField]
    RectTransform englishTextBox = null;


    [SerializeField]
    GameObject content = null;

    [SerializeField]
    GameObject credit = null;


    private void Start()
    {
        //スクロールするやつも
        KKUtilities.WaitSeconde(0.01f, () =>
        {
            RectTransform rectTransform;
            //一旦英語の画像がないので強制的に日本語クレジット表示
            //if (AppData.UsedLanguage == SystemLanguage.Japanese)
            //{
                japaneseTextBox.gameObject.SetActive(true);
                rectTransform = japaneseTextBox;
            //}
            //else
            //{
            //    englishTextBox.gameObject.SetActive(true);
            //    rectTransform = englishTextBox;
            //    Header.sprite = EnglishHeaderSprite;
            //}

            textBox.sizeDelta = new Vector2(textBox.sizeDelta.x, rectTransform.sizeDelta.y);
        }, this);
    }


    public void PushCredit()
    {
        credit.SetActive(true);
        content.SetActive(false);
    }
    public void ClickBackButton()
    {
        credit.SetActive(false);
        content.SetActive(true);
    }

}
