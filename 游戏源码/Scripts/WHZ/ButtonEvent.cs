using UnityEngine;
using System.Collections;
/// <summary>
/// 这个脚本集中了游戏关卡中的按钮事件
/// </summary>
public class ButtonEvent : MonoBehaviour
{
    private PlayerControl Splayer;
    private GameObject Gplayer;
    public GameObject teach_left;
    public GameObject teach_right;

    public GameObject pauseBackground;//这里获取暂停时出现的背景图游戏物体。
    public GameObject startBackground;//游戏刚开始时出现的背景图片
    public Object backgroundMusic;
    public GameObject countNUm;
    /// <summary>
    /// 进入游戏关卡后，点击游戏开始的start按钮
    /// </summary>
    void Start()
    {
        startBackground = GameObject.Find("startBackground");
     
        Gplayer = GameObject.Find("Player");
        Splayer = Gplayer.GetComponent<PlayerControl>();
    }

    public void Start1()
    {
        Instantiate(backgroundMusic, this.transform.position, Quaternion.identity);
        Debug.Log(this.transform.position + "++++++++++++++++++++++++++++++++++++++++++++++++++++++");


        //在这里让激活另一个脚本
        //在那个脚本里，倒数数字并设置timescale为1
        //游戏开始进行
        countNUm.SetActive(true);
        startBackground.SetActive(false);
    }

    /// <summary>
    /// 暂停按钮
    /// </summary>
    public void Pause()
    {

        Time.timeScale = 0;

        pauseBackground.SetActive(true);

    }
    /// <summary>
    /// 重新开始的按钮
    /// </summary>
    public void ReflashGame()
    {
        Time.timeScale = 1;
        pauseBackground.SetActive(false);
        Application.LoadLevel(Application.loadedLevelName);
        Debug.Log("重新开始游戏");
    }
    /// <summary>
    /// 暂停后开始游戏
    /// </summary>
    public void ReturnToGame()
    {
        Time.timeScale = 1;
        pauseBackground.SetActive(false);
        Debug.Log("继续游戏");
    }
    /// <summary>
    /// 返回主菜单
    /// </summary>
    public void ReturnToMenu()
    {
        Application.LoadLevel("ClothesSence");
    }
    public void nextPass()
    {
        char[] temp = Application.loadedLevelName.ToCharArray();
        temp[2] = (char)((int)temp[2] + 1);
        string next = "";
        next = next + temp[0] + temp[1] + temp[2] + "";
        string next_load = next.Substring(0, 3);
        Application.LoadLevel(next_load);
    }

    public void ReturnToMap()
    {

        PlayerPrefs.SetInt("City", (int)(Application.loadedLevelName[0] - '0'));
        Application.LoadLevel("MapSence");

    }

    public void ReturnToMap_2()
    {
        Application.LoadLevel("MapSence");
    }
    public void TeachLeft()
    {
       Debug.Log("点击~~~~~~~~~~~~~~~~~~~~~~");
       //使得猫跳一次
       Vector2 velocity = Gplayer.GetComponent<Rigidbody2D>().velocity;
       velocity.y = Splayer.m_jumpHeight;
       Gplayer.GetComponent<Rigidbody2D>().velocity = velocity;

       Time.timeScale = 1;
       teach_left.SetActive(false);                           
    }
    public void TeachRight()
    {

    
      Time.timeScale = 1;
      teach_right.SetActive(false);

      GameObject.Find("PropUI 1").GetComponent<PropUI>().ReleaseProp();
      PlayerPrefs.SetInt("PlayTimes", 1);
    
    }
}
