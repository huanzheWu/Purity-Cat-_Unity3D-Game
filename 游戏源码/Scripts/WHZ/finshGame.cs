using UnityEngine;
using System.Collections;

public class finshGame : MonoBehaviour
{
    //主角脚本
    private PlayerControl player;
    public GameObject[] stars;
    public GameObject finishGameUI;
    public GameObject faileGame;
    public float waitTime;
    public int beat;        //主角打败的敌人数目

    private float timer;
    private float timerOfstar1;
    private float timeOfstart2;
    private float timeOfstar3;

    public GameObject button_next;

    //保存3个得分等级对应的名次
    public int[] rank = new int[3];

    //获取主角的排名的脚本
    private showRangeNum script_PlayerRank;

    //XML对象
    private XMLManager xml = new XMLManager();

    bool isfinish;
    void Start()
    {
        beat = 0;

        timer = 0;
        //获取游戏主角的脚本
        player = GameObject.Find("Player").GetComponent<PlayerControl>();

        timerOfstar1 = timeOfstart2 = timeOfstar3 = 0.0f;

        script_PlayerRank = GameObject.Find("rangeNumbers").GetComponent<showRangeNum>();

        button_next.GetComponent<UIButton>().enabled = false;

        isfinish = true;
    }
    void Update()
    {

        //如果完成关卡
        if (player.m_isFinish == true)
        {
            //获得主角当前的排名
            int playerRank = script_PlayerRank.old_count;
            //计算主角打败敌人的数量
            beat = rank[rank.Length - 1] - playerRank + 1;
            if(beat==0)
            {
                timer += 0.02f;
                if (timer >= 2.0f)
                {
                    faileGame.SetActive(true);
                }
            }
            else
            {
                timer += Time.deltaTime;
                if (timer >= 2.0f)
                {
                    //激活胜利面板
                    finishGameUI.SetActive(true);

                    timerOfstar1 += Time.deltaTime;

                    //根据排名来进行
                    if (timerOfstar1 >= 1.0f && rank[0] <= beat)
                    {
                        stars[0].SetActive(true);
                        timeOfstart2 += Time.deltaTime;
                        xml.ExchangePassLevelXML(Application.loadedLevelName, 1);///设置该关卡的星星数量
                    }
                    if (timeOfstart2 >= 1.0f && rank[1] <= beat)
                    {
                        stars[1].SetActive(true);
                        timeOfstar3 += Time.deltaTime;
                        xml.ExchangePassLevelXML(Application.loadedLevelName, 2);///设置该关卡的星星数量
                    }
                    if (timeOfstar3 >= 1.0f && rank[2] <= beat)
                    {
                        stars[2].SetActive(true);
                        xml.ExchangePassLevelXML(Application.loadedLevelName, 3);///设置该关卡的星星数量
                    }
                    if (timer > 6.0f && isfinish == true)
                    {
                        isfinish = false;
                        button_next.GetComponent<UIButton>().enabled = true;

                        int HazeHaveBeenBeat = 0;
                        //获取打败敌人的数目
                       HazeHaveBeenBeat= PlayerPrefs.GetInt("Enemy", HazeHaveBeenBeat);
                        //累加打败敌人数目
                        HazeHaveBeenBeat += beat;
                        //将累加成果写入XML中
                        PlayerPrefs.SetInt("Enemy", HazeHaveBeenBeat);
                    }
                }
            }
           
        }

        //如果主角死亡
        else if (player.m_isDead == true)
        {
            timer += 0.02f;
            if (timer >= 2.0f)
            {
                faileGame.SetActive(true);
            }
        }
    }
}
