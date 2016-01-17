using UnityEngine;
using System.Collections;

public class absorbHaze : MonoBehaviour {
    private PlayerControl player;
    private GameObject air;
    bool isEnd = false;
	// Use this for initialization
	void Start () {
        player = GameObject.Find("Player").GetComponent<PlayerControl>();
        air = GameObject.Find("AIR");
	}
	
	// Update is called once per frame
	void Update () {
        //如果主角已经到达终点
        if (player.m_isFinish == true && isEnd == false)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                child.GetComponent<HazeControl>().EndBlow(air.transform.position);
                Debug.Log(air.transform.position);
                child.GetComponent<HazeControl>().EndGame();
                child.GetComponent<HazeControl>().enabled = false;
            }
            isEnd = true;
        }
	
	}
}
