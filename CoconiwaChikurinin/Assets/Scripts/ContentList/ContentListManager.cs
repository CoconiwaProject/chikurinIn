using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContentListManager : MonoBehaviour
{
    ContentsData contentsData = null;

    [SerializeField]
    ContentGroup ContentGroupN = null;

    [SerializeField]
    ContentGroup ContentGroupH = null;

    [SerializeField]
    ContentGroup ContentGroupY = null;

    [SerializeField]
    RectTransform contentRec = null;


    // Use this for initialization
    void Start()
    {
        contentsData = AppData.ContentsData;
       
        for (int i = 0; i < contentsData.Elements.Count; i++)
        {
            Sprite sprite = Resources.Load<Sprite>(contentsData.Elements[i].FileID);
            string name = contentsData.Elements[i].ContentsName;

            char h = contentsData.Elements[i].FileID[0];

            //タイプによって画像、色の変更
            if (h == 'N')
            {
                ContentGroupN.contentParams.Add(contentsData.Elements[i]);

            }
            else if (h == 'H')
            {
                ContentGroupH.contentParams.Add(contentsData.Elements[i]);

            }
            else if (h == 'Y')
            {
                ContentGroupY.contentParams.Add(contentsData.Elements[i]);
            }
        }

        ContentGroupN.Create();
        ContentGroupH.Create();
        ContentGroupY.Create(ContentGroupN.mostUnderItem);

        RectTransform rec = ContentGroupY.mostUnderItem.transform as RectTransform;
        RectTransform parentRec = rec.parent as RectTransform;
        float limit = rec.anchoredPosition.y + parentRec.anchoredPosition.y - 100.0f;
        Vector2 contentRecSize = contentRec.sizeDelta;
        contentRecSize.y = Mathf.Abs(limit)*2;
        contentRec.sizeDelta = contentRecSize;
    }


    int GetIndex(string name)
    {
        for (int i = 0; i < contentsData.Elements.Count; i++)
        {
            if (contentsData.Elements[i].FileID == name)
            {
                return i;
            }
        }
        Debug.LogError("NotFoundData");
        return -1;
    }
}
