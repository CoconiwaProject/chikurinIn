using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//丸ポチのやつ
public class ContentsPageControl : MonoBehaviour
{
    [SerializeField]
    Image pageControlPrefab = null;

    [SerializeField]
    Sprite enableSprite = null;
    [SerializeField]
    Sprite disebleSprite = null;
    
    Image[] pageControlImages;

    int oldSelectIndex = 0;

    public void Initialize(int pageNum)
    {
        pageControlImages = new Image[pageNum];

        for(int i = 0;i< pageNum;i++)
        {
            pageControlImages[i] = Instantiate(pageControlPrefab, transform) as Image;
        }

        pageControlImages[0].sprite = enableSprite;
    }

    public void SetIndex(int index)
    {
        if (oldSelectIndex == index) return;

        pageControlImages[oldSelectIndex].sprite = disebleSprite;
        pageControlImages[index].sprite = enableSprite;

        oldSelectIndex = index;
    }
}
