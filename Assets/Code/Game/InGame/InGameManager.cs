using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : MonoBehaviour {
    static InGameManager instance;

    public InGameRole role;

    public GameObject touchPlane,groundPlane;

    GameTouchController gameTouchController;
    InGameLevelManager inGameLevelManager;

    public static InGameManager GetInstance(){
        return instance;
    }

    private void Awake()
    {
        instance = this;
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
    }
	
	// Update is called once per frame
	void Update () {
        if (gameTouchController != null) gameTouchController.Update();
        if(inGameLevelManager != null)inGameLevelManager.Update();
	}

    private void OnDestroy()
    {

        if (inGameLevelManager != null) inGameLevelManager.Destroy();
    }
}
