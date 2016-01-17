using UnityEngine;
using System.Collections;

public class teaches_left : MonoBehaviour
{
    public GameObject teach_left;
    void OnTriggerEnter2D(Collider2D other)
    {
        //与主角发生了碰撞
        if (other.tag.CompareTo("Player") == 0)
        {
            int playTimes = PlayerPrefs.GetInt("PlayTimes", 0);
            if(playTimes==0)
            {
                PlayerPrefs.SetInt("PlayTimes", 0);
                //激活
                teach_left.SetActive(true);
                Time.timeScale = 0;
            }
        }
    }
}
