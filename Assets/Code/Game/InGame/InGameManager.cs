using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : MonoBehaviour {
    static InGameManager instance;

    public InGameRole role;

    public GameObject touchPlane,groundPlane;

    GameTouchController gameTouchController;
    public InGameLevelManager inGameLevelManager;
    public InGameUIManager inGameUIManager;

    public RapidBlurEffectManager rapidBlurEffectManager;

    public Camera gamecamera;

    int reviveCount = 0;

    public static InGameManager GetInstance(){
        return instance;
    }

    private void Awake()
    {
        instance = this;
        gamecamera = Camera.main;

        rapidBlurEffectManager = gamecamera.gameObject.AddComponent<RapidBlurEffectManager>();
    }

    // Use this for initialization
    void Start () {
        gameTouchController = new GameTouchController();

        //创建角色
        GameObject roleObj = Resources.Load("Prefabs/MapObj/InGameRole") as GameObject;
        roleObj = Instantiate(roleObj);
        role = roleObj.GetComponent<InGameRole>();

        //地面
        GameObject touchPlaneObj = Resources.Load("Prefabs/MapObj/TouchPlane") as GameObject;
        touchPlane = Instantiate(touchPlaneObj);


        GameObject groundPlaneObj = Resources.Load("Prefabs/MapObj/GroundPlane") as GameObject;
        groundPlane = Instantiate(groundPlaneObj);
        //
        inGameLevelManager = new InGameLevelManager();
        inGameLevelManager.Init();

        inGameUIManager = new InGameUIManager();
        inGameUIManager.Init();
    }
	
	// Update is called once per frame
	void Update () {
        if (gameTouchController != null) gameTouchController.Update();
        if(inGameLevelManager != null)inGameLevelManager.Update();
        if (inGameUIManager != null) inGameUIManager.Update();
	}

    private void OnDestroy()
    {
        instance = null;
        if (inGameLevelManager != null) inGameLevelManager.Destroy();
        if (inGameUIManager != null) inGameUIManager.Destroy();
    }

    public void GameOver(){
        rapidBlurEffectManager.StartBlur();
        Invoke("ShowOverLayer", 1.0f);
    }

    public void ShowOverLayer(){
        if (reviveCount <= 0)
        {
            inGameUIManager.ShowReviveLayer();
        }
        else
        {
            inGameUIManager.ShowResultLayer();
        }
        reviveCount += 1;
    }

    public void Revive(){
        rapidBlurEffectManager.OverBlur();
        role.Revive();
    }

    public void Restart(){
        reviveCount = 0;

    }

}
