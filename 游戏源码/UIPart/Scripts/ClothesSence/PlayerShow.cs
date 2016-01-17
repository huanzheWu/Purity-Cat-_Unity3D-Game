using UnityEngine;
using System.Collections;
/// <summary>
/// 角色秀模块
/// 作者：卫子麒
/// </summary>
public class PlayerShow : MonoBehaviour {

    //单例
    public static PlayerShow Instance;

    //衣服，眼镜，鞋子编号
    private int Clothes;
    private int Glasses;
    private int Shoes;

    //单例初始化
    void Awake()
    {
        if (!Instance)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }
}
