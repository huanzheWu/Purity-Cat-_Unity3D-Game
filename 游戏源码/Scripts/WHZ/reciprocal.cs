using UnityEngine;
using System.Collections;

public class reciprocal : MonoBehaviour {

    private float timer;
    public GameObject num1;
    public GameObject num2;
    public GameObject num3;
    GameObject[] hazes;                 //持有场上所有的雾霾怪，假如雾霾怪到达了终点，这个数组会被更新
    void Start()
    {
        hazes = GameObject.FindGameObjectsWithTag("Haze");
        timer = 0;
    }
    void Update () {
        
        timer += 0.023f;
        if (timer > 0.0f && timer < 1.0f)
        {
            num1.SetActive(true);
        }
        else if (timer > 1.0f && timer < 2.0f)
        {
            num1.SetActive(false);
            num2.SetActive(true);
        }
        else if (timer > 2.0f && timer < 3.0f)
        {
            num2.SetActive(false);
            num3.SetActive(true);
        }
        else if(timer>3.0f)
        {
            num3.SetActive(false);
            //开始游戏
            Time.timeScale = 1;
            this.gameObject.SetActive(false);

            //激活怪物
		    foreach(GameObject haze in hazes)
		    {
                 if (haze != null)
                {
                  haze.GetComponent<HazeControl>().InitVelo();
                 }
		    }
	    }
        
	}
}
