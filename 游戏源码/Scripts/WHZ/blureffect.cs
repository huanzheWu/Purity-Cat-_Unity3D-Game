using UnityEngine;
using System.Collections;
//该脚本用于主角在加速/道具弹跳情况下的残影效果
//负责实例化残影图片
//作者：巫焕哲
public class blureffect : MonoBehaviour
{

    public GameObject[] sprite_player;  //主角的四帧动画
    public float genDel;       //产生动画的时间间隔
    public float genTime;

    private float timer;
    private float delTimer;



    // Use this for initialization
    void Start()
    {
        genTime = 1000;
        delTimer = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timer += Time.deltaTime;
        //将在指定时间长度内产生残影
        if (timer < genTime)
        {
            for (int i = 0; i < sprite_player.Length; i++)
            {
                delTimer += Time.deltaTime;
                if (delTimer > 0.3)
                {
                    delTimer = 0;
                    //产生一帧图片
                    Instantiate(sprite_player[i], transform.position, Quaternion.identity);
                }
            }
        }
    }
}