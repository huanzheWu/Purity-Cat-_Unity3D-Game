using UnityEngine;
using System.Collections;
/**************************
 * 脚本主要用于绘制抛出的绳子
 * ***********************/
public class Rope : MonoBehaviour {

    PlayerControl playerControl;            //玩家的控制脚本
    Transform transPlayer;                  //玩家的transform脚本
    Transform transNearestHaze;             //距离玩家最近的，在攻击距离之内的雾霾怪的tranform脚本
    Vector3 endPosi;                        //绳子的终点
    Vector3 beginPosi;                      //绳子的起点
    LineRenderer lineRender;                //绘制绳子线条的工具
    float widthBegin = 0.1f;                //绳子在玩家一端的大小
    float widthEnd = 0.1f;                  //绳子在NPC一端的大小
    PropUI scriPropUI;                      //道具UI的脚本
	bool isInit = false;
    public  translateDis camtranslate;              //移动摄像机脚本
    public GameObject ropeEffect;
	void Start () {
	}
    /****************************************
     * paraTransPlayer 用于确立绳子的起始位置
     * paraTransHaze   用于确立绳子的结束位置
     * 操作结果：设置绳子的起始与终止位置，设置绳子的宽度
     ****************************************/
    //函数在初始化绳子的时候被调用

    //巫焕哲修改
    //添加一个参数 Transform CamTra,用于传入摄像机

    public void InitRope(Transform paraTransPlayer, Transform paraTransHaze,Transform CamTra)
    {
        lineRender = this.GetComponent<LineRenderer>();//实例化的时候start函数执行出错，所以在这里获取脚本
        scriPropUI = GameObject.FindGameObjectWithTag("PropUI").GetComponent<PropUI>();
        transPlayer = paraTransPlayer;
        playerControl = paraTransPlayer.GetComponent<PlayerControl>();
        transNearestHaze = paraTransHaze;//设置NPC的为位置
        SetPosi();//设置绘制绳子时的起始于终点位置
        lineRender.SetWidth(widthBegin, widthEnd);
		isInit = true;

        //巫焕哲添加
        //
        float cameraMovDis = endPosi.x - transPlayer.position.x;
        GameObject.Instantiate(ropeEffect, transPlayer.position, Quaternion.identity);
        transPlayer.position = endPosi;

        //巫焕哲添加
        //用于把摄像机移动一段距离

        CamTra.position = new Vector3(CamTra.position.x + cameraMovDis, CamTra.position.y, CamTra.position.z);
        Debug.Log("PPPPPPPPPPPPPPPPPPPPPPPPPPPPPPP" + cameraMovDis);
    }
    /*************************************
     * 操作结果：设定绳子的起始位置与终止位置
     * 协助InitRope函数 
     * ***********************************/
    void SetPosi()//----------------------------------------------------------------------------------------------------------------------------------------------
    {
        beginPosi = transPlayer.position;
        lineRender.SetPosition(0, transPlayer.position);
        if (transNearestHaze != null)
        {	//绳子通知主角可以移动的地方
            playerControl.TellHazePosi(transNearestHaze);
            endPosi = transNearestHaze.position;
        }
		else if(isInit == false)
		{
			endPosi = beginPosi + new Vector3(1, 0, 0);
		}
		lineRender.SetPosition(1, endPosi);
	}
	/*************************************
     * 操作结果：根据绳子初始位置与结束位置不断更新绳子的位置
     * 当玩家到达绳子的另外一端时，假如持有NPC，告诉道具UI，销毁绳子，设置冷却
     * 假如没有持有NPC，直接销毁绳子
     * **********************************/
	void Update () 
    {
		if (isInit == true) {
			SetPosi ();
		}
		if ((endPosi - beginPosi).magnitude < 1 || (endPosi.x <= beginPosi.x)) {
			if (transNearestHaze != null)
				scriPropUI.ArrivedHaze ();
			Destroy (this.gameObject);
		}
	}
}