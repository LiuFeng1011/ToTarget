using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour {
    static MenuManager instance;
    public static MenuManager GetInstance() { return instance; }

    GameObject yesObj;

    private void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start () {
        Transform menu = GameObject.Find("UI Root").transform.Find("Menu");
        GameObject startBtn = menu.Find("StartGame").gameObject;
        UIEventListener.Get(startBtn).onClick = StartCB;

        yesObj = menu.Find("Yes").gameObject;

        GameObject model1Obj = menu.Find("model1").gameObject;
        GameObject model2Obj = menu.Find("model2").gameObject;

        UIEventListener.Get(model1Obj).onClick = Model1;
        UIEventListener.Get(model2Obj).onClick = Model2;

	}

    void Model1(GameObject obj){
        yesObj.transform.localPosition = new Vector3(yesObj.transform.localPosition.x, obj.transform.localPosition.y, 0);
        //sel model 1
    }

    void Model2(GameObject obj){
        yesObj.transform.localPosition = new Vector3(yesObj.transform.localPosition.x, obj.transform.localPosition.y, 0);
        //sel model 2
        
    }

	// Update is called once per frame
	void Update () {
		
	}
    void StartCB(GameObject go)
    {
        (new EventChangeScene(GameSceneManager.SceneTag.Game)).Send();
    }

    private void OnDestroy()
    {
        instance = null;
    }
}
