using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditController : MonoBehaviour
{
    [SerializeField]
    RectTransform textBox = null;

    [SerializeField]
    Text titleText = null;

    [SerializeField]
    RectTransform japaneseTextBox = null;

    [SerializeField]
    RectTransform englishTextBox = null;

    [SerializeField]
    GameObject HeaderObject = null;

    [SerializeField]
    GameObject content = null;

    [SerializeField]
    GameObject Panel = null;

    [SerializeField]
    GameObject CrediyHeader = null;

    public void PushCredit()
    {
        CrediyHeader.SetActive(true);
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
                titleText.text = "COCONIWA PROJECT";
            }

            textBox.sizeDelta = new Vector2(textBox.sizeDelta.x, rectTransform.sizeDelta.y);
        }, this);
    }
    public void ClickBackButton()
    {
        CrediyHeader.SetActive(false);
        Panel.SetActive(false);
        content.SetActive(true);
        HeaderObject.SetActive(true);
        titleText.gameObject.SetActive(false);
        englishTextBox.gameObject.SetActive(false);
        japaneseTextBox.gameObject.SetActive(false);
    }

}
