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

    InGameBaseModel modelManager;

    enGameState gameState;

    public static float gameTime = 0f;

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

        int selmodel = PlayerPrefs.GetInt(GameConst.USERDATANAME_MODEL, 0);
        modelManager = InGameBaseModel.Create(selmodel);
        modelManager.Init();

        gameState = enGameState.playing;
    }
	
	// Update is called once per frame
	void Update () {
        gameTime += Time.deltaTime;
        if (inGameUIManager != null) inGameUIManager.Update();

        if(gameState != enGameState.playing){
            return;
        }

        if (gameTouchController != null) gameTouchController.Update();
        if(inGameLevelManager != null)inGameLevelManager.Update();
        if (modelManager != null) modelManager.Update();
        if (role != null) role.RoleUpdate();
	}

    private void OnDestroy()
    {
        instance = null;
        if (inGameLevelManager != null) inGameLevelManager.Destroy();
        if (inGameUIManager != null) inGameUIManager.Destroy();
        if (modelManager != null) modelManager.Destroy();
    }

    public void GameOver(){
        gameState = enGameState.over;

        rapidBlurEffectManager.StartBlur();
        Invoke("ShowOverLayer", 1.0f);
    }

    public void ShowOverLayer(){

        gameState = enGameState.over;

        int selmodel = PlayerPrefs.GetInt(GameConst.USERDATANAME_MODEL, 0);
        int basescores = PlayerPrefs.GetInt(GameConst.USERDATANAME_MODEL_MAXSCORES + selmodel);

        int thisscores = role.scores;
        if (basescores < thisscores)
        {
            PlayerPrefs.SetInt(GameConst.USERDATANAME_MODEL_MAXSCORES + selmodel, thisscores);

            GameCenterManager.GetInstance().UploadScores(GameConst.gameModels[selmodel].lbname,thisscores);
        }
        PlayerPrefs.SetInt(GameConst.USERDATANAME_MODEL_LASTSCORES + selmodel, thisscores);

        if (reviveCount <= 0 && ADManager.GetInstance().isAdLoaded)
        {
            inGameUIManager.ShowReviveLayer();
        }
        else
        {
            inGameUIManager.ShowResultLayer();

        }
        reviveCount += 1;
    }

    public void Pause(){
        gameState = enGameState.pause;
    }

    public void Resume(){
        gameState = enGameState.playing;
    }

    public void Revive(){

        gameState = enGameState.playing;

        rapidBlurEffectManager.OverBlur();
        role.Revive();
        modelManager.Revive();
    }

    public void Restart(){
        reviveCount = 0;

    }

}
