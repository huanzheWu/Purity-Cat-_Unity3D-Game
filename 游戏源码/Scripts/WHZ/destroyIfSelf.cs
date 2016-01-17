using UnityEngine;
using System.Collections;
//该脚本用于主角在加速/道具弹跳情况下的残影效果上
//负责自动销毁残影图片
//以及控制图片的透明变换
//作者：巫焕哲
public class destroyIfSelf : MonoBehaviour {

    public float exitTime;              //每帧动画存在的时间
    private float blend;                 //alpha值变化的速度
    private float timer;
	// Use this for initialization
	void Start () {
        exitTime = 2;
        blend = 1;
        timer = 0;
	}
	
	// Update is called once per frame
    void Update()
    {
        
        timer += Time.deltaTime;
        if (timer > exitTime)
        {
            Destroy(this.gameObject); 
        }   
        //逐渐变透明
        blend -= Time.deltaTime;
        this.GetComponent<SpriteRenderer>().color = new Vector4(255, 255, 255,0.7f*blend);
	}
}
