using UnityEngine;
using System.Collections;

/************************************
 * 赋值对象：道具UI，PropUI
 * 功能：  1.管理道具的UI界面，包括持有什么道具，以及绳子的冷却
 *         2.管理持有道具：包括道具的数量限制，持有道具的释放，所有道具通过实例化来实现
 * ********************************/
public class PropUI : MonoBehaviour {

    struct SkillCold                    //用于定义道具UI中的CD
    {
        public UISprite bg;
        public UISprite timeSprite;
    }
    public Object m_prefabWatermelen;   //西瓜的预设，用于释放道具时实例化一个西瓜
    public GameObject m_prefabRope;     //绳子的预设，用于释放道具时实例化一根绳子
    Transform transPlayer;              //主角的transform脚本
    PlayerControl scriPlayerCon;        //玩家的控制脚本
    GameObject objectPlayer;            //玩家的游戏对象
    public UISprite m_leftProp;         //道具UI的左道具栏
    public UISprite m_rightProp;        //道具UI的右道具栏
    public GameObject m_ObjSkilCold;    //绳子的CD对象
    public GameObject m_Skelon;
    public GameObject m_Protected;
    SkillCold skillCold;                //持有绳子CD的背景与CD的前置背景
    GameObject []hazes;                 //持有场上所有的雾霾怪，假如雾霾怪到达了终点，这个数组会被更新
    const float maxAttaDis = 5;         //绳子的最大攻击距离
    bool needUpdateHazes;               //是否需要更新雾霾怪数组的标志
    bool isCatSucc;                     //是否成功抓住了雾霾的标志
    bool isJumpByRope;                  //是否通过绳子正在进行跳跃
    GameObject nowRope;
    Rigidbody2D rigidPlayer;

    ///巫焕哲添加 
    public Transform CamTra;            //摄像机脚本
    /// 
    public int touchOneSign;

	void Start () {
        touchOneSign = 0;
        ///巫焕哲添加
        CamTra = GameObject.Find("Main Camera").transform;
        ///
        Debug.Log("KKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKKK" + CamTra);

        objectPlayer = (GameObject)GameObject.FindGameObjectWithTag("Player");
        scriPlayerCon = objectPlayer.GetComponent<PlayerControl>();
        transPlayer = objectPlayer.GetComponent<Transform>();
        rigidPlayer = objectPlayer.GetComponent<Rigidbody2D>();
        needUpdateHazes = true;
        isCatSucc = false;
        isJumpByRope = false;
        StartCoroutine(FindHazes());
        m_ObjSkilCold.SetActive(false);
	}

    /*******************************
     * propName：玩家捡到的道具的名称。对应NGUI图库里的图
     * 操作结果：向道具栏添加道具（实际上就是更换图片）
     * ****************************/
    void AddPropToUI(string propName)
    {
        if (m_rightProp.spriteName == "NULL")
            m_rightProp.spriteName = propName;
        else if (m_leftProp.spriteName == "NULL")
            m_leftProp.spriteName = propName;
    }
    /*************************************
     * para：设置“是否需要更新雾霾怪的标志”的bool值
     * 操作结果：假如para为真，表示需要更新雾霾怪数组
     * **********************************/
    public void SetNeedFindHazes(bool para)
    {
        if (para == true)
        {
            StopCoroutine("FindHazes");
            StartCoroutine("FindHazes");
        } 
        needUpdateHazes = para;
    }

    public IEnumerator FindHazes()
    {
        while (needUpdateHazes == false)
            yield return null;
        needUpdateHazes = false;
        hazes = GameObject.FindGameObjectsWithTag("Haze");
    }
    //用于外界添加道具
    public void AddProp(PropType paraType)
    {
        if (m_leftProp.spriteName == "NULL")
        {
            switch (paraType)
            { 
                case PropType.Watermelon:
                    AddPropToUI("Watermelon");
                    break;
                case PropType.SkateBoard:
                    AddPropToUI("SkateBoard");
                    break;
                case PropType.Rope:
                    AddPropToUI("Rope");
                    break;
                case PropType.Skelon:
                    AddPropToUI("Skelon");
                    break;
                case PropType.Protected:
                    AddPropToUI("Protected");
                    break;
            }
        }
    }
    //
    public void ArrivedHaze()
    {
        StopCoroutine("FinishRope");
        StartCoroutine("FinishRope");
    }
    //主角已经抓捕到NPC并且到达NPC身边时调用
    IEnumerator FinishRope()
    {
        m_ObjSkilCold.SetActive(true);
        skillCold.bg = m_ObjSkilCold.GetComponent<UISprite>();
        skillCold.timeSprite = m_ObjSkilCold.transform.FindChild("TimeSprite").GetComponent<UISprite>();
        isJumpByRope = false;
        float timeCount = 2;
        skillCold.timeSprite.fillAmount = 1;
        while (skillCold.timeSprite.fillAmount > 0)
        {
            yield return null;
            skillCold.timeSprite.fillAmount -= Time.deltaTime / timeCount;
        }
        isCatSucc = false;
        m_ObjSkilCold.SetActive(false);
    }
    //实例并初始化一根绳子
    void InstanRope()
    {
        nowRope = GameObject.Instantiate(m_prefabRope);
        Rope tempScriRope = nowRope.GetComponent<Rope>();
        //初始化一根绳子并获取最近的NPC
        Transform tempNear = XNearestHazeTrans();
        if (tempNear != null)
        {
            isJumpByRope = true;
            isCatSucc = true;
            tempScriRope.InitRope(transPlayer, tempNear,CamTra);//提供玩家的位置与NPC的位置给绳子
        }
        else
        {
            isJumpByRope = false;
            isCatSucc = false;
			//tempScriRope.InitRope(transPlayer,tempNear);//提供玩家的位置与NPC的位置给绳子
        }
    }
    public void ReleaseProp()
    {
        if (isJumpByRope != true && (scriPlayerCon.GetState() != PlayerState.Hitted))//正在跳跃不允许释放新的道具
        {
            if (isCatSucc == true)  //跳跃刚完成，还是释放绳子
                InstanRope();
            else
            {
                GameObject temp;
                switch (m_rightProp.spriteName)
                {
                    case "Watermelon":
                        temp = (GameObject)GameObject.Instantiate(m_prefabWatermelen,transPlayer.position,Quaternion.identity);
                        temp.GetComponent<Watermelon>().SetInit(rigidPlayer.velocity);//设置西瓜的初始速度
                        temp.name = "Player";
                        break;
                    case "SkateBoard":
                        scriPlayerCon.AskSkateBoard();//成功调用
                        break;
                    case "Rope":
                        InstanRope();
                        break;
                    case "Skelon":
                        temp = (GameObject)GameObject.Instantiate(m_Skelon);
                        temp.GetComponent<Skeleton>().Init(transPlayer.position);
                        break;
                    case "Protected":
                        temp = (GameObject)GameObject.Instantiate(m_Protected);
                        temp.GetComponent<Protected>().Init();
                        break;
                }
                m_rightProp.spriteName = m_leftProp.spriteName;
                m_leftProp.spriteName = "NULL";
            }
        }
    }
    void InputControl()
    {

        //巫焕哲修改，改为移动设备端触屏操作
        for (int i = 0; i < Input.touches.Length; i++)
        {
            Touch touch = Input.touches[i];
            if (touch.position.x >= Screen.width / 2.0f && touchOneSign==0)
            {
                touchOneSign++;
                ReleaseProp();
            }
            if(Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                touchOneSign = 0;
            }
        }
     
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ReleaseProp();
        }

        ////巫焕哲修改，改为移动设备端触屏操作
        //for (int i = 0; i < Input.touches.Length; i++)
        //{
        //    Touch touch = Input.touches[i];
        //    if (touch.position.x >= Screen.width / 2.0f)
        //    {

        //        ReleaseProp();
        //    }
        //}
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    ReleaseProp();
        //}
    }

    //获取距离玩家最近并且在攻击范围内的NPC，假如没有，返回null
    Transform XNearestHazeTrans()
    {
        Transform near = null;
        float dis = maxAttaDis;
        foreach(GameObject inHazes in hazes)
        {
            if (inHazes != null)
            {
                Vector3 posiHaze = inHazes.transform.position;
                Vector3 posiPlayer = transPlayer.position;
                if (posiHaze.x > (posiPlayer.x + 0.01))
                    if (dis > (inHazes.transform.position - transPlayer.position).magnitude)
                        if ((near == null) || (near.position.x > posiHaze.x))
                        {
                            near = inHazes.transform;
                            dis = (inHazes.transform.position - transPlayer.position).magnitude;
                        }
            }
        }
        return near;
    }

	void Update () {
        InputControl();
	}
    public float GetAttDis()
    {
        return maxAttaDis;
    }
	//------------------------------------
	//初始化NPC向前奔跑
    //用于开始游戏按钮的调用
	//------------------------------------
	public void InitHaze()
	{
		foreach(GameObject haze in hazes)
		{
            if (haze != null)
            {
                haze.GetComponent<HazeControl>().InitVelo();
            }
		}
	}
}
