using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRestart : MonoBehaviour
{
    [SerializeField]
    GameObject CameraRestartButton;
	// Use this for initialization
	void Start ()
    {
        CameraRestartButton.SetActive(false);
        StartCoroutine(RestartButtonActive());
	}
	
	IEnumerator RestartButtonActive()
    {
        yield return new WaitForSeconds(5);
        CameraRestartButton.SetActive(true);
    }
}
