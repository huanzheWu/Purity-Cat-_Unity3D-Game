using UnityEngine;
using System.Collections;

public class teaches_right : MonoBehaviour {

    public GameObject teach_right;
    void OnTriggerEnter2D(Collider2D other)
    {
        //与主角发生了碰撞
        if (other.tag.CompareTo("Player") == 0)
        {
            int playTimes = PlayerPrefs.GetInt("PlayTimes", 0);
            if (playTimes == 0)
            {
                //激活
                teach_right.SetActive(true);
                Time.timeScale = 0;

            }
        }
    }
}
