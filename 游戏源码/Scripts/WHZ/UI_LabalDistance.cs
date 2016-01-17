using UnityEngine;
using System.Collections;

/// <summary>
/// 
/// 脚本说明：该脚本用于游戏关卡中显示主角奔跑距离数值
/// 时间：7/24
/// 作者：巫焕哲
/// </summary>


public class UI_LabalDistance : MonoBehaviour {

    private TrackBar trackBar;          //进度条
    private float playerRunDistance;    //主角已跑距离
    private float scaleFactor;          //距离缩放系数
    private UILabel lable;              //显示距离UI组件
    private PlayerControl player;
	void Start () {
        //初始化
        trackBar = GameObject.Find("Player_slider").GetComponent<TrackBar>();

        player = GameObject.Find("Player").GetComponent<PlayerControl>();

        lable = this.GetComponent<UILabel>();

        playerRunDistance = trackBar.lengthHasBeenRun;

        scaleFactor = 1;
    }
	
	void Update () {

        if(player.m_isFinish!=true)
        {
            playerRunDistance = trackBar.lengthHasBeenRun;
            lable.text = (int)playerRunDistance * scaleFactor + " ";
        }
     
	}
}
