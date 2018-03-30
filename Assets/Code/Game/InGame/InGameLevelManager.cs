using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameLevelManager : BaseGameObject {

    float addStepDis = 0;

    List<InGameStep> stepList = new List<InGameStep>();


    public void Init(){
        
    }

    public void Update(){
        float roledis = addStepDis - InGameManager.GetInstance().role.transform.position.x;

        while (roledis < 10){
            addStepDis += Random.Range(2, 5);
            //add step
            AddStep();
            roledis = addStepDis - InGameManager.GetInstance().role.transform.position.x;
        }

        while(stepList.Count > 0 && stepList[0].transform.position.x + 5 < InGameManager.GetInstance().role.transform.position.x){
            MonoBehaviour.Destroy(stepList[0].gameObject);
            stepList.RemoveAt(0);
        }
    }

    public void AddStep(){
        GameObject obj = Resources.Load("Prefabs/MapObj/InGameStep") as GameObject;
        obj = MonoBehaviour.Instantiate(obj);

        obj.transform.position = new Vector3(addStepDis,0,Random.Range(-3,3));
        InGameStep step = obj.GetComponent<InGameStep>();
        stepList.Add(step);

    }

    public void Destroy(){
        
    }
}
