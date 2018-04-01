using UnityEngine;

public class MapScaleController : MonoBehaviour
{
    [SerializeField]
    RectTransform imageRect = null;

    //カメラ視覚の範囲
    public const float scaleRateMin = 1.0f;
    public const float scaleRateMax = 5.0f;

    //現在の値
    float currentScaleRate = 1.5f;
    float scalingSpeed = 1.0f;

    //最初にタッチした時の2点間の距離.
    float backDist = 0.0f;

    /// <summary>
    /// 指を離した時間
    /// </summary>
    Timer separateTime = new Timer();

    MapManager mapManager;
    
    void Start()
    {
        mapManager = MapManager.I;
    }

    void Update()
    {
        separateTime.Update();
        if (separateTime.IsLimitTime) separateTime.Stop(true);

        //エディターで複数タッチはできないことが多いので
#if UNITY_EDITOR
        currentScaleRate += Input.mouseScrollDelta.y * 0.2f;
#else
        float pinchValue = GetPinchValue();
        float tempSpeed = scalingSpeed + ((imageRect.localScale.x/ scaleRateMax) * (0.1f * mapManager.screenSizeRate));
        currentScaleRate += pinchValue * scalingSpeed;
#endif
        // 限界値をオーバーした際の処理
        if (currentScaleRate > scaleRateMax)
        {
            if (Input.touchCount <= 0)
            {
                if (!separateTime.IsWorking) separateTime.TimerStart(1.0f);
                currentScaleRate = Mathf.Lerp(currentScaleRate, scaleRateMax, separateTime.Progress);
            }
            currentScaleRate = Mathf.Min(currentScaleRate, scaleRateMax + 3.0f);
        }
        else if (currentScaleRate < scaleRateMin)
        {
            if (Input.touchCount <= 0)
            {
                if (!separateTime.IsWorking) separateTime.TimerStart(1.0f);
                currentScaleRate = Mathf.Lerp(currentScaleRate, scaleRateMin, separateTime.Progress);
            }

            currentScaleRate = Mathf.Max(currentScaleRate, scaleRateMin * 0.5f);
        }

        // 相対値が変更した場合、カメラに相対値を反映させる
        if (currentScaleRate != 0)
        {
            imageRect.transform.localScale = new Vector3(currentScaleRate, currentScaleRate, 1.0f);
        }
    }

    //２本の指の間隔を広くした時がプラス、狭めた時がマイナスの値になる
    float GetPinchValue()
    {
        if (Input.touchCount < 2) return 0.0f;

        // タッチしている２点を取得
        Touch t1 = Input.GetTouch(0);
        Touch t2 = Input.GetTouch(1);

        //2点タッチ開始時の距離を記憶
        if (t2.phase == TouchPhase.Began)
        {
            backDist = Vector2.Distance(t1.position, t2.position);
            return 0.0f;
        }
        else if (t1.phase == TouchPhase.Moved || t2.phase == TouchPhase.Moved)
        {
            float offset = 0.0001f;
            if (t1.phase == TouchPhase.Stationary)
            {
                offset = 0.0002f;
            }

            // タッチ位置の移動後、長さを再測し、前回の距離からの相対値を取る。
            return (Vector2.Distance(t1.position, t2.position) - backDist) * offset;
        }

        return 0.0f;
    }
}
