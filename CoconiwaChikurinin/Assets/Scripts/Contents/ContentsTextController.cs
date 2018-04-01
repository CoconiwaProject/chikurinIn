using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ContentsTextController : MonoBehaviour
{
    [SerializeField]
    RectTransform[] setTextTransforms = null;

    [SerializeField]
    RectTransform ContentsBox = null;

    [SerializeField]
    Text contentsText = null;

    Vector2 imageInterval = new Vector2(0, 0);
    Vector2 titleInterval = new Vector2(0, 0);

    // Use this for initialization
    void Start()
    {
        //SizeFilterの変更に１フレームかかるのでディレイをかける
        KKUtilities.WaitSeconde(0.01f, () =>
        {
            //タイトルが2行になったら1行分ずらす
            Vector2 tmp = Vector2.down * (setTextTransforms[0].sizeDelta.y - 86.0f);
            setTextTransforms[1].anchoredPosition += tmp;
            setTextTransforms[2].anchoredPosition += tmp;

            float boxSize = 0.0f;
            foreach (RectTransform c in setTextTransforms)
            {
                boxSize += c.sizeDelta.y;
            }
            ContentsBox.sizeDelta = new Vector2(ContentsBox.sizeDelta.x, boxSize);
        }, this);
    }

    public void SetTextInterval()
    {
        contentsText.text = "\n\n" + contentsText.text;
    }
}
