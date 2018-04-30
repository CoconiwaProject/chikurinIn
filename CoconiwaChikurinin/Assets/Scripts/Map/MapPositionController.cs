using UnityEngine;

public class MapPositionController : MonoBehaviour
{
    [SerializeField]
    RectTransform imageRect = null;
    Vector2 imageSize;
    Vector3 imagePivot = new Vector3(0.35f, 0.35f);

    //キャッシュ
    Vector3 zeroVec = Vector3.zero;
    Vector3 swipeVec = Vector3.zero;
    float swipeSpeed = 0.2f;

    //慣性
    Vector3 inerVec = Vector3.zero;
    //どの程度慣性の影響を受けるか
    float inerPow = 0.7f;
    float minimumIner = 3.0f;

    //摩擦
    float rubPow = 5.0f;

    MapManager mapManager;

    float screenSizeRate;

    void Start()
    {
        imageSize = imageRect.sizeDelta;

        screenSizeRate = 1.0f + (1.0f - ((float)Screen.width / 1080));

        TouchManager.Instance.Drag += OnImageSwipe;

        mapManager = MapManager.I;
    }

#if UNITY_EDITOR
#else
    void OnDestroy()
    {
        TouchManager.Instance.Drag -= OnImageSwipe;
    }
#endif

    void Update()
    {
        float tempSpeed = swipeSpeed + ((imageRect.localScale.x / MapScaleController.scaleRateMax) * (0.1f * screenSizeRate));
        Vector3 movement = new Vector3(swipeVec.x, swipeVec.y, 0) * tempSpeed;

        //慣性
        inerVec += movement * inerPow;
        //摩擦
        inerVec -= inerVec.normalized * rubPow;

        movement += inerVec;

        //ピポットの移動で画像を移動させる
        imagePivot.x -= movement.x / (imageSize.x * imageRect.localScale.x);
        imagePivot.y -= movement.y / (imageSize.y * imageRect.localScale.y);

        //値の制限
        imagePivot.x = Mathf.Clamp(imagePivot.x, 0.0f, 1.0f);
        imagePivot.y = Mathf.Clamp(imagePivot.y, 0.0f, 1.0f);

        //ある程度小さくなったらゼロとみなす
        if(inerVec.x < minimumIner && inerVec.x > -minimumIner &&
            inerVec.y < minimumIner && inerVec.y > -minimumIner)
        {
            inerVec = zeroVec;
        }

        imageRect.pivot = imagePivot;
        imageRect.localPosition = zeroVec;

        //毎回ゼロを入れる
        swipeVec = zeroVec;
    }

    void OnImageSwipe(object sender, CustomInputEventArgs e)
    {
        if (Input.touchCount >= 2) return;

        swipeVec = e.Input.DeltaPosition;
    }
}
