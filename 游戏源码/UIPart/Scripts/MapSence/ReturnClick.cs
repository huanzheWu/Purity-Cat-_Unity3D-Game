using UnityEngine;
using System.Collections;
/// <summary>
///地图场景的按钮点击事件
///作者：卫子麒
/// </summary>

public class ReturnClick : MonoBehaviour {

    //是否打开选关和选关的界面
    private bool isOpenSelect = false;
    public TweenAlpha SelectBG = null;

    //
    private int ReturnCityNum;

    //记录选择的城市
    private int city = 0;

    public GameObject Title1 = null;
    public GameObject Title2 = null;

    void Start()
    {
        ReturnCityNum = PlayerPrefs.GetInt("City", 0);
        if (ReturnCityNum != 0)
            ClickCity(ReturnCityNum);
    }

    //返回按钮点击事件
    public void Return()
    {
        if (!isOpenSelect)
        {
            PlayerPrefs.SetInt("City", 0);
            Application.LoadLevel("ClothesSence");
        }
        else
        {
            isOpenSelect = false;
            SelectBG.PlayReverse();
            LevelManager.Instance.SetCityPicture();
            Title1.SetActive(true);
            Title2.SetActive(false);
        }
    }

    //城市点击事件
    public void ClickCity(int city)
    {
        if (!isOpenSelect)
        {
            this.city = city;
            SelectBG.PlayForward();
            isOpenSelect = true;
            LevelManager.Instance.IceAllCity();
            LevelManager.Instance.SetLevelPicture(city);
            Title1.SetActive(false);
            Title2.SetActive(true);
        }
    }

    //关闭选择关卡页面
    public void CloseSelect()
    {
        isOpenSelect = false;
        SelectBG.PlayReverse();
    }

    //进行关卡场景跳转
    public void SelectLevel(int level)
    {
        PlayerPrefs.SetInt("City", 0);
        Application.LoadLevel(city + "_" + level);
        //XMLManager xml = new XMLManager();
        //xml.ExchangePassLevelXML(city + "_" + level, Random.Range(1, 4));//xml.ExchangePassLevelXML(Application.loadedLevelName,(获得的星星数))
        //Application.LoadLevel("MapSence");
    }
}
