using UnityEngine;
using System.Collections;
/// <summary>
/// 成就脚本
/// 作者：卫子麒
/// </summary>

public class AchievementLable : MonoBehaviour {

    public static AchievementLable Instance = null;

    public GameObject[] Dark = null;
    public GameObject[] Boxes = null;
    public GameObject[] Ach = null;

    /*成就内容
    1、"小试牛刀:消灭了10个雾霾怪",
    2、"小有成就:消灭了30个雾霾怪",
    3、"一网打尽:在关卡中消灭了所有敌人",
    4、"锋芒毕露:净化了一个城市!",
    5、"光明使者:成功净化了所有城市!"*/

    public bool[] isAchievementGot;

    void Awake()
    {
        if (!Instance)
            Instance = this;
        else if (Instance != this)
            Destroy(this.gameObject);
    }

    public void SetLabel()
    {
        int index = 0;
        for (int i = 0; i < Boxes.Length; i++)
        {
            if (isAchievementGot[i])
            {
                Ach[i].transform.parent = Boxes[index].transform;
                Ach[i].transform.localPosition = Vector3.zero;
                Dark[index].SetActive(false);
                index++;
            }
        }

        for (int i = 0; i < Boxes.Length; i++)
        {
            if (!isAchievementGot[i])
            {
                Ach[i].transform.parent = Boxes[index].transform;
                Ach[i].transform.localPosition = Vector3.zero;
                Dark[index].SetActive(true);
                index++;
            }
        }
    }
}
