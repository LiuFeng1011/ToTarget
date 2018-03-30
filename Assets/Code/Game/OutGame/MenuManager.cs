using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GameObject startBtn = GameObject.Find("UI Root").transform.Find("StartGame").gameObject;
        UIEventListener.Get(startBtn).onClick = StartCB;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void StartCB(GameObject go)
    {
        (new EventChangeScene(GameSceneManager.SceneTag.Game)).Send();
    }
}
