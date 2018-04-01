using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContentManager : MonoBehaviour
{
    ContentsData contentsData;

    public Transform imageContainer = null;
    [SerializeField]
    ContentsPageControl pageControl = null;

    int maxImageNum;
    [SerializeField]
    Image imagePrefab = null;

    [SerializeField]
    Text ContentName = null;

    [SerializeField]
    HyphenationJpn ContentText = null;
    [SerializeField]
    Image ContentsBack = null;

    [SerializeField]
    Image Header = null;

    [SerializeField]
    Sprite Inter = null;
    [SerializeField]
    Sprite Artfact = null;
    [SerializeField]
    Sprite Prants = null;

    [SerializeField]
    ContentsTextController contentsTextController = null;

    private int index;

    // Use this for initialization
    void Awake()
    {
        contentsData = AppData.ContentsData;
        index = GetIndex(AppData.SelectTargetName);
        
        if (index == -1) return;

        string fileName = contentsData.Elements[index].FileID;
        char c = 'b';
        Vector2 imagePosition = new Vector2(0.0f, 373.0f);
        int fileNum = 0;

        while(true)
        {
            if(fileNum > 0)
            {
                fileName = contentsData.Elements[index].FileID + c;
                c++;
            }

            Sprite sprite = Resources.Load<Sprite>(fileName);
            if (sprite == null) break;

            Image image = Instantiate(imagePrefab, imageContainer.transform);
            imagePosition.x = fileNum * 1080.0f;
            image.rectTransform.anchoredPosition = imagePosition;
            image.sprite = sprite;

            fileNum++;
        }

        maxImageNum = fileNum;

        ContentsSwipeController.I.SetImageNum(maxImageNum);
        if(maxImageNum > 1) pageControl.Initialize(maxImageNum);
        
        ContentName.text = contentsData.Elements[index].ContentsName;
        ContentText.GetText(contentsData.Elements[index].ContentsText);

        contentsTextController.SetTextInterval();
        char h = contentsData.Elements[index].FileID[0];

        //タイプによって画像、色の変更
        if (h == 'A')
        {
            Header.sprite = Artfact;
            ContentsBack.color = new Color(0.65f, 0.25f, 0.0f);
        }
        else if (h == 'P')
        {
            Header.sprite = Prants;
        }
        else if (h == 'I')
        {
            Header.sprite = Inter;
            ContentsBack.color = new Color(0.19f, 0.3f, 0.54f);
        }
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
        //UnityEngine.SceneManagement.SceneManager.LoadScene("Title");
        return -1;
    }
}
