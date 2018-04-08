using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Together;

public class ADManager {

    public const string ADAPPID = "2f76MS7770Rz5Bk4v7ny";
    public const string ADSCENEID_REVIVE = "61E0TnppulAQhMIIuGq";
    public const string ADSCENEID_GAMEOVER = "ecCZVaP7JaBJdRDIBj2";

    static ADManager instance;
    public bool isAdLoaded = false;
    public static ADManager GetInstance(){
        if(instance == null){
            instance = new ADManager();
            instance.ADManagerInit();
        }
        return instance;
    }

	// Use this for initialization
    public void ADManagerInit () {
#if UNITY_EDITOR
        isAdLoaded = true;
#endif
        TGSDK.Initialize(ADAPPID);
        TGSDK.PreloadAd();

        // 视频类广告已准备好
        TGSDK.VideoAdLoadedCallback = OnVideoAdLoaded;
	}
	
    public void OnVideoAdLoaded(string ret)
    {
        
        isAdLoaded = true;
    }

    public void PlayReviveAD(System.Action<string> action,System.Action<string> closeaction){
#if UNITY_EDITOR
        action("");
        closeaction("");
        return;
#endif
        int noad = PlayerPrefs.GetInt("noad", 0);

        if (noad == 1)
        {
            action("");
            closeaction("");
            return;
        }

        if (TGSDK.CouldShowAd(ADSCENEID_REVIVE))
        {
            TGSDK.ShowAd(ADSCENEID_REVIVE);
        }
        TGSDK.PreloadAd();
        isAdLoaded = false;
        TGSDK.AdRewardSuccessCallback = action;
        TGSDK.AdCloseCallback = closeaction;
    }
    public void PlayGameOverAD()
    {

        int noad = PlayerPrefs.GetInt("noad", 0);

        if(noad == 1){
            return;
        }
        if (TGSDK.CouldShowAd(ADSCENEID_GAMEOVER))
        {
            TGSDK.ShowAd(ADSCENEID_GAMEOVER);
        }
        TGSDK.PreloadAd();
        isAdLoaded = false;
    }

}
