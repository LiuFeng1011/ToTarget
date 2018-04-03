using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePadScoresLabel : MonoBehaviour {

    float labelActionTime = 0f, labelActionMaxTime = 0.5f, toTargetTime = 0f, toTargetDeltaTime = 0.2f;

    UILabel scoreslabel;
    Vector3 baseLabelScale;

    int nowScores, targetScores;
	// Use this for initialization
	void Start () {
        scoreslabel = gameObject.GetComponent<UILabel>();
        baseLabelScale = scoreslabel.transform.localScale;
        scoreslabel.text = "0";
	}
	
	// Update is called once per frame
	void Update () {
        if (labelActionTime > 0) LabelAction();

        if(nowScores != targetScores) ToTarget();
	}

    void LabelAction()
    {
        labelActionTime = Mathf.Max(labelActionTime - Time.deltaTime, 0);
        float rate = 1 - labelActionTime / labelActionMaxTime;
        float scale = Mathf.Sin((3.14f) * rate) * 0.5f;
        scoreslabel.transform.localScale = baseLabelScale + new Vector3(scale, scale, scale);
    }

    void ToTarget(){
        toTargetTime += Time.deltaTime;
        Debug.Log(toTargetTime);
        if (toTargetTime < toTargetDeltaTime) return;

        toTargetTime = 0f;
        nowScores += (int)Mathf.Ceil((float)(targetScores - nowScores) * 0.3f);
        nowScores = Mathf.Min(nowScores, targetScores);

        labelActionTime = labelActionMaxTime;

        scoreslabel.text = ""+nowScores;
    }


    public void SetScores(int val)
    {
        targetScores = val;
        toTargetTime = 1.0f;
    }

}
