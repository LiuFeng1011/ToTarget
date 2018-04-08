using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameLevelManager : BaseGameObject {

    float addStepDis = 0;

    List<InGameStep> stepList = new List<InGameStep>();

    float minSize, maxSize, nowSize, randomRange = 0.5f;

    public void Init(){
        minSize = GameConst.STEP_MIN_SIZE;
        maxSize = GameConst.STEP_MAX_SIZE;
        nowSize = maxSize;
    }

    public void Update(){

        //
        float roledis = addStepDis - InGameManager.GetInstance().role.transform.position.x;

        while (roledis < 10){
            addStepDis += Random.Range(4f, 6f);
            //add step
            AddStep();
            roledis = addStepDis - InGameManager.GetInstance().role.transform.position.x;
        }

        while(stepList.Count > 0 && stepList[0].transform.position.x + 10 < InGameManager.GetInstance().role.transform.position.x){
            MonoBehaviour.Destroy(stepList[0].gameObject);
            stepList.RemoveAt(0);
        }
    }

    public void AddStep(){
        GameObject obj = Resources.Load("Prefabs/MapObj/InGameStep") as GameObject;
        obj = MonoBehaviour.Instantiate(obj);

        obj.transform.position = new Vector3(addStepDis, 0f,Random.Range(-GameConst.MAP_OBJ_MAX_POSX,GameConst.MAP_OBJ_MAX_POSX));
        InGameStep step = obj.GetComponent<InGameStep>();
        stepList.Add(step);

        float dis = InGameManager.GetInstance().role.transform.position.x;

        nowSize = 1-Mathf.Min(dis / 200f, 1f);

        nowSize = minSize + (maxSize - minSize) * nowSize;
        step.Init(Mathf.Max(nowSize-randomRange,minSize),Mathf.Min(nowSize + randomRange,maxSize));
    }

    public void Destroy(){
        
    }
}
