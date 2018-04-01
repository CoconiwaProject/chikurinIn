using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class CameraFocusMode : MonoBehaviour {

    [System.Serializable]
    public enum FocusMode
    {
        FOCUS_MODE_NORMAL = 0,
        FOCUS_MODE_TRIGGERAUTO = 1,
        FOCUS_MODE_CONTINUOUSAUTO = 2,
        FOCUS_MODE_INFINITY = 3,
        FOCUS_MODE_MACRO = 4
    }

    public FocusMode m_FocusMode = FocusMode.FOCUS_MODE_CONTINUOUSAUTO;

    void Start()
    {
        VuforiaBehaviour qcar = (VuforiaBehaviour)FindObjectOfType(typeof(VuforiaBehaviour));
        if (qcar)
        {
            qcar.RegisterVuforiaStartedCallback(OnQCARStarted);
        }
        else
        {
            Debug.Log("Failed to find QCARBehaviour in current scene");
        }
    }

    private void OnQCARStarted()
    {
        Debug.Log("Vuforia has started.");
        bool autofocusOK = CameraDevice.Instance.SetFocusMode((CameraDevice.FocusMode)m_FocusMode);
        if (autofocusOK)
        {
            Debug.Log("Successfully enabled Continuous Autofocus mode");
        }
        else
        {
            // set a different focus mode (for example, FOCUS_NORMAL):
            CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_NORMAL);
        }
    }

    void Update()
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_TRIGGERAUTO);
                Debug.Log("focus");
            }
        }
    }

}
