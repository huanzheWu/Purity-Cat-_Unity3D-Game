using UnityEngine;
using System.Collections;
/// <summary>
/// 游戏UI总管理器
/// 作者：卫子麒
/// </summary>
public class GameManager : MonoBehaviour {

    //单例化
    public static GameManager Instance;

    //关卡混乱信息
    private ArrayList LevelArray;

    //XML管理器类变量
    private XMLManager XMLManager = new XMLManager();

    //音量大小和音量开关
    private float Sound = 0.0f;

    //总星数管理变量
    private int TotalStarNum;

    //初始化单例模式和初始化XML，读取声音大小和声音开关。
    void Awake()
    {
        if (!Instance)
            Instance = this;
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        XMLManager.InitAllXML();

        Sound = PlayerPrefs.GetFloat("Sound", 0.0f);

        if (LevelManager.Instance != null)
        {
            XMLManager.LoadXML("Level", XMLTYPE.Level, out LevelArray);
            LevelManager.Instance.GetImformation(LevelArray);
        }
    }

    //进行读取关卡、背包信息，并完成整理。
    void Start()
    {
        if (Application.loadedLevelName == "MapSence")
            LevelManager.Instance.SetCityPicture();

        if (ButtonClick.Instance != null)
        {
            if (Sound == 0)
                ButtonClick.Instance.MusicClose();
            else
                ButtonClick.Instance.MusicOpen();
        }

        if(Application.loadedLevelName == "ClothesSence")
            AchievementManager();
    }

    //设置设置界面的数据
    public void SetSettingData(float value)
    {
        Sound = value;
        PlayerPrefs.SetFloat("Sound", Sound);
    }

    //获取设置界面的数据
    public void GetSettingData(out float value)
    {
        value = Sound;
    }

    //成就管理
    private void AchievementManager()
    {
        
        AchievementLable ach = AchievementLable.Instance;
        ach.isAchievementGot = new bool[ach.Boxes.Length];
        for (int i = 0; i < ach.isAchievementGot.Length; i++)
            ach.isAchievementGot[i] = false;
        if (PlayerPrefs.GetInt("Enemy", 0) >= 10)
            ach.isAchievementGot[0] = true;
        if (PlayerPrefs.GetInt("Enemy", 0) >= 30)
            ach.isAchievementGot[1] = true;
        if (LevelManager.Instance.HaveThreeStar())
            ach.isAchievementGot[2] = true;
        if (LevelManager.Instance.WholeCityThreeStar())
            ach.isAchievementGot[3] = true;
        if (LevelManager.Instance.WholeMapThreeStar())
            ach.isAchievementGot[4] = true;
        ach.SetLabel();
    }
}
