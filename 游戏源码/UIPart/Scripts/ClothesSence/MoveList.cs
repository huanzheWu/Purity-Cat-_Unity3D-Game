using UnityEngine;
using System.Collections;

/// <summary>
/// 用于移动背包中内容的脚本
/// 作者：卫子麒
/// </summary>

public class MoveList : MonoBehaviour {

    //分别记录鼠标点击的始位置的默认坐标和世界中坐标。
    private Vector3 mouseStartPosition = Vector3.zero;
    private Vector3 mouseStartPositionInWorld = Vector3.zero;

    //检测是否处于当前内容页。
    public bool m_isActive = false;

    //记录Panel空间的左上角和右下角位置。
    public Transform cornerLeftUp;
    public Transform cornerRightDown;

    //基础位置和目标位置
    private Vector3 BasePosition;
    private Vector3 AimPosition;

    //判断鼠标是否点击和点击区域是否为Panel空间内。
    public bool click = false;
    private bool inside = false;

    //存放UI的摄像机用于摄像机坐标和场景坐标变换。
    public Camera UIcamera;

    //存放鼠标变化是否完成。
    private bool isGotMouseChange = false;
    private bool isSetAim = false;

    //记录当前模块信息条数。
    public int m_imformationNum = 6;

    //
    private float ScaleRate = 0;

    void Start()
    {
        ScaleRate = (cornerLeftUp.position.x - cornerRightDown.position.x) / (UIcamera.WorldToScreenPoint(cornerLeftUp.position).x - UIcamera.WorldToScreenPoint(cornerRightDown.position).x);
    }


    private float Distance2Aim = 0f;
    private Vector3 LocalPosition = new Vector3(-50f, -15f, 0f);
    private float Rate = 0;
    void Update()
    {
        //当前页面被激活。
        if (m_isActive && !ButtonClick.Instance.isOption)
        {
            //检测鼠标点击事件。
            if (Input.GetMouseButtonDown(0) && !isGotMouseChange)
            {
                click = true;
                mouseStartPosition = Input.mousePosition;
                mouseStartPositionInWorld = UIcamera.ScreenToWorldPoint(Input.mousePosition);
                BasePosition = this.transform.position;
            }

            //检测鼠标点击位置。
            if (click && mouseStartPositionInWorld.x >= cornerLeftUp.position.x && mouseStartPositionInWorld.x <= cornerRightDown.position.x &&
                mouseStartPositionInWorld.y >= cornerRightDown.position.y && mouseStartPositionInWorld.y <= cornerLeftUp.position.y && !inside)
            {
                inside = true;
            }
            else if (click && !inside)
            {
                click = false;
                mouseStartPosition = Vector3.zero;
            }


            //移动列表。
            if (click && inside)
            {
                float temp = (Input.mousePosition.y - mouseStartPosition.y) * ScaleRate;
                this.transform.position = BasePosition + new Vector3(0f, temp, 0f);

                if (Input.GetMouseButtonUp(0))
                {
                    click = inside = false;
                    isGotMouseChange = true;
                }
            }

            if (isGotMouseChange)
            {
                if (!isSetAim)
                {
                    LocalPosition = this.transform.localPosition;
                    if (LocalPosition.y < -15f)
                        AimPosition = LocalPosition + new Vector3(0f, -LocalPosition.y - 15f, 0f);
                    else if (LocalPosition.y > -15f + 105f * (m_imformationNum - 3))
                        AimPosition = LocalPosition + new Vector3(0f, -LocalPosition.y - 15f + 105f * (m_imformationNum - 3), 0f);
                    else
                    {
                        int temp = (int)(LocalPosition.y + 15) / 105;
                        if (LocalPosition.y - (-15 + 105 * temp) > (-15 + 105 * (temp + 1)) - LocalPosition.y)
                            AimPosition = LocalPosition - new Vector3(0, LocalPosition.y - (-15 + 105 * (temp + 1)), 0);
                        else if (LocalPosition.y - (-15 + 105 * temp) < (-15 + 105 * (temp + 1)) - LocalPosition.y)
                            AimPosition = LocalPosition - new Vector3(0, LocalPosition.y - (-15 + 105 * temp), 0);
                        else
                        {
                            if (BasePosition.y > LocalPosition.y)
                                AimPosition = LocalPosition - new Vector3(0, LocalPosition.y - (-15 + 105 * (temp + 1)), 0);
                            else
                                AimPosition = LocalPosition - new Vector3(0, LocalPosition.y - (-15 + 105 * temp), 0);
                        }
                    }
                    isSetAim = true;
                    Distance2Aim = Mathf.Abs(AimPosition.y - LocalPosition.y);
                }
                else
                {
                    Rate += 5f * Time.deltaTime;
                    this.transform.localPosition = Vector3.Lerp(LocalPosition, AimPosition, Rate);
                    if (Rate >= 1)
                    {
                        this.transform.localPosition = AimPosition;
                        Rate = 0;
                        isGotMouseChange = isSetAim = false;
                    }
                }
            }
        }
    }
}
