using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
public class countNumFastThanPlayer : MonoBehaviour
{

    private PlayerControl player;
    //主角当前的位置排名
    public int count = 1;
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerControl>();
    }
    // Update is called once per frame
    void Update()
    {
        if (player.m_isFinish == false)
            count = 1;
        //计算主角当前的位置排名
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform haze = transform.GetChild(i);
            if (player.m_isFinish == false && haze.transform.position.x > player.transform.position.x)
            {
                count++;
            }
        }
    }
}