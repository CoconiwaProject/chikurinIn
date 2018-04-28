using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContentGroup : MonoBehaviour
{
    [SerializeField]
    ContentListItem[] itemPrefabs = null;

    [SerializeField]
    Sprite itemBGImage = null;

    public List<ContentsData.Params> contentParams = new List<ContentsData.Params>();

    const int oneLineTextLength = 24;//24以下だと１列
    const int towLineTextLength = 50;//50以下だと２列

    public ContentListItem mostUnderItem;

    public void Create(ContentListItem UnderItem = null)
    {
       // 一つ上のグループの最下のアイテムが指定されていたらそれに合わせてグループの位置を移動
        //if (UnderItem != null)
        //{
        //    RectTransform rect = GetComponent<RectTransform>();

        //    Debug.Log(UnderItem.gameObject.GetComponent<RectTransform>().anchoredPosition);

        //    switch (AppData.UsedLanguage)
        //    {
        //        case SystemLanguage.English:
        //            rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, UnderItem.GetComponent<RectTransform>().anchoredPosition.y - 900);
        //            break;
        //        case SystemLanguage.Japanese:
        //            rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, UnderItem.GetComponent<RectTransform>().anchoredPosition.y-200);
        //            break;
        //        case SystemLanguage.Korean:
        //        case SystemLanguage.Chinese:
        //        default:
        //            rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, UnderItem.GetComponent<RectTransform>().anchoredPosition.y - 700);
        //            break;
        //    }

        //}
        //配置するX座標
        const float leftX = 110.0f;
        const float rightX = 620.0f;

        bool isLeft = true;

        //Y座標の間隔
        float yDistance = 90.0f;
        const float yDistanceRate = 20.0f;
        float currentY = 80.0f;
        bool isChangeDistance = false;


        //ストック
        List<ContentsData.Params> oneLineContentList = new List<ContentsData.Params>();
        List<ContentsData.Params> towLineContentList = new List<ContentsData.Params>();
        List<ContentsData.Params> threeLineContentList = new List<ContentsData.Params>();

        //計算用
        Vector3 itemPosition = Vector3.zero;
        ContentListItem item = null;

        //仕分け
        for (int i = 0; i < contentParams.Count; i++)
        {
            int lineNum = GetTextLineNum(contentParams[i].ContentsName);

            // Debug.Log(contentParams[i].ContentsName + " lineNum = " + lineNum);
            switch (lineNum)
            {
                case 1:
                    oneLineContentList.Add(contentParams[i]);
                    break;
                case 2:
                    towLineContentList.Add(contentParams[i]);
                    break;
                case 3:
                    threeLineContentList.Add(contentParams[i]);
                    break;
            }
        }

        //小さいのから配置していく
        for (int i = 0; i < oneLineContentList.Count; i++)
        {
            item = Instantiate(itemPrefabs[0], transform);

            //座標の計算
            itemPosition.x = isLeft ? leftX : rightX;
            isLeft = !isLeft;
            currentY -= ((i + 1) % 2) * yDistance;
            itemPosition.y = currentY;
            item.transform.localPosition = itemPosition;

            item.BGImage.sprite = itemBGImage;
            item.ContentSet(oneLineContentList[i]);
        }

        yDistance += yDistanceRate;
        isLeft = true;

        //以下規則性のあるコードが並ぶが、うまいこと思い浮かばなかったので許して
        for (int i = 0; i < towLineContentList.Count; i++)
        {
            if (i == 2)
            {
                yDistance += yDistanceRate;
                isChangeDistance = true;
            }

            item = Instantiate(itemPrefabs[1], transform);

            //座標の計算
            itemPosition.x = isLeft ? leftX : rightX;
            isLeft = !isLeft;
            currentY -= ((i + 1) % 2) * yDistance;
            itemPosition.y = currentY;
            item.transform.localPosition = itemPosition;

            item.BGImage.sprite = itemBGImage;
            item.ContentSet(towLineContentList[i]);
        }

        if (!isChangeDistance) yDistance += yDistanceRate;
        yDistance += yDistanceRate;
        isLeft = true;

        for (int i = 0; i < threeLineContentList.Count; i++)
        {
            if (i == 2)
            {
                yDistance += yDistanceRate;
            }

            item = Instantiate(itemPrefabs[2], transform);

            //座標の計算
            itemPosition.x = isLeft ? leftX : rightX;
            isLeft = !isLeft;
            currentY -= ((i + 1) % 2) * yDistance;
            itemPosition.y = currentY;
            item.transform.localPosition = itemPosition;

            item.BGImage.sprite = itemBGImage;
            item.ContentSet(threeLineContentList[i]);
        }

        mostUnderItem = item;
    }

    /// <summary>
    /// 渡されたテキストを何行で表示すればよいかを返す
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    int GetTextLineNum(string text)
    {
        int length = text.Length;
        if (towLineTextLength < length) return 3;
        else if (oneLineTextLength < length) return 2;
        else return 1;
    }
}
