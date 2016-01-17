using UnityEngine;
using System.Collections;

public class TimeColdTest : MonoBehaviour {

    UISprite timeCold;

	// Use this for initialization
	void Start () {
        timeCold = this.GetComponent<UISprite>();
	}
	
	// Update is called once per frame
	void Update () {
        if (timeCold.fillAmount <= 0)
            timeCold.fillAmount = 1;
        else
            timeCold.fillAmount -= Time.deltaTime;
	}
}
