using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasMode : MonoBehaviour
{

    private static bool _isIphoneX = true;

    [RuntimeInitializeOnLoadMethod]
    static void Init()
    {
        float aspect = Camera.main.aspect;
 
        _isIphoneX = (aspect < 9.0f / 16.0f);    
    }

    // Use this for initialization
    void Start()
    {
        //状況を見て縦と横のどちらを優先するか決定する
        if (_isIphoneX)
        {
            GetComponent<CanvasScaler>().matchWidthOrHeight = 0;
        }
        else
        {
            GetComponent<CanvasScaler>().matchWidthOrHeight = 1;
        }
    }
}
