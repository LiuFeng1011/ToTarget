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
        Transform menu = GameObject.Find("UI Root").transform.Find("Menu");
        GameObject startBtn = menu.Find("StartGame").gameObject;
        UIEventListener.Get(startBtn).onClick = StartCB;

        yesObj = menu.Find("Yes").gameObject;

        GameObject modelList = menu.Find("ModelList").gameObject;
        GameObject modelScoresList = menu.Find("ModelScoresList").gameObject;
        int selmodel = PlayerPrefs.GetInt(GameConst.USERDATANAME_MODEL,0);

        for (int i = 0; i < GameConst.gameModels.Length; i ++){
            
            GameObject modelObj = NGUITools.AddChild(modelList, Resources.Load("Prefabs/UI/GameModel") as GameObject);
            modelObj.transform.localPosition = new Vector3(0, - 80 * i,0);
            UIEventListener.Get(modelObj).onClick = SelModel;

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
    }

    private void OnDestroy()
    {
        instance = null;
    }
}
