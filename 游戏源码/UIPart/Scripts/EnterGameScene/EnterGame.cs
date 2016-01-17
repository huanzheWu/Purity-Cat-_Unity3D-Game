using UnityEngine;
using System.Collections;
using System;
using System.IO;
/// <summary>
/// 进入游戏的简单跳转页面
/// 作者：卫子麒
/// </summary>
public class EnterGame : MonoBehaviour {

    public GameObject m_RandomPlace;
    public GameObject m_Loading;
    public GameObject m_Point1;
    public GameObject m_Point2;
    public GameObject m_Point3;

	// Use this for initialization
	void Start () {

        if (File.Exists(Application.persistentDataPath + "Level.xml"))
        {
         File.Delete(Application.persistentDataPath + "Level.xml");
        }
        PlayerPrefs.SetInt("PlayTimes", 0);
        int playTimes = PlayerPrefs.GetInt("PlayTimes", 0);
        PlayerPrefs.SetInt("PlayTimes", playTimes);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(Loading());
            StartCoroutine(LoadingSence());
            //Application.LoadLevel("ClothesSence");
        }
	}

    private IEnumerator LoadingSence()
    {
        AsyncOperation async = Application.LoadLevelAsync("ClothesSence");
        yield return async;
    }
    
    private IEnumerator Loading()
    {
        m_RandomPlace.SetActive(false);
        m_Loading.SetActive(true);
        while (Application.loadedLevelName == "EnterGameSence")
        {
            yield return new WaitForSeconds(0.2f);
            m_Point1.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            m_Point2.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            m_Point3.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            m_Point1.SetActive(false);
            m_Point2.SetActive(false);
            m_Point3.SetActive(false);
        }
    }
}
