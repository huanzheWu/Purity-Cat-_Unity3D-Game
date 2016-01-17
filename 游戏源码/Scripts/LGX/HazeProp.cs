using UnityEngine;
using System.Collections;

/************************************
 * 未完全实现的脚本：雾霾怪的道具脚本
 * 原因：雾霾怪的道具未确定
 * *********************************/
public class HazeProp : MonoBehaviour {

    Vector2 velo = new Vector2(8.0f, 0);

    public void SetDefauVelo(float x)
    {
        velo.x += x;
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.Translate(velo.x * Time.deltaTime, 0, 0);
	}
    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            //Debug.Log("攻击到了玩家");
            //--------------------------------------这部分还没写
        }
        else if (col.tag == "EndOfGame")
        {
            Destroy(this.gameObject);
        }
    }
}
