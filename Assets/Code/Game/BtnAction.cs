using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnAction : MonoBehaviour {
    float actionTime = 0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        actionTime += Time.deltaTime;

        float scale = 1 + Mathf.Sin(actionTime*5)*0.1f;

        transform.localScale = new Vector3(scale,scale,1f);
	}
}
