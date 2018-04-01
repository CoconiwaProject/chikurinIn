using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppDataText : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<UnityEngine.UI.Text>().text= AppData.SelectTargetName;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
