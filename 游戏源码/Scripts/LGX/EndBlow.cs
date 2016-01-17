using UnityEngine;
using System.Collections;

public class EndBlow : MonoBehaviour {

    LineRenderer lineRen;
    Vector3 beginPosi;
    Vector3 endPosi;
    Vector3 moveDir;
    float angle;
    float linelength = 1f;
    float beginWidth = -1;
    float endWidth = -1;
    float beginWidVel = 0.98f;
    float endWidVel = 0.99f;
    const float animTimeCon = 2f;
    float distance;


    IEnumerator Move()
    {
        do
        {
            beginPosi += distance * Time.deltaTime * moveDir / animTimeCon;
            if (beginPosi.x > endPosi.x)
            {
                linelength = 0f;
                beginPosi = endPosi;
            } 
            lineRen.SetPosition(0, beginPosi);
            lineRen.SetPosition(1, beginPosi + moveDir * linelength);
            lineRen.SetWidth(beginWidth, endWidth);
            beginWidth *= beginWidVel;
            endWidth *= endWidVel;

            yield return null;
        } while ((Vector3.Magnitude(endPosi - beginPosi) > (linelength - 0.5f))
            && (beginPosi.x < endPosi.x));
        Destroy(this.gameObject);
    }
    public void Init(Vector3 beginPosiPara, Vector3 endPosiPara)
    {
        lineRen = this.GetComponent<LineRenderer>();
        beginPosi = beginPosiPara;
        endPosi = endPosiPara;
        angle = Mathf.Atan((endPosi.y - beginPosi.y) / (endPosi.x - beginPosi.x));
        moveDir = endPosi - beginPosi;
        distance = Vector3.Magnitude(moveDir) - linelength;//计算起点与终点的长度再减去长度，等于最终需要移动的位移
        moveDir = Vector3.Normalize(moveDir);
        StartCoroutine("Move");
    }
}
