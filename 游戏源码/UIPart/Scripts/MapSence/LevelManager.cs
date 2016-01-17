using UnityEngine;
using System.Collections;

/// <summary>
/// 关卡管理模块
/// 作者：卫子麒
/// </summary>

public class LevelManager : MonoBehaviour {

    //单例化
    public static LevelManager Instance = null;

    //关卡按钮
    public UIButton[] m_LevelButton;

    //城市按钮和城市按钮的遮罩
    public UIButton[] m_CityButton;
    public GameObject[] m_CityButtonCover;

    //关卡内容类，用于整理XML读取文件内容。
    public class LevelContent
    {
        public int City;
        public int Level;
        public bool Open;
        public bool Pass;
        public int StarNum;
    } ;

    //关卡列表，记录所有关卡内容
    private ArrayList LevelList = new ArrayList();

    //单例初始化
    void Awake()
    {
        if (!Instance)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }

    //从XML读取出来的混乱列表整理成规则列表。
    public void GetImformation(ArrayList ImformationList)
    {
        LevelList.Clear();
        for (int i = 0; i < ImformationList.Count; i++)
        {
            ArrayList Temp = ImformationList[i] as ArrayList;
            LevelContent Contenttemp = new LevelContent();
            string strTemp = Temp[0] as string;
            Contenttemp.City = (int)(strTemp[0] - '0');
            Contenttemp.Level = (int)(strTemp[2] - '0');
            strTemp = Temp[1] as string;
            Contenttemp.Open = strTemp == "true";
            strTemp = Temp[2] as string;
            Contenttemp.Pass = strTemp == "true";
            strTemp = Temp[3] as string;
            Contenttemp.StarNum = (int)(strTemp[0] - '0');
            LevelList.Add(Contenttemp);
        }
    }

    //获取规则列表的某个城市某个关卡的信息
    public LevelContent GetLevelByCityAndLevel(int City, int Level)
    {
        LevelContent Temp = LevelList[(City - 1) * 8 + Level - 1] as LevelContent;
        return Temp;
    }

    //设置城市的图样和按键激活情况
    public void SetCityPicture()
    {
        for (int i = 0; i < m_CityButton.Length; ++i)
        {
            if (!GetLevelByCityAndLevel(i + 1, 1).Open)
            {
                m_CityButton[i].enabled = false;
                m_CityButtonCover[i].SetActive(true);
            }
            else
            {
                m_CityButton[i].enabled = true;
                m_CityButtonCover[i].SetActive(false);
            }
            m_CityButton[i].GetComponent<UIButtonScale>().enabled = m_CityButton[i].enabled;
        }
    }

    public void IceAllCity()
    {
        for (int i = 0; i < m_CityButton.Length; ++i)
        {
            m_CityButton[i].enabled = false;
            m_CityButton[i].GetComponent<UIButtonScale>().enabled = false;
            m_CityButtonCover[i].SetActive(true);
        }
    }

    //设置关卡的图样和按键激活情况
    public void SetLevelPicture(int city)
    {
        for (int i = 0; i < m_LevelButton.Length; ++i)
        {
            LevelContent Temp = GetLevelByCityAndLevel(city, i + 1);
            LevelButtonManager ButtonIManager = m_LevelButton[i].GetComponent<LevelButtonManager>();
            if (!Temp.Open)
                m_LevelButton[i].enabled = false;
            else
                m_LevelButton[i].enabled = true;
            ButtonIManager.SetCoverAndStar(Temp.Open, Temp.StarNum);
        }
    }

    public bool HaveThreeStar()
    {
        for (int i = 1; i < 5; i++)
            for (int j = 1; j < 9; j++)
            {
                if (GetLevelByCityAndLevel(i, j).StarNum == 3)
                    return true;
            }
        return false;
    }

    public bool WholeCityThreeStar()
    {
		return GetLevelByCityAndLevel(2, 1).Open;
    }

    public bool WholeMapThreeStar()
    {
		return GetLevelByCityAndLevel(4, 8).Open && (GetLevelByCityAndLevel(4, 8).StarNum != 0);
    }
}
