using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteAnimation : MonoBehaviour
{

    [SerializeField]
    ColorAnimation m_ColorAnimation;
    [SerializeField]
    ScaleAnimation m_ScaleAnimation;


    public void PlayAll()
    {
        m_ColorAnimation.Play();
        m_ScaleAnimation.Play();
    }


    // Use this for initialization
    void Start()
    {
        m_ColorAnimation.Initialize(gameObject);
        m_ScaleAnimation.Initialize(gameObject);

    }

    // Update is called once per frame
    void Update()
    {
        m_ColorAnimation.Update();
        m_ScaleAnimation.Update();
    }
}


[System.Serializable]
public class SpriteAnimationBase
{
    public Vector4 m_TargetValue;
    public float m_Duration;
    public bool isPlayOnAwake;
    protected bool isPlay;
    protected float m_Time;

    public virtual void Initialize(GameObject gameObject)
    {
        if (isPlayOnAwake)
        {
            isPlay = true;
        }
        else
        {
            isPlay = false;
        }
    }

    public virtual void Play()
    {
        isPlay = true;
    }

    public virtual void Update()
    {

    }

}

[System.Serializable]
public class ColorAnimation : SpriteAnimationBase
{
    Image m_Image;
    Color m_StartColor;
    Color m_TargetColor;

    public override void Initialize(GameObject gameObject)
    {
        m_Image = gameObject.GetComponent<Image>();
        m_StartColor = m_Image.color;
        m_TargetColor = m_TargetValue;
        base.Initialize(gameObject);
    }

    public override void Update()
    {
        if (!isPlay) return;
        m_Time += Time.deltaTime;

        m_Image.color = Color.Lerp(m_StartColor,m_TargetColor,m_Time/m_Duration);

        if (m_Time >= m_Duration)
        {
            isPlay = false;
        }
        base.Update();
    }

}

[System.Serializable]
public class ScaleAnimation : SpriteAnimationBase
{
    Transform transform;
    Vector3 m_StartScale;
    Vector3 m_TargetScale;

    public override void Initialize(GameObject gameObject)
    {
        transform = gameObject.transform;
        m_StartScale = transform.localScale;
        m_TargetScale = new Vector3(m_TargetValue.x,m_TargetValue.y,m_TargetValue.z);
        base.Initialize(gameObject);
    }

    public override void Update()
    {
        if (!isPlay) return;
        m_Time += Time.deltaTime;

        transform.localScale = Vector3.Lerp(m_StartScale, m_TargetScale, m_Time / m_Duration);

        if (m_Time >= m_Duration)
        {
            isPlay = false;
        }
        base.Update();
    }
}

