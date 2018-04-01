using UnityEngine;

public class VRFirstPersonCameraController : MonoBehaviour
{
    Quaternion gyro;
    Transform m_transform;

    [SerializeField]
    float speed = 0.1f;
    //キャッシュ
    Vector3 zeroVec = Vector3.zero;

    float screenSizeRate;

    bool isSwipe = false;

    [SerializeField]
    Transform sphere = null;

    float longitude = 0.0f;
    float latitude = -7.0f;

    void Start()
    {
        Input.gyro.enabled = true;
        m_transform = transform;
        TouchManager.Instance.Drag += OnSwipe;

        screenSizeRate = 1.0f + (1.0f - ((float)Screen.width / 1080));
    }

#if !UNITY_EDITOR
    void OnDestroy()
    {
        TouchManager.Instance.Drag -= OnSwipe;
    }
#endif

    void Update()
    {
        if (isSwipe) return;
#if !UNITY_EDITOR
        gyro = Input.gyro.attitude;
        //ジャイロはデフォルトで下を向いているので90度修正。X軸もY軸も逆のベクトルに変換
        gyro = Quaternion.Euler(90.0f, 0.0f, 0.0f) * (new Quaternion(-gyro.x, -gyro.y, gyro.z, gyro.w));
        m_transform.localRotation = gyro;
#endif
    }

    void LateUpdate()
    {
        isSwipe = false;
    }

    void OnSwipe(object sender, CustomInputEventArgs e)
    {
        isSwipe = true;
        float input;
        input = e.Input.DeltaPosition.x;
        
        longitude += input * Time.deltaTime * speed;

        Vector3 targetPosition = transform.position + SphereCoordinate(longitude, latitude, 10.0f);
        sphere.LookAt(targetPosition);
    }

    public void SetDifferenceVec(Vector2 vec)
    {
        longitude += vec.x;
        latitude += vec.y;

        Vector3 targetPosition = transform.position + SphereCoordinate(longitude, latitude, 10.0f);
        sphere.LookAt(targetPosition);
    }

    /// <summary>
    /// 指定した角度の球体座標を返す
    /// </summary>
    /// <param name="longitude">経度</param>
    /// <param name="latitude">緯度</param>
    /// <returns></returns>
    public Vector3 SphereCoordinate(float longitude, float latitude, float distance)
    {
        Vector3 position = zeroVec;

        //重複した計算
        longitude *= Mathf.Deg2Rad;
        latitude *= Mathf.Deg2Rad;
        float temp = distance * Mathf.Cos(latitude);

        position.x = temp * Mathf.Sin(longitude);
        position.y = distance * Mathf.Sin(latitude);
        position.z = temp * Mathf.Cos(longitude);

        return position;
    }
}
