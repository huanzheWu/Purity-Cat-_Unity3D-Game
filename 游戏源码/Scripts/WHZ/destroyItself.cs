using UnityEngine;
using System.Collections;

public class destroyItself : MonoBehaviour {

    private float timer;


	// Use this for initialization
	void Start () {
        timer = 0;
	
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        if (timer >= 2f)
        {
            Destroy(this.gameObject);
            Debug.Log("销毁销毁~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
        }
	}
}
