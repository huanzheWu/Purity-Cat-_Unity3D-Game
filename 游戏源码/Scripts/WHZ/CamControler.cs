using UnityEngine;
using System.Collections;

//该脚本用于使主角始终位于屏幕中心。
//时间：7/22
//
//
//
public class CamControler : MonoBehaviour {

    public float m_camMoveSpeed;        //摄像机的移动速度
    private GameObject deadline;    //绑在摄像机后面的一个碰撞体，用于判断主角超出屏幕时的死亡判定
    private PlayerControl player;       //主角的脚本
    private Transform camTransform;     //本脚本的Transform
    private bool isPlayerOnCenter;
    private finshGame finshgame;
    private Rigidbody2D Playervelocity;

	void Start () {

        deadline = GameObject.Find("collider_outOfView_dead");
        player = GameObject.Find("Player").GetComponent<PlayerControl>(); //获取主角的脚本

        Playervelocity = player.GetComponent<Rigidbody2D>();

        finshgame = this.GetComponent<finshGame>();
        camTransform = this.transform;                                     //获取摄像机Transform                      //初始化摄像机速度
        isPlayerOnCenter = false;

	}
	void FixedUpdate () {

        this.GetComponent<Rigidbody2D>().velocity = Playervelocity.velocity;
        deadline.GetComponent<Rigidbody2D>().velocity = Playervelocity.velocity;

        if (player.m_isFinish == true)
        {
            m_camMoveSpeed = 0;
        }
        else if (isPlayerOnCenter == true)                   //如果主角位于摄像机的中心，则摄像机的移动速度与主角同步
            m_camMoveSpeed = player.m_runSpeed;
        else                                             //否则，摄像机移动慢于主角（等待主角追上来） 
            m_camMoveSpeed = player.m_runSpeed -0.5f;
        camTransform.Translate(Time.deltaTime * m_camMoveSpeed, 0, 0);
	}
     void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.CompareTo("Player") == 0)
        {
            isPlayerOnCenter = true;
        }
    }
     void OnTriggerExit2D(Collider2D other)
     {
         if (other.tag.CompareTo("Player") == 0)
         {
             isPlayerOnCenter = false;
         }
     }

}
