using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameGround : InGameBaseObj {

    Material m;
    int lastposx, nowposx , basedis;
    // Use this for initialization
    void Start()
    {
        basedis = Random.Range(0, 100);
        nowposx = (int)transform.position.x +basedis ;
        lastposx = nowposx;

        m = gameObject.GetComponent<Renderer>().sharedMaterial;
        SetColor(lastposx);
    }

    // Update is called once per frame
    void Update()
    {
        nowposx = (int)(transform.position.x +basedis);
        if(lastposx != nowposx){
            lastposx = nowposx;

            SetColor(lastposx);
        }
    }

    public void SetColor(float lastposx){

        float h, s, v;
        h = ((float)lastposx) / 100f;
        s = 0.4f;
        v = 0.7f;
        m.color = Color.HSVToRGB(h - (int)h, s, v);
    }
}
