using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCenterManager : BaseGameObject {
    
    static GameCenterManager gameCenterManager;

    public static GameCenterManager GetInstance(){
        if(gameCenterManager == null){
            gameCenterManager = new GameCenterManager();
            gameCenterManager.Init();
        }
        return gameCenterManager;
    }

    public void Init(){
        UnityEngine.Social.localUser.Authenticate(AccessGameCenterCallback);
    }

    private void AccessGameCenterCallback(bool success)
    {
        if (success) {
            Debug.Log("GameCenterCallback true"); 
        }
        else { 
            Debug.Log("GameCenterCallback false");
        }
    }

    public void UploadScores(string lbname , int scores){
        Social.ReportScore(scores, lbname, UploadScoresCB); 
    }
    //上传排行榜分数  
    public void UploadScoresCB(bool success)  
    {  
        Debug.Log("*** UploadScoresCB: success = " + success);  
    }  

    //打开排行榜
    public void Showlb(){
        Social.ShowLeaderboardUI(); 
    }
}
