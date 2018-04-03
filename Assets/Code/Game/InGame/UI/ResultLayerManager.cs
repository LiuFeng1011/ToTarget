using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultLayerManager : InGameUIBaseLayer {

    public UILabel scoreLabel;


    public override void Init(){
        base.Init();
        GameObject exitbtn = transform.Find("exit").gameObject;
        UIEventListener.Get(exitbtn).onClick = ExitBtn;

        GameObject replaybtn = transform.Find("replay").gameObject;
        UIEventListener.Get(replaybtn).onClick = ReplayBtn;

        scoreLabel = transform.Find("scores").GetComponent<UILabel>();
    }

	// Use this for initialization
	void Start () {



	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void Show(){
        base.Show();
        gameObject.SetActive(true);
    }

    public void SetVal(int val){

        scoreLabel.text = val + "";
    }

    void ExitBtn(GameObject obj){
        (new EventChangeScene(GameSceneManager.SceneTag.Menu)).Send();
        gameObject.SetActive(false);
    }

    void ReplayBtn(GameObject obj){
        (new EventChangeScene(GameSceneManager.SceneTag.Game)).Send();
        gameObject.SetActive(false);
    }
}
