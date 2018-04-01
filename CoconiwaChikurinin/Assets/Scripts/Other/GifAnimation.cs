using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GifAnimation : MonoBehaviour {

    [SerializeField]
    private List<Sprite> animationSpriteList = new List<Sprite>();

    public Image changeImage;

    private int nowSprite = 0;

    private int speed = 15;
	// Update is called once per frame
	void Update () {
        nowSprite++;
        if(nowSprite>animationSpriteList.Count*speed)
        {
            nowSprite = 0;
        }
        if(nowSprite% speed==0)
        changeImage.sprite=animationSpriteList[nowSprite/ speed];

    }
}
