using UnityEngine;
using System.Collections;

public class ColliderDead: MonoBehaviour {
    
    private PlayerControl player; //主角脚本

	void Start () {
        player = GameObject.Find("Player").GetComponent<PlayerControl>();       //获取主角脚本
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        //与主角发生了碰撞
        if (other.tag.CompareTo("Player") == 0)
        {
            player.m_isDead = true; //主角死亡
        }
    }

}
