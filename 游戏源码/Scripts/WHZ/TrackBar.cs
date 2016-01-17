using UnityEngine;
using System.Collections;

public class TrackBar : MonoBehaviour {

    private Transform startOfTrack; //跑道起始标志
    private Transform endOfTrack;   //跑道结束标志

    private float startPosition;    //跑道的起始位置
    public Transform character;       //玩家在跑道中

    private float trackLength;      //跑道长度
    private float barLength;        //进度条长度
    public float lengthHasBeenRun; //已经跑过的长度
    private UISlider slider;
    void Start()
    {
        //获取该关卡起始与结束位置标志的Transform组件
        startOfTrack = GameObject.Find("sprite_trackBegin").GetComponent<Transform>();
        endOfTrack = GameObject.Find("sprite_trackEnd").GetComponent<Transform>();
        slider = this.GetComponent<UISlider>();
        slider.value = 0.0f;
        //算出该关卡的跑道长度
        trackLength = endOfTrack.position.x-startOfTrack.position.x ;
        //保存该关卡的起始位置
        startPosition = startOfTrack.position.x;
    }

    void FixedUpdate()
    {
        lengthHasBeenRun = character.transform.position.x - startPosition;
        slider.value  = lengthHasBeenRun / trackLength;  //计算出主角当前在跑道中的位置比例
    }

}
