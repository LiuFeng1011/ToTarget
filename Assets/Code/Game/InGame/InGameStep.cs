using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameStep : InGameBaseObj {
    float baseX, moveWidth, speed, downTime = 0, downMaxTime = 0.6f;
    // Use this for initialization

    float moveTime = 0;

    bool isstop = false;

    static Material m_light;

    public GameObject childObj;


    public static AnimationCurve downAC;
    //public AnimationCurve downAC_;

	void Start () {

        if(m_light == null){
            m_light = Resources.Load<Material>("Materials/InGameStep_light");
        }

        if(downAC == null){
            Keyframe[] ks = {
                new Keyframe(0f,-0.5f),
                new Keyframe(0.5f,0.5f),
                new Keyframe(1f,0f)
            };

            downAC = new AnimationCurve(ks);
        }

        float scale = Random.Range(1f, 2.5f);
        transform.localScale = new Vector3(scale,transform.localScale.y,scale);
        baseX = transform.position.z;

        moveWidth = GameConst.MAP_WIDTH / 2 - Mathf.Abs(baseX);
        moveWidth = Random.Range(1f, moveWidth);

        moveTime = Random.Range(0f, 3.14f);
        speed = Random.Range(0.5f, 2.5f);

         Material _m = childObj.GetComponent<Renderer>().material;
        float speedrate = (speed - 0.5f) / 2f;
        _m.color = new Color( speedrate, 1-speedrate, 1-speedrate);

        transform.position = new Vector3(
            transform.position.x,
            downAC.Evaluate(0),
            baseX + Mathf.Sin(moveTime) * moveWidth
        );
	}
	
	// Update is called once per frame
	void Update () {
        if (downMaxTime > downTime ){
            downTime += Time.deltaTime;
            downTime = Mathf.Min(downTime + Time.deltaTime, downMaxTime);

            float rate = downTime / downMaxTime;
            float y = downAC.Evaluate(rate);

            transform.position = new Vector3(  transform.position.x, y, transform.position.z );

            return;
        }
        if (isstop) return;
        moveTime += Time.deltaTime * speed;

        transform.position = new Vector3(
            transform.position.x,
            transform.position.y,
            baseX + Mathf.Sin(moveTime) * moveWidth
        );
	}

    private void OnTriggerEnter(Collider other)
    {

        InGameBaseObj obj = other.transform.GetComponent<InGameBaseObj>();
        if (obj == null)
        {
            Debug.Log("cant find InGameBaseObj : " + other.gameObject.name);
            return;
        }
        if (obj.mytype == enObjType.role)
        {
            isstop = true;
            childObj.GetComponent<Renderer>().material = m_light; 

        }
    }
}
