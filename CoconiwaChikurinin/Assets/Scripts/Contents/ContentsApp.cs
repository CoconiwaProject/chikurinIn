using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContentsApp : MonoBehaviour
{

    //アップになる画像
    Image contentsImage = null;
    //消える文字のセット
    [SerializeField]
    GameObject ContentsTestSet = null;

    [SerializeField]
    ContentManager contentManager = null;



    // Use this for initialization
    void Start()
    {
        contentsImage = contentManager.imageContainer.GetChild(0).GetComponent<Image>();


    }

    // Update is called once per frame
    void Update()
    {

    }
}
