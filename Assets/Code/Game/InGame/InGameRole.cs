using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameRole : InGameBaseObj {

    public Camera camera;
    public float validTouchDistance; //200  

    RoleJump jump;

    Vector3 camearDis,jumpPos;

    bool hit = false;

    GameObject flag;

    int combo = 0,scores = 0;

    private void Awake()
    {
        camera = Camera.main;
        validTouchDistance = 200;

        EventManager.Register(this,
                       EventID.EVENT_TOUCH_DOWN);


        jump = transform.GetComponent<RoleJump>();

        camearDis = camera.transform.position - transform.position;
        jumpPos = transform.position;
    }
    // Use this for initialization
    void Start () {

        //Flag
        GameObject flagObj = Resources.Load("Prefabs/MapObj/TargetFlag") as GameObject;
        flag = Instantiate(flagObj);
        flag.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        float x = camera.transform.position.x + (transform.position.x + camearDis.x - camera.transform.position.x) * 0.2f;
        camera.transform.position = new Vector3(x, camera.transform.position.y, camera.transform.position.z);

        InGameManager.GetInstance().groundPlane.transform.position = new Vector3(
            transform.position.x,
            InGameManager.GetInstance().groundPlane.transform.position.y,
            InGameManager.GetInstance().groundPlane.transform.position.z);

        jump.JumpUpdate();
	}

    public void JumpStart(Vector3 targetPos){
        hit = false;
        jump.JumpStart(transform.position, targetPos, 5);
        InGameManager.GetInstance().touchPlane.gameObject.SetActive(false);

        flag.transform.position = targetPos;
        flag.SetActive(true);
    }

    public bool JumpFinished(){
        flag.SetActive(false);
        if (!hit) {
            // game over
            InGameManager.GetInstance().GameOver();
            combo = 0;
            gameObject.SetActive(false);
            //create efffect
            GameObject effect = Resources.Load("Prefabs/Effect/RoleDieEffect") as GameObject;
            effect = Instantiate(effect);
            effect.transform.position = transform.position;
            return false;
        }
        jump.Stop();
        ResetTouchPlane();
        jumpPos = transform.position;

        return true;
    }

    public override void HandleEvent(EventData resp)
    {

        switch (resp.eid)
        {
            case EventID.EVENT_TOUCH_DOWN:
                EventTouch eve = (EventTouch)resp;
                TouchToPlane(eve.pos);
                //Fire(GameCommon.ScreenPositionToWorld(eve.pos));
                break;
        }

    }

    public void TouchToPlane(Vector3 pos)
    {
        Ray ray = camera.ScreenPointToRay(pos);

        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, validTouchDistance, LayerMask.GetMask("TouchPlane")))
        {
            GameObject gameObj = hitInfo.collider.gameObject;
            Vector3 hitPoint = hitInfo.point;
            JumpStart(hitPoint);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if(hit ){
            return;
        }
        Debug.Log(other.gameObject.name);
        InGameBaseObj obj = other.transform.GetComponent<InGameBaseObj>();
        if (obj == null) {
            Debug.Log("cant find InGameBaseObj : " + other.gameObject.name);
            return;
        }
        if(obj.mytype == enObjType.step){
            //if(!jump.isfull) JumpFinished();
            hit = true;

            //combo
            if (Mathf.Abs(transform.position.x - obj.transform.position.x) < GameConst.comboDis && 
                Mathf.Abs(transform.position.z - obj.transform.position.z) < GameConst.comboDis){
                GameObject effect = Resources.Load("Prefabs/Effect/ShowEnemy") as GameObject;
                effect = Instantiate(effect);
                effect.transform.position = obj.transform.position;
                effect.transform.localScale = new Vector3(obj.transform.localScale.x, obj.transform.localScale.z, obj.transform.localScale.y);

                combo += 1;
            }else {
                combo = 0;
            }

            int val = 1 + combo;

            scores += val;
            InGameManager.GetInstance().inGameUIManager.AddScores(transform.position,val,scores);
        }
    }

    public void Revive(){
        hit = true;

        gameObject.SetActive(true);

        jump.Stop();

        transform.position = jumpPos;
        flag.transform.position = jumpPos;

        Invoke("ResetTouchPlane", 1f);
    }

    public void ResetTouchPlane(){
        InGameManager.GetInstance().touchPlane.gameObject.SetActive(true);
        InGameManager.GetInstance().touchPlane.transform.position = new Vector3(transform.position.x, 0, 0);
    }

    private void OnDestroy()
    {
        EventManager.Remove(this);
    }
}
