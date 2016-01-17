using UnityEngine;
using System.Collections;

public class HazeControl : MonoBehaviour
{

    enum HazeStateType
    {
        HitByWatermelon, Run, Jump
    }


    Rigidbody2D rigidHaze;
    float mulCity;           //每个城市包含了4个关卡的变化，2.0相当于放大了一倍
    float mulCustPass;      //RandomVeloMul中设定
    float mulJumpCity = 1.6f;
    float mulJumpCustPass = 0.4f;
    Vector2 minVelo = new Vector2(2.0f, 2.0f);
    Vector2 defaVelocity;
    HazeStateType hazeState = HazeStateType.Run;
    float hitTimeCount;
    const float hitTime = 3.0f;
    float attaTimeCount;
    const float attaTime = 5.0f;
    Animator anim;
    PropUI scriPropUI;
    public GameObject m_PreMark;
    GameObject objectMark;

    public Object m_PropHaze;
    Vector3 offset = new Vector3(0.2f, 0.5f, 0);
    int jumpNum;
    public GameObject NPCEffect;

    void Awake()
    {
        anim = this.GetComponent<Animator>();
        scriPropUI = GameObject.FindGameObjectWithTag("PropUI").GetComponent<PropUI>();
        rigidHaze = this.GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        jumpNum = 0;
        //-------感叹号
        objectMark = (GameObject)GameObject.Instantiate(m_PreMark, this.transform.position + offset, Quaternion.identity);
    }
    void RandomVeloMul(float city, float custPass)
    {
        mulCity = 1.5f;
        mulCustPass = 0.87f;
    }
    float CityVertiVelo(float city, float custPass)
    {
        return 3 + city * mulJumpCity + custPass * mulJumpCustPass;
    }
    float CityHoriVelo(float city, float custPass)
    {
        return 2.69f + 0.6f * custPass;
    }
    //在UI的开始按钮中调用
    //在UI的开始按钮中调用
    public void InitVelo()
    {
        if (anim == null)
            anim = this.GetComponent<Animator>();
        if (scriPropUI == null)
            scriPropUI = GameObject.FindGameObjectWithTag("PropUI").GetComponent<PropUI>();
        if (rigidHaze == null)
            rigidHaze = this.GetComponent<Rigidbody2D>();
        anim.SetBool("isBegin", true);
        StartCoroutine("Attack");
        Destroy(objectMark);
        rigidHaze = this.GetComponent<Rigidbody2D>();
        float city = Application.loadedLevelName[0] - '0';
        float custPass = Application.loadedLevelName[2] - '0';
        RandomVeloMul(city, custPass);//随机NPC的速度
        defaVelocity = new Vector2(CityHoriVelo(city, custPass), CityVertiVelo(city, custPass));
        rigidHaze.velocity = new Vector2(defaVelocity.x, 0);
        Debug.Log(defaVelocity + "46541561651651");
    }
    //在这个地方不断产生敌人的道具
    IEnumerator Attack()
    {
        float minTime, maxTime;
        float city = Application.loadedLevelName[0] - '0';
        float custPass = Application.loadedLevelName[2] - '0';
        minTime = 12 - custPass * 1f;
        maxTime = minTime + 12 -  custPass * 0.5f;
        attaTimeCount = Random.Range(minTime,maxTime);
        while (true)
        {
            if (attaTimeCount <= 0)
            {
                GameObject temp = (GameObject)GameObject.Instantiate(m_PropHaze, this.transform.position, Quaternion.identity);
                temp.name = "HazeAttack";
                temp.GetComponent<Cell>().SetInit(rigidHaze.velocity);
                attaTimeCount = Random.Range(minTime,maxTime);
            }
            attaTimeCount -= Time.deltaTime;
            yield return null;
        }
    }


    //可以成功调用
    public void GetGlueHit(bool para)
    {
        if (para == true)
        {
            if (rigidHaze.velocity.y > 0.000001)
            {
                rigidHaze.velocity = minVelo;
            }
            else
            {   //包含在平地以及下降状态的速度变化
                Vector2 temp = rigidHaze.velocity;
                temp.x = minVelo.x;
                rigidHaze.velocity = temp;
            }
            hazeState = HazeStateType.HitByWatermelon;
        }
        else
        {
            Vector2 temp = rigidHaze.velocity;
            temp.x = defaVelocity.x;
            rigidHaze.velocity = temp;
            hazeState = HazeStateType.Run;
        }
    }
    void Jump()
    {
        if (jumpNum < 2)
        {
            if (hazeState == HazeStateType.Run)
                rigidHaze.velocity = new Vector2(defaVelocity.x, defaVelocity.y);
            else
                rigidHaze.velocity = new Vector2(minVelo.x, minVelo.y);
            jumpNum++;
        }
    }
    bool isEndGame = false;
    void OnCollisionEnter2D(Collision2D col)
    {
        if (isEndGame == false)
        {
            GameObject.Instantiate(NPCEffect, this.transform.position, Quaternion.identity);
            if (col.collider.tag == "Ground" || col.collider.tag == "Platform")
            {
                jumpNum = 0;
                if (hazeState == HazeStateType.Run)
                    rigidHaze.velocity = new Vector2(defaVelocity.x, 0);
                else if (hazeState == HazeStateType.HitByWatermelon)
                    rigidHaze.velocity = new Vector2(minVelo.x, 0);
            }
        }
    }
    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "JumpPlace")
        {
            float jump = Random.Range(0.0f, 1.0f);
            if(jump < 0.5)
                Jump();
            
        }
        else if (col.tag == "EndOfGame")
        {
            scriPropUI.SetNeedFindHazes(true);
            //Destroy(this.gameObject);
        }
    }

    void NormalMove()
    {
        anim.SetFloat("vertiVelo", rigidHaze.velocity.y);
        //NPC的自动跳跃，当NPC下降时，进行跳跃
        //当NPC跳跃的时候撞上平台时，假如只跳跃了一次，那么很快就会落回地面
        //所以当NPC奔跑水平速度比较小的时候，需要不断地进行跳跃
        if (rigidHaze.velocity.x < 0.1)
        {
            if (hazeState == HazeStateType.Run)
            {
                if (rigidHaze.velocity.y <= 0.01)
                {
                    Jump();
                }
            }
        }
        //假如NPC没有进行跳跃，假如是奔跑的状态，时候奔跑的速度
        //NPC跳跃撞上了平台,水平速度回骤减，所以在这里保持一定的水平速度
        if (hazeState == HazeStateType.Run)
            rigidHaze.velocity = new Vector2(defaVelocity.x, rigidHaze.velocity.y);
        else if (hazeState == HazeStateType.HitByWatermelon)//NPC被胶水击中
            rigidHaze.velocity = new Vector2(minVelo.x, rigidHaze.velocity.y);  
    }
    void Update()
    {
        if (isTurnBack == false)
        {
            //Debug.Log(rigidHaze.velocity);
            //Debug.Log(defaVelocity + "adfadfasdf");
            NormalMove();
        }
        else
        {
            rigidHaze.velocity = new Vector2(-3f, 0);
        }
    }
    //-----------------------------
    //焕哲用到的接口
    //-------------------------------
    public GameObject m_endBlowObj;
    public void EndBlow(Vector3 endPosition)
    {
        GameObject temp = (GameObject)GameObject.Instantiate(m_endBlowObj);
        EndBlow endBlowScri = temp.GetComponent<EndBlow>();
        endBlowScri.Init(this.transform.position, endPosition);
        //Destroy(this.gameObject);
        this.GetComponent<SpriteRenderer>().enabled = false;
        //this.GetComponent<HazeControl>().enabled = false; 
    }
    public void EndGame()
    {
        StopCoroutine("Attack");
        isEndGame = true;
    }

    bool isTurnBack = false;
    public void IsSkelon(bool para)
    {
        isTurnBack = para;
        Vector3 scale = this.transform.localScale;
        scale.x = -scale.x;
        this.transform.localScale = scale;
        if(para == true)
        {
            rigidHaze.velocity = new Vector2(-3f, 0);
        }    
    }

}

