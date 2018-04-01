using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThetaViewSceneManager : SingletonMonoBehaviour<ThetaViewSceneManager>
{
    [SerializeField]
    ThetaPictureChanger pictureChanger = null;

    protected override void Start()
    {
        base.Start();

        pictureChanger.SetPicture(AppData.SelectThetaPictures[0]);
        pictureChanger.gameObject.SetActive(AppData.CanChangePicture);
    }
}
