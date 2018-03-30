using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameRole : InGameBaseObj {

    Camera camera;
    public float validTouchDistance; //200  

    RoleJump jump;

    Vector3 camearDis;

    private void Awake()
    {
        camera = Camera.main;
        validTouchDistance = 200;

        EventManager.Register(this,
                       EventID.EVENT_TOUCH_DOWN);


        jump = transform.GetComponent<RoleJump>();

        camearDis = camera.transform.position - transform.position;
    }
    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        float x = camera.transform.position.x + (transform.position.x + camearDis.x - camera.transform.position.x) * 0.2f;
        camera.transform.position = new Vector3(x, camera.transform.position.y, camera.transform.position.z);

        jump.JumpUpdate();
	}

    public void JumpStart(Vector3 targetPos){
        jump.JumpStart(transform.position, targetPos, 5);
        InGameManager.GetInstance().touchPlane.gameObject.SetActive(false);
    }

    public void JumpFinished(){
        jump.Stop();
        InGameManager.GetInstance().touchPlane.gameObject.SetActive(true);
        InGameManager.GetInstance().touchPlane.transform.position = new Vector3(transform.position.x,0, 0);
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
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

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

        InGameBaseObj obj = other.transform.GetComponent<InGameBaseObj>();
        if (obj == null) {
            Debug.Log("cant find InGameBaseObj : " + other.gameObject.name);
            return;
        }
        if(obj.mytype == enObjType.step){
            JumpFinished();
        }
    }
}
