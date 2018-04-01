using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditController : MonoBehaviour
{
    [SerializeField]
    RectTransform textBox = null;

    [SerializeField]
    Text titleText=null;

    [SerializeField]
    RectTransform japaneseTextBox = null;

    [SerializeField]
    RectTransform englishTextBox = null;

    [SerializeField]
    RectTransform copyRightTextBox = null;

    [SerializeField]
    GameObject HeaderObject = null;

    [SerializeField]
    GameObject content = null;

    [SerializeField]
    GameObject Panel = null;

    public void PushCredit()
    {
        Panel.SetActive(true);
        content.SetActive(false);
        HeaderObject.SetActive(false);
        titleText.gameObject.SetActive(true);
        KKUtilities.WaitSeconde(0.01f, () =>
        {
            RectTransform rectTransform;
            if (AppData.UsedLanguage == SystemLanguage.Japanese)
            {
                japaneseTextBox.gameObject.SetActive(true);
                rectTransform = japaneseTextBox;
            }
            else
            {
                englishTextBox.gameObject.SetActive(true);
                rectTransform = englishTextBox;
                titleText.text= "EnglishTextTitle";
            }
            copyRightTextBox.anchoredPosition = rectTransform.anchoredPosition + (rectTransform.sizeDelta.y) * Vector2.down;

            textBox.sizeDelta = new Vector2(textBox.sizeDelta.x, rectTransform.sizeDelta.y + copyRightTextBox.sizeDelta.y + 40.0f);
        }, this);
    }

}
