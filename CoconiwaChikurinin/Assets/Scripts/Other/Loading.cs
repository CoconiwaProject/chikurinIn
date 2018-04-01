using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loading : MonoBehaviour
{
    void Start()
    {
        KKUtilities.WaitSeconde(2.0f, () => {
            SceneLoadManager.I.LoadSceneAsync("Content");
            UnderBerMenu.I.ChangeIconActive("Content");
        }, this);
    }
}
