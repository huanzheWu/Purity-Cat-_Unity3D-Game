using UnityEngine;
using System.Collections;

public class SkateBoard : MonoBehaviour {

    Transform playerTrans;
    Vector3 offSet = new Vector3(0, -0.5f,0);
	// Use this for initialization
	void Start () {
        playerTrans = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.position = playerTrans.position + offSet;
	}
}
