using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraStartUp : MonoBehaviour {

    // Use this for initialization
    void Start()
    {
        KKUtilities.WaitSeconde(2.0f,()=> {
            SceneLoadManager.I.LoadSceneAsync("Camera");
        },this);
      
    }

    // Update is called once per frame
    void Update () {
      
	}
}

