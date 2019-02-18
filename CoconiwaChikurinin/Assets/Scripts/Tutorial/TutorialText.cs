using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialText : MonoBehaviour
{
    //serializeだと改行できないため、そのままだと単語の途中で改行されてしまうため
    string[] Japanese = new string[10]
    {
       "より深く楽しめる\n観光アプリ",
        "ココニワは高校生と大学生が\n開発した観光アプリです\n竹林院をより深く\n楽しむことができます",
        "木札マーカーを見つけよう",
        "園内にはいくつかの木札マーカーが\n設置されているので\n見つけてみよう！",
        "AR機能を使って\n写真を撮ろう",
        "木札マーカーを見つけたら\nカメラアイコンをタップして\n写真を撮ろう！",
        "コンテンツが\n楽しめる",
        "撮影したコンテンツの\n写真や説明が表示され\nより深く竹林院のことを\n知ることができます",
        "コンプリートしよう",
        "コンテンツはマップ上に\n反映され確認できます\nコンテンツを全て集めて\nコンプリートを目指そう！",
    };

    string[] English = new string[10]
    {
        "Application for better sightseeing",
        "Coconiwa is an application for sightseeing that high school and universuty students developed.You can enjoy the scenic spot \"Chikurin-in\" more deeply.",
        "Please find the wooden tags.",
        "Some wooden tags are set up in the garden. Please find them!!",
        "Please take a picture with AR function.",
        "Find the wooden tag and tap a camera icon to take a picture.",
        "You can enjoy the contents.",
        "The pictures and explanations are shown and you can understand the Chikurin-in more deeply.",
        "Let's complete.",
        "The contents are reflected on a map and you can confirm them.Try to collect all contents and complete it ! ",
     };

    string[] Korea = new string[10]
    {
        "더 깊이 즐길 수 있는 관광어플",
        "고코니와는 고등학생과 대학생이 개발한 관광어플입니다 명승지 이스이원을 더 깊이 즐길 수 있습니다 ",
        "나무판자를 찾아내자 ",
        "원내에는 나무 판자가 몇 개 설치되어 있으니 찾아보자!",
         "AR기능으로  사진을 찍자",
         "나무판자를 찾아내서 카메라아이콘을 클릭하고 사진을 찍어보자! ",
         "컨텐츠를 즐겨보자",
         "촬영한 컨텐츠의 사진이나 설명이 표시되어, 이스이원을 더 깊이 알 수 있습니다",
         "완성하자",
         "내용은 지도에 반영되며 확인할 수 있습니다 내용을 모두 모아서 완성해보자! ",
    };


    string[] ChinaSimplification = new string[10]
    {
        "有趣的深度观光软件",
        "coconiwa的高中生与大学生共同开发的观光软件 能更深入有趣地观赏名胜依水园",
        "寻找木牌标识",
        "园内设置有若干的木牌标识，一起寻找吧",
        "请使用AR功能拍照",
        "找到木牌标识后，点击相机图标进行拍照",
        "享阅内容",
        "拍摄后手机会显示相对的图片和说明，通过阅读这些内容，你能更深入地了解依水园",
        "完整收集",
        "收集到的内容会在地图上显现，可以进行确认 让我们集齐所有的说明内容吧",
    };
    string[] ChinaTraditional = new string[10]
    {
        "有趣的深度觀光軟體",
        "coconiwa的高中生與大學生共同開發的觀光軟體 能更深入有趣地觀賞名勝依水園",
        "尋找木牌標識",
        "園內設置有若干的木牌標識,一起尋找吧",
        "請使用AR功能拍照",
        "找到木牌標識後,點擊相機圖標進行拍照",
        "享閱內容",
        "拍攝後手機會顯示相對的圖片和說明,通過閱讀這些內容,你能更深入地了解依水園",
        "完整收集",
        "收集到的內容會在地圖上顯現，可以進行確認 讓我們集齊所有的說明內容吧",
    };
    [SerializeField]
    List<Text> texts = null;

    // Use this for initialization
    void Start()
    {
        switch (AppData.UsedLanguage)
        {
            case SystemLanguage.Japanese:
                SetText(Japanese);
                break;
            case SystemLanguage.Korean:
                SetText(Korea);
                break;
            case SystemLanguage.Chinese:
                SetText(ChinaSimplification);
                break;
            case SystemLanguage.ChineseSimplified:
                SetText(ChinaSimplification);
                break;
            case SystemLanguage.ChineseTraditional:
                SetText(ChinaTraditional);
                break;
            default:
                SetText(English);
                break;
        }
    }


    private void SetText(string[] setTexts)
    {

        for (int i = 0; i < setTexts.Length; i++)
        {
            texts[i].text = setTexts[i];
        }

    }
}
