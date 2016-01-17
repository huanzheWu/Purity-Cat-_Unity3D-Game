using UnityEngine;
using System.Collections;

public class showRangeNum : MonoBehaviour {

    private countNumFastThanPlayer script;
    public int old_count;

	// Use this for initialization
	void Start () {
        script = GameObject.Find("Hazes").GetComponent<countNumFastThanPlayer>();
        old_count = script.count+1;
	}
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(script.count + "****************************************");
        GameObject child = transform.GetChild(script.count-1).gameObject;
        if(script.count!=old_count)
        {
            child.SetActive(true);
            GameObject old_child = transform.GetChild(old_count-1).gameObject;
            old_child.SetActive(false);
            old_count = script.count;
        }
	}
}
