using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//多层背景图循环算法
//使用方法：将该脚本搭载在游戏物体OBJ上（可以是一个空物体）
//所有需要循环的背景图片，按顺序添加为OBJ的子物体
//本脚本自动为子物体创建链表，并执行图片循环
//speed用于设置循环的速度
//direction用于设置图片移动的方向
//2015.07

public class background_loop : MonoBehaviour
{
    public Vector2 speed = new Vector2(0 , 0);  	//图片的移动速度
    public Vector2 direction = new Vector2(-1, 0);	//图片的移动方向（-1向左运动，相对摄像机来说，其内容向右运动）
    public bool isLinkedToCamera = true;			//图片是否与摄像机关联
    public bool isLooping = true;					//循环播放？
	
    private List<Transform> backgroundPart;			//存放背景图的链表
	
    void Start()
    {
        if (isLooping)	
        {
            backgroundPart = new List<Transform>();
            for (int i = 0; i < transform.childCount; i++) 	//获取子物体，将其加入链表中
            {
                Transform child = transform.GetChild(i);
                if (child.GetComponent<Renderer>() != null)
                {
                    backgroundPart.Add(child);
                }
            }
            backgroundPart = backgroundPart.OrderBy(t => t.position.x).ToList();  //依据图片的相对位置进行排序
        }
    }
    void FixedUpdate()	//游戏循环
    {
        Vector3 movement = new Vector3(  //计算移动速度向量
         speed.x * direction.x,
        speed.y * direction.y,
          0);
        movement *= Time.deltaTime;    
        transform.Translate(movement); //以独立的速度进行移动
        if (isLooping)
        {
            Transform firstChild = backgroundPart.FirstOrDefault();
            if (firstChild != null)
            {
                if (firstChild.position.x < Camera.main.transform.position.x)  
                {
                    if (firstChild.GetComponent<Renderer>().IsVisibleFrom(Camera.main) == false) 
						//判断是否在摄像机的可视范围之内，如果已经超出可视范围，则将图片连接到最右端
                    {
                        Transform lastChild = backgroundPart.LastOrDefault();
                        Vector3 lastPosition = lastChild.transform.position;
                        Vector3 lastSize = (lastChild.GetComponent<Renderer>().bounds.max - lastChild.GetComponent<Renderer>().bounds.min);
                        firstChild.position = new Vector3(lastPosition.x + lastSize.x, firstChild.position.y, firstChild.position.z);
                        backgroundPart.Remove(firstChild);//删除原来位置的图片
                        backgroundPart.Add(firstChild); 	//在后面添加新的图片
                    }
                }
            }
        }
    }
}
