using UnityEngine;
using System.Collections;

//该脚本用于控制背景图的循环
//时间：7/21

public class BG_Roller : MonoBehaviour {

      private Transform [] bgSprites= new Transform[2];              //获取两张相连的背景图片
      private const float switchDistanceCam = 10.07f - (-0.41f);        //摄像机移动的切换图片距离，每移动这个距离则切换图片
      private const float switchDistanceSpr = 9.59f*2;    //图片每次右移时应该移动的距离。
      private Transform camTransform;                                //获取摄像机的Transform
      private float camSourcePosition;                               //摄像机的初始位置（或者刚移动完图片那个时刻的摄像机位置）
      private int signOfSprite;                                      //标记接下来要移动那张图片
      void Start()
      {
          camTransform = this.transform;
          camSourcePosition = camTransform.position.x;
          signOfSprite = 0;
          bgSprites[0] = GameObject.Find("backGround1").transform;
          bgSprites[1] = GameObject.Find("backGround2").transform;   
      }
      void FixedUpdate()
      {
          float camCurrenPosition = camTransform.position.x;                 //获取摄像机当前位置的x变量
          if (camCurrenPosition - camSourcePosition > switchDistanceCam)     //若移动了切换距离的倍数
          {
              camSourcePosition = camCurrenPosition;
              //进行图片的切换
              if (signOfSprite % 2 == 0) //将图片1移动
                  bgSprites[0].transform.position = new Vector3(bgSprites[0].position.x + switchDistanceSpr, bgSprites[0].position.y, bgSprites[0].position.z);
              else    //将图片二移动
                  bgSprites[1].transform.position = new Vector3(bgSprites[1].position.x + switchDistanceSpr, bgSprites[1].position.y, bgSprites[1].position.z);
              signOfSprite++;
          }
      }
}

