using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameStep : InGameBaseObj {
    float baseX, moveWidth;
    // Use this for initialization

    float moveTime = 0;

    bool isstop = false;

	void Start () {
        baseX = transform.position.z;

        moveWidth = GameConst.MAP_WIDTH / 2 - Mathf.Abs(baseX);
        moveWidth = Random.Range(1f, moveWidth);

        moveTime = Random.Range(0f, 3.14f);

        transform.position = new Vector3(
            transform.position.x,
            transform.position.y,
            baseX + Mathf.Sin(moveTime) * moveWidth
        );
	}
	
	// Update is called once per frame
	void Update () {
        if (isstop) return;
        moveTime += Time.deltaTime;

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
        }
    }
}
