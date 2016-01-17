using UnityEngine;
using System.Collections;
/// <summary>
/// 初始界面按钮点击模块
/// 作者：卫子麒
/// </summary>

//所有列表的枚举表
public enum SHOWLIST {Item = 0, Achievement = 1 };

public class ButtonClick : MonoBehaviour
{
    //当前列表
    private SHOWLIST currentList = SHOWLIST.Item;

    //滑动模块保存，用于设置滑动功能是否激活
    public MoveList[] Lists = null;

    //整个背景滑动
    public Transform m_BGList = null;

    //设置模块
    public TweenAlpha SettingPanel = null;

    //音量滑动条
    public UISlider m_Slider = null;

    //按钮序号
    private int buttonNum = 0;

    //左移右移操作标记
    private bool movingLeft = false;
    private bool movingRight = false;

    //设置页面是否被打开
    public bool isOption = false;

    //设置的音量图案标识
    public GameObject Closed = null;
    public GameObject BBB = null;

    //一个模块的长度
    private const float SenceLength = 270f;

    public static ButtonClick Instance = null;

    void Awake()
    {
        if (!Instance)
            Instance = this;
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    //设置初始化的模块活跃
    void Start()
    {
        SetCurrentListActive();
    }

    void Update()
    {
        //左移标记被激活，做左移操作
        if (movingLeft)
        {
            Time.timeScale = 1;
            int temp = -(int)currentList - 1;
            float distance = Vector2.Distance(m_BGList.localPosition, new Vector2(temp * SenceLength + 15, 0));
            m_BGList.localPosition = Vector2.Lerp(m_BGList.localPosition, new Vector2(temp * SenceLength + 15, 0), SenceLength * 2.5f / distance * Time.deltaTime);
            if (m_BGList.localPosition == new Vector3(temp * SenceLength + 15, 0))
            {
                movingLeft = false;
                currentList = (SHOWLIST)((int)currentList + 1);
                SetCurrentListActive();
            }
        }

        //右移标记被激活，做右移操作
        if (movingRight)
        {
            Time.timeScale = 1;
            int temp = -(int)currentList + 1;
            float distance = Vector2.Distance(m_BGList.localPosition, new Vector2(temp * SenceLength + 15, 0));
            m_BGList.localPosition = Vector2.Lerp(m_BGList.localPosition, new Vector2(temp * SenceLength + 15, 0), SenceLength * 2.5f / distance * Time.deltaTime);
            if (m_BGList.localPosition == new Vector3(temp * SenceLength + 15, 0))
            {
                movingRight = false;
                currentList = (SHOWLIST)((int)currentList - 1);
                SetCurrentListActive();
            }
        }
    }

    //“下一个页面”按钮被点击
    public void NextButtonClick()
    {
        if (!movingLeft && !movingRight && !isOption)
        {
            movingLeft = true;
        }
    }

    //“前一个页面”按钮被点击
    public void FrontButtonClick()
    {
        if (!movingLeft && !movingRight && !isOption)
            movingRight = true;
    }

    //设置当前背包模块中按钮的活跃。
    private void SetCurrentListActive()
    {
        for (int i = 0; i < Lists.Length; i++)
            Lists[i].m_isActive = false;
        Lists[(int)currentList].m_isActive = true;
    }

    //跳转场景按钮被点击
    public void NextSenceButtonClick()
    {
        if (!isOption)
        {
            PlayerPrefs.SetInt("City", 0);
            Application.LoadLevel("MapSence");
        }
    }

    //退出按钮被点击
    public void ExitButtonClick()
    {
        if(!isOption)
            Application.Quit();
    }

    //设置按钮被点击
    public void OptionButtonClick()
    {
        SettingPanel.PlayForward();
        float tempFloat;
        GameManager.Instance.GetSettingData(out tempFloat);
        m_Slider.value = tempFloat;
        isOption = true;
    }

    //值发生变化
    public void MusicValueExchange()
    {
        if(GameManager.Instance != null)
            GameManager.Instance.SetSettingData(m_Slider.value);
        if (m_Slider.value == 0)
            MusicClose();
        else
            MusicOpen();
    }

    //关闭按钮被点击
    public void RefuseButtonClick()
    {
        isOption = false;
        SettingPanel.PlayReverse();
    }

    //音量关闭
    public void MusicClose()
    {
        Closed.SetActive(true);
        BBB.SetActive(false);
    }

    //音量打开
    public void MusicOpen()
    {
        Closed.SetActive(false);
        BBB.SetActive(true);
    }
}
