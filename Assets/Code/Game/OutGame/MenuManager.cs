using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour {
    static MenuManager instance;
    public static MenuManager GetInstance() { return instance; }

    GameObject yesObj;

    Dictionary<int,InGameUIBaseLayer> scoresList = new Dictionary<int,InGameUIBaseLayer>();

    private void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start () {

        AudioManager.Instance.PlayBG("sound/bgm");

        Transform menu = GameObject.Find("UI Root").transform.Find("Menu");
        GameObject startBtn = menu.Find("StartGame").gameObject;
        GameUIEventListener.Get(startBtn).onClick = StartCB;


        GameObject leaderBoardBtn = menu.Find("Anchor").Find("LeaderBoard").gameObject;
        GameUIEventListener.Get(leaderBoardBtn).onClick = ShowLB;


        GameObject noADBtn = menu.Find("Anchor").Find("NoAD").gameObject;
        GameUIEventListener.Get(noADBtn).onClick = NoADCB;

        int noad = PlayerPrefs.GetInt("noad", 0);

        if (noad == 1)
        {
            noADBtn.SetActive(false);
        }

        GameObject starBtn = menu.Find("Anchor").Find("Star").gameObject;
        GameUIEventListener.Get(starBtn).onClick = StarCB;

        yesObj = menu.Find("Yes").gameObject;

        GameObject modelList = menu.Find("ModelList").gameObject;
        GameObject modelScoresList = menu.Find("ModelScoresList").gameObject;
        int selmodel = PlayerPrefs.GetInt(GameConst.USERDATANAME_MODEL,0);

        for (int i = 0; i < GameConst.gameModels.Length; i ++){
            
            GameObject modelObj = NGUITools.AddChild(modelList, Resources.Load("Prefabs/UI/GameModel") as GameObject);
            modelObj.transform.localPosition = new Vector3(0, - 80 * i,0);
            GameUIEventListener.Get(modelObj).onClick = SelModel;

            modelObj.name = GameConst.gameModels[i].modelid + "";

            UILabel namelabel = modelObj.transform.Find("Label").GetComponent<UILabel>();
            namelabel.text = GameConst.gameModels[i].name;


            GameObject scoresObj = NGUITools.AddChild(modelScoresList, Resources.Load("Prefabs/UI/ModelScores") as GameObject);
            scoresObj.transform.localPosition = Vector3.zero;

            int basescores = PlayerPrefs.GetInt(GameConst.USERDATANAME_MODEL_MAXSCORES + GameConst.gameModels[i].modelid);
            int lastscores = PlayerPrefs.GetInt(GameConst.USERDATANAME_MODEL_LASTSCORES + GameConst.gameModels[i].modelid);

            UILabel bestScoresLabel = scoresObj.transform.Find("Best").Find("Label").GetComponent<UILabel>();
            bestScoresLabel.text = basescores + "";

            UILabel lastScoresLabel = scoresObj.transform.Find("Last").Find("Label").GetComponent<UILabel>();
            lastScoresLabel.text = lastscores + "";

            InGameUIBaseLayer baselayer = scoresObj.GetComponent<InGameUIBaseLayer>();
            scoresList.Add(GameConst.gameModels[i].modelid,baselayer);
            baselayer.Init();

            scoresObj.SetActive(false);
            if (selmodel == GameConst.gameModels[i].modelid)
            {
                yesObj.transform.position = new Vector3(yesObj.transform.position.x, modelObj.transform.position.y, 0);
                baselayer.Show();
            }


        }


	}

    void SelModel(GameObject obj){
        int selmodel = int.Parse(obj.name);

        PlayerPrefs.SetInt(GameConst.USERDATANAME_MODEL,selmodel);

        yesObj.transform.position = new Vector3(yesObj.transform.position.x, obj.transform.position.y, 0);

        foreach (KeyValuePair<int, InGameUIBaseLayer> kv in scoresList)
        {
            if(kv.Key == selmodel){
                kv.Value.Show();
            }else{
                kv.Value.Hide();
            }
        }

    }

	// Update is called once per frame
	void Update () {
        foreach (KeyValuePair<int, InGameUIBaseLayer> kv in scoresList)
        {
            kv.Value.ActionUpdate();
        }
	}
    void StartCB(GameObject go)
    {
        (new EventChangeScene(GameSceneManager.SceneTag.Game)).Send();
        Debug.Log(go.name);
    }

    void ShowLB(GameObject go){
        GameCenterManager.GetInstance().Showlb();
    }

    void NoADCB(GameObject go){
        PurchaseManager.GetInstance().DoIapPurchase(DoIapCB);
    }

    void DoIapCB(bool b,string s){
        
    }

    void StarCB(GameObject go){
        #if UNITY_IPHONE || UNITY_EDITOR
        const string APP_ID = "1369028254";
        var url = string.Format(
            "itms-apps://ax.itunes.apple.com/WebObjects/MZStore.woa/wa/viewContentsUserReviews?type=Purple+Software&id={0}",
            APP_ID);
        Application.OpenURL(url);
        #endif
    }

    private void OnDestroy()
    {
        instance = null;
    }
}
