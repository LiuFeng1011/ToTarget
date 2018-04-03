using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectCtrl : MonoBehaviour {
    float time = 2.0f;
	// Use this for initialization
	void Start () {
        ParticleSystem sys = transform.GetComponent<ParticleSystem>();
        time = sys.duration;
	}
	
	// Update is called once per frame
	void Update () {
        time -= Time.deltaTime;
        if(time <= 0){
            Destroy(gameObject);
        }
	}
}
