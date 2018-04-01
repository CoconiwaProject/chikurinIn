using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAcceraletionMove : MonoBehaviour
{
    [SerializeField]
    float Threshold = 0.1f;
    [SerializeField]
    float xMaximum = 500.0f;

    [SerializeField]
    float xMinimum = -500.0f;

    [SerializeField]
    float m_Speed = 0.5f;
    
    float m_sideAcceleration = 0.0f;

    Vector3 rightVec = Vector3.right;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        SideMove();
	}

    void SideMove()
    {
        float acceleration = GetAccelaration();
        if (Mathf.Abs(acceleration) < Threshold) return;

        m_sideAcceleration += acceleration * m_Speed;

        transform.localPosition += rightVec * acceleration;

        //maximum
        if(transform.localPosition.x>xMaximum)
        {
            transform.localPosition = new Vector3( xMaximum,transform.localPosition.y,transform.localPosition.z);
            m_sideAcceleration = 0.0f;
        }

        //minmum
        if (transform.localPosition.x < xMinimum)
        {
            transform.localPosition = new Vector3(xMinimum, transform.localPosition.y, transform.localPosition.z);
            m_sideAcceleration = 0.0f;
        }
    }

    float GetAccelaration()
    {
#if UNITY_EDITOR
        return Input.GetAxis("Horizontal") * 100.0f;
#else
        return Input.acceleration.x;
#endif
    }
}
