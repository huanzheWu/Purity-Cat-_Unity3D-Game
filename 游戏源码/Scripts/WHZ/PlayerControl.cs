using UnityEngine;
using System.Collections;
/// <summary>
/// 主角逻辑脚本
/// 巫焕哲
/// </summary>
public enum PlayerState
{
    Run, Hitted, Walk
}
public class PlayerControl : MonoBehaviour
{


    private Transform playerTransform;      //主角的Transform
    public GameObject cordEffect;           //加速奔跑时的效果
    private Transform haze;                 //被绳子攻击的雾霾怪的Transform

    public float m_runSpeed;                //主角奔跑速度
    private float constRunSpeed;            //主角默认的奔跑速度（常量）
    public float m_jumpHeight;              //主角跳跃的高度
    public bool m_isHinder;                //主角是否被游戏场景绊到
    public bool m_isDead;                   //主角是否已经死亡：死亡的状态有两种：掉落陷阱或者超出摄像机
    public bool m_isFinish;                 //主角是否已经到达终点
    private int jumpCount;                  //表示主角跳跃的次数
    private bool isGround;                  //表示主角是否在地面
    private bool isUseCord;                 // 标志是否使用了绳子道具                 
    private bool isInsitCoreEffect;         //是否实例了一个加速光环的效果
    private background_loop far_background; //远景脚本
    private background_loop near_background;//近景脚本
    private finshGame script_finshGame;
    private Vector2 defaultVelocity_Lgx;
    private GameObject glue_Lgx = null;
    private PlayerState playerState = PlayerState.Walk;
    private float verSpeed = 0;


    //广鑫添加的变量2015/9/22
    Vector2 defaulVelo;
    void Start()
    {
        script_finshGame = Camera.main.GetComponent<finshGame>();
        far_background = GameObject.Find("backGround_far").GetComponent<background_loop>();
        near_background = GameObject.Find("backGround_near").GetComponent<background_loop>();
        //广鑫添加的函数调用2015/9/22
        CountAddVeloScale_Lgx();//加速时的速度变化倍数
        CountDefaultVelo_Lgx();//主角默认的速度
        //---------------------------------
        //初始化主角属性
        m_isHinder = false;
        m_isFinish = false;
        m_isDead = false;
        isGround = false;
        isUseCord = false;
        isInsitCoreEffect = false;

        m_runSpeed = defaultVelocity_Lgx.x;
        constRunSpeed = m_runSpeed;
        m_jumpHeight = 7;
        jumpCount = 0; //主角的跳跃次数
        playerTransform = this.transform; //获取主角的Transform
        Time.timeScale = 0;

        ///巫焕哲添加
        ///开启多点触控
        Input.multiTouchEnabled = true;//开启多点触碰

        
    }

    //计算默认的速度
    void CountDefaultVelo_Lgx()
    {
        defaultVelocity_Lgx = new Vector2(CityHoriVelo_Lgx(), CityVertiVelo_Lgx());
    }
    //计算默认的垂直速度
    float CityVertiVelo_Lgx()
    {   //最后的缩放系数参考了HazeControl脚本的取值
        float city = Application.loadedLevelName[0] - '0';
        float custPass = Application.loadedLevelName[2] - '0';
        return 3 + city * 1.6f + custPass * 0.4f;
    }
    //计算默认的水平速度
    float CityHoriVelo_Lgx()
    {
        float city = Application.loadedLevelName[0] - '0';
        float custPass = Application.loadedLevelName[2] - '0';
        return 3.14f + custPass * 0.36f;
    }
    void FixedUpdate()
    {
        int numOfPlayerBeat = script_finshGame.beat;

        //主角按一定的速度移动
        transform.Translate(Time.deltaTime * m_runSpeed, Time.deltaTime * verSpeed, 0);
        if (this.transform.position.y > -0.45 && (playerState == PlayerState.Hitted))
            this.transform.position = new Vector3(this.transform.position.x, -0.45f, 0);

        //如果触碰的位置为屏幕的左半屏

        if (jumpCount < 2 )
        {
            //检查每点触屏
            for (int i = 0; i < Input.touches.Length; i++)
            {
                Touch touch = Input.touches[i];

               //如果触屏点在屏幕的左半屏
                if(touch.position.x <= Screen.width / 2.0f)
                {
                    //计算跳跃次数
                    jumpCount++;
                    //使主角获得向上速度，跳跃
                    Vector2 velocity = this.GetComponent<Rigidbody2D>().velocity;
                    velocity.y = m_jumpHeight;
                    this.GetComponent<Rigidbody2D>().velocity = velocity;
                }
            }
        }

        //--------------------------------------
        if (Input.GetKeyDown(KeyCode.A))
        {
            //计算跳跃次数
            jumpCount++;
            //使主角获得向上速度，跳跃
            Vector2 velocity = this.GetComponent<Rigidbody2D>().velocity;
            velocity.y = m_jumpHeight;
            this.GetComponent<Rigidbody2D>().velocity = velocity;    
        }

        //----------------------------------------

     
        if ( m_isDead == true/*||(m_isFinish ==true&& numOfPlayerBeat ==0)*/)
        {
            Debug.Log("主角死亡");
            //主角死亡，这里应该进行UI处理
            Time.timeScale = 0;
        }   
        if (m_isFinish == true)
        {
            //主角到达终点，进行其他处理
            far_background.speed = new Vector2(0, 0);
        }
     

        //如果当前正在使用绳子道具
        if (isUseCord == true)
            PropCordEffect(); //调用道具效果函数

    }


    //碰撞函数，用于设置主角是否在地面上
    public void OnCollisionEnter2D()
    {
        isGround = true;
        jumpCount = 0;
    }
    public void OnCollisionExit2D()
    {
        isGround = false;
    }

    //
   


    /*
     * 函数名：TellHazePosi()
     * 函数功能：提供给广鑫调用，告诉主角脚本当前NPC的Transform位置
     * 以及通知主角脚本进入绳子道具使用模式
     * 时间:7/23
     */

    public void TellHazePosi(Transform Haze)
    {
        haze = Haze;
        isUseCord = true;
    }

    /*
     * 函数名：PropCordEffect()
     * 函数功能：使主角进入快速追击模式，同时实例化光环
     * 时间:7/23
     */

    private void PropCordEffect()
    {
        //如果还没有实例化效果
        if (isInsitCoreEffect == false)
        {
            //实例化
            //cordEffect = (GameObject)Instantiate(cordEffect, transform.position, Quaternion.identity);
            //cordEffect.transform.parent = playerTransform;
            //加速
            m_runSpeed = constRunSpeed + 2;
            //改变标记
            isInsitCoreEffect = true;
        }

        //如果已经追上了NPC
        if (playerTransform.position.x - haze.transform.position.x >= 1.0f)
        {
            //速度返回常态
            m_runSpeed = constRunSpeed;
            //销毁效果
            Destroy(cordEffect);
            //改变标记
            isUseCord = false;
            isInsitCoreEffect = false;
        }
    }
    //0个引用是因为使用了字符串调用了这个协程函数
    float addSpeedTime_Lgx = -1;
    public GameObject m_SkateBoard;
    //广鑫2015/9/22
    //计算出来的速度变化倍数用于主角提速时的速度变化
    float veloScale;
    void CountAddVeloScale_Lgx()
    {
        float city = Application.loadedLevelName[0] - '0';
        float custPass = Application.loadedLevelName[2] - '0';
        veloScale = 1 + city * 1 + custPass * 0.25f;
    }
    IEnumerator AddSpeed()
    {
        GameObject temp = (GameObject)GameObject.Instantiate(m_SkateBoard);
        float maxSpeed;
        float custPass = Application.loadedLevelName[2] - '0';
        maxSpeed = 4 + 0.7f * custPass;
        do
        {

            m_runSpeed += Time.deltaTime * veloScale;
            if (m_runSpeed > maxSpeed)
                m_runSpeed = maxSpeed;
            addSpeedTime_Lgx -= Time.deltaTime;
            yield return null;
        } while (addSpeedTime_Lgx > 0);
        m_runSpeed = defaultVelocity_Lgx.x;
        if (addSpeedTime_Lgx == -1000)
            playerState = PlayerState.Hitted;
        else
            playerState = PlayerState.Walk;
        Destroy(temp);//销毁滑板
        this.GetComponent<blureffect>().enabled = false;
        this.GetComponent<Animator>().enabled = true;
        skateBoardObj = null;
    }
    public void AskSkateBoard()
    {
        playerState = PlayerState.Run;
        this.GetComponent<blureffect>().enabled = true;
        this.GetComponent<Animator>().enabled = false;
        if (addSpeedTime_Lgx > 0)//还在加速，直接设置加速时间
            addSpeedTime_Lgx = 3;
        else
        {
            //当被胶水束缚时还需要将胶水的图案去除
            if (glue_Lgx != null)
            {
                StopCoroutine("GetGlueHitReal");
                Destroy(glue_Lgx);
                Rigidbody2D temp = this.GetComponent<Rigidbody2D>();
                temp.gravityScale = 2;
                verSpeed = 0;
                Debug.Log("去除减速效果");
            }
            StopCoroutine("AddSpeed");
            addSpeedTime_Lgx = 3;
            StartCoroutine("AddSpeed");
        }
    }
    /*********************
     * 是否被胶水伤害
     * para为真，被胶水减速
     * para为假，恢复原来的速度
     * ******************/
    IEnumerator GetGlueHitReal()
    {
        playerState = PlayerState.Hitted;
        addSpeedTime_Lgx = -1000;//假如有加速状态，还需要取消
        if (skateBoardObj != null)
        {
            this.GetComponent<blureffect>().enabled = false;
            this.GetComponent<Animator>().enabled = true;
            Destroy(skateBoardObj);
        } skateBoardObj = null;
        yield return null;
        m_runSpeed = 1;
        verSpeed = 1;
        //被气泡攻击
        far_background.speed = new Vector2(0, 0);
    }
    bool isHitGlue_Lgx = false;
    float gravityScale_lgx;
    public void GetGlueHit(bool para)
    {
        Rigidbody2D temp = this.GetComponent<Rigidbody2D>();
        if (para == true && isHitGlue_Lgx == false)
        {
            temp.gravityScale = 0;
            verSpeed = 3;
            StartCoroutine("GetGlueHitReal");
        }
        else if (para == false)//气泡的效果消除
        {
            m_runSpeed = defaultVelocity_Lgx.x;
            playerState = PlayerState.Walk;
            Debug.Log("气泡消失，walk");
            temp.gravityScale = 2;
            verSpeed = 0;
            far_background.speed = new Vector2(2, 2);
        }
    }
    public void SetGlue(Object para)
    {
        glue_Lgx = (GameObject)para;
        Debug.Log(glue_Lgx);
    }
    public PlayerState GetState()
    {
        return playerState;
    }
    public void AddCellTime()
    {
        if (glue_Lgx != null)
            glue_Lgx.GetComponent<Cell>().AddKeepTime();
    }
    public GameObject skateBoardObj;
    public bool isProtect = false;
}
