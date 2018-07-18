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
    ContentsViewController contentsViewController = null;

    [SerializeField]
    Text ContentName = null;

    [SerializeField]
    HyphenationJpn ContentText = null;
    [SerializeField]
    Image ContentsBack = null;

    [SerializeField]
    Image Header = null;

    [SerializeField]
    Sprite History = null;
    [SerializeField]
    Sprite Nature = null;
    [SerializeField]
    Sprite Yoshino = null;

    [SerializeField]
    Image backButtonImage = null; 
    [SerializeField]
    Sprite HistoryButton = null;
    [SerializeField]
    Sprite NatureButton = null;
    [SerializeField]
    Sprite YoshinoButton = null;


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
        string up = "_UP";
        Vector2 imagePosition = new Vector2(0.0f, 373.0f);
        int fileNum = 0;
        Sprite sprite;

        while (true)
        {
            if (fileNum > 0)
            {
                fileName = contentsData.Elements[index].FileID + c;
                c++;
            }


            sprite = Resources.Load<Sprite>(fileName);
            if (sprite == null) break;

            Image image = Instantiate(imagePrefab, imageContainer.transform);
            imagePosition.x = fileNum * 1080.0f;
            image.rectTransform.anchoredPosition = imagePosition;
            image.sprite = sprite;

            fileNum++;
        }

        maxImageNum = fileNum;

        ContentsSwipeController.I.SetImageNum(maxImageNum);
        if (maxImageNum > 1) pageControl.Initialize(maxImageNum);

        ContentName.text = contentsData.Elements[index].ContentsName;
        ContentText.GetText(contentsData.Elements[index].ContentsText);

        contentsTextController.SetTextInterval();
        char h = contentsData.Elements[index].FileID[0];

        Color color = default(Color);
        //タイプによって画像、色の変更
        if (h == 'H')
        {
            backButtonImage.sprite = HistoryButton;
            ColorUtility.TryParseHtmlString("#826787",out color);
            Header.sprite = History;
        }
        else if (h == 'N')
        {
            backButtonImage.sprite = NatureButton;
            Header.sprite =Nature;
            ColorUtility.TryParseHtmlString("#268050", out color);
        }
        else if (h == 'Y'|| h == 'I')
        {
            backButtonImage.sprite = YoshinoButton;
            Header.sprite = Yoshino;
            ColorUtility.TryParseHtmlString("#e79ea3", out color);
        }
        ContentsBack.color = color;


        //拡大があったら
        fileName = contentsData.Elements[index].FileID + up;
        sprite = Resources.Load<Sprite>(fileName);
        if (sprite == null)
        {
            contentsViewController.gameObject.SetActive(false);
            return;
        }
        contentsViewController.SetInit(sprite);
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
