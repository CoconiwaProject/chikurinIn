using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThetaText : MonoBehaviour
{
    private const int buttoneCount = 3;

    //言語によって余計なものが混じっているので直接
    string[] JapaneseTexts = new string[buttoneCount] { "前庭", "主庭", "柳生堂" };
    string[] ChineseTraditionalTexts = new string[buttoneCount] { "前園", "後園", "柳生堂" };
    string[] ChineseSimplifiedTexts = new string[buttoneCount] { "前园", "后园", "柳生堂" };
    string[] EnglishTexts = new string[buttoneCount] { "The Front Yard", "The Main Garden", "The Yagyu-do Hall" };
    string[] KoreanTexts = new string[buttoneCount] { "전원", "후원", "야규당" };

    string addText = "360°";

    [SerializeField]
    List<Text> TextList = new List<Text>();

    [SerializeField]
    List<Text> EnglishTextList = new List<Text>();

    [SerializeField]
    List<ThetaButton> ThetaButtons = new List<ThetaButton>();

    [SerializeField]
    List<Button> EnglishButtone = new List<Button>();

    // Use this for initialization
    void Start()
    {
        
        if (AppData.UsedLanguage == SystemLanguage.Japanese)
        {
            AddLanguage(JapaneseTexts, TextList);
        }
        else if (AppData.UsedLanguage == SystemLanguage.Korean)
        {
            AddLanguage(KoreanTexts, TextList);
        }
        else if (AppData.UsedLanguage == SystemLanguage.Chinese || AppData.UsedLanguage == SystemLanguage.ChineseTraditional)
        {
            AddLanguage(ChineseTraditionalTexts, TextList);
        }
        else if (AppData.UsedLanguage == SystemLanguage.ChineseSimplified)
        {
            AddLanguage(ChineseSimplifiedTexts, TextList);
        }
        else
        {
            AddLanguage(EnglishTexts, EnglishTextList);
            for (int i = 0; i < ThetaButtons.Count; i++)
            {
                ThetaButtons[i].Popup = EnglishButtone[i];
            }
        }
    }
    private void AddLanguage(string[] texts, List<Text> addTextList)
    {
        for (int i = 0; i < texts.Length; i++)
            addTextList[i].text = texts[i] + addText;
    }
}
