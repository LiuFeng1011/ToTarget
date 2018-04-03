using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUIManager : BaseGameObject {

    ResultLayerManager  resultLayerManager;
    ReviveLayerManager  reviveLayerManager;
    GamePadManager      gamePadManager;

    GameObject uiroot,addScoresLabelRes;
    Camera uicamera;

    public void Init()
    {
        uiroot = GameObject.Find("UI Root");
        resultLayerManager  = uiroot.transform.Find("ResultLayer").GetComponent<ResultLayerManager>();
        reviveLayerManager  = uiroot.transform.Find("ReviveLayer").GetComponent<ReviveLayerManager>();
        gamePadManager      = uiroot.transform.Find("GamePad").GetComponent<GamePadManager>();

        resultLayerManager.Init();
        reviveLayerManager.Init();
        gamePadManager.Init();

        resultLayerManager.gameObject.SetActive(false);
        reviveLayerManager.gameObject.SetActive(false);
        gamePadManager.gameObject.SetActive(true);

        addScoresLabelRes = Resources.Load("Prefabs/UI/AddScoresLabel") as GameObject;

        uicamera = uiroot.transform.Find("Camera").GetComponent<Camera>();
    }
	
	// Update is called once per frame
	public void Update () {
        resultLayerManager.ActionUpdate();
        reviveLayerManager.ActionUpdate();
        gamePadManager.ActionUpdate();
	}

    public void Destroy()
    {

    }

    public void ShowResultLayer(){
        resultLayerManager.Show();
        resultLayerManager.SetVal(123);
        HideGamePad();
    }

    public void ShowReviveLayer(){
        reviveLayerManager.Show();
        HideGamePad();
    }

    public void Revive(){
        ShowGamePad();
    }

    public void HideGamePad()
    {
        gamePadManager.Hide();
    }

    public void ShowGamePad()
    {
        gamePadManager.Show();
    }

    public void AddScores(Vector3 worldPos, int scores, int sumscores, bool createLabel = true){

        if(createLabel){
            GameObject labelObj = NGUITools.AddChild(uiroot, addScoresLabelRes);
            //GameObject labelObj = MonoBehaviour.Instantiate(addScoresLabelRes);
            AddScoresLabel label = labelObj.GetComponent<AddScoresLabel>();
            Vector3 pos = GameCommon.WorldPosToNGUIPos(Camera.main, UICamera.currentCamera, worldPos);
            Debug.Log(pos);
            label.Init(pos, scores);
            label.transform.position = pos;
        }

        gamePadManager.SetScores(sumscores);
    }
}
