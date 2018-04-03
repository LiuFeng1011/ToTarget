using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReviveLayerManager : InGameUIBaseLayer {

    float timeCount;
    int nowTime;

    UILabel timeLabel;

    GameObject reviveBtn, cancelBtn;

    public override void Init(){
        base.Init();
        reviveBtn = transform.Find("reviveBtn").gameObject;
        UIEventListener.Get(reviveBtn).onClick = Revive;

        cancelBtn = transform.Find("cancelBtn").gameObject;
        UIEventListener.Get(cancelBtn).onClick = Cancel;

        timeLabel = transform.Find("Time").GetComponent<UILabel>();
    }
	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        if(timeCount <=0){
            return;
        }
        timeCount -= Time.deltaTime;
        if( timeCount <= 0){
            Cancel(null);
            return;
        }
        if((int)timeCount != nowTime){
            nowTime = (int)timeCount;
            timeLabel.text = nowTime + "";
        }

	}

    void Revive(GameObject obj){
        InGameManager.GetInstance().Revive();
        InGameManager.GetInstance().inGameUIManager.Revive();
        //gameObject.SetActive(false);
        Hide();
    }

    void Cancel(GameObject obj){
        InGameManager.GetInstance().inGameUIManager.ShowResultLayer();
        //gameObject.SetActive(false);
        Hide();
    }

    public override void Show(){
        base.Show();
        timeCount = 5.0f;
        nowTime = 5;
    }

}
