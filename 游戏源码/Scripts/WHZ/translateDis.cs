using UnityEngine;
using System.Collections;

public class translateDis : MonoBehaviour {

    public void translateCam(float distance)
    {
       
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKeyDown(KeyCode.T))
        this.transform.position = new Vector3(this.transform.position.x +10, this.transform.position.y, this.transform.position.z);
	}
}
