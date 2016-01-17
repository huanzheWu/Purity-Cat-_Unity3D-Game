using UnityEngine;
using System.Collections;

/*************************************
 * 赋值对象：路上的绳子
 * 功能：用于高速道具UI玩家捡起了绳子
 * *********************************/
public class Gro_Rope : MonoBehaviour {

    PropUI propUI;
    public Object musicOfEat;
    // Use this for initialization
    public Object effectOfEat;
    void Start()
    {
        GameObject objectPropUI = (GameObject)GameObject.FindGameObjectWithTag("PropUI");
        propUI = objectPropUI.GetComponent<PropUI>();

    }
    //成功调用
    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            Instantiate(effectOfEat, this.transform.position, Quaternion.identity);
            Instantiate(musicOfEat, this.transform.position, Quaternion.identity);
            propUI.AddProp(PropType.Rope);
            Destroy(this.gameObject);
        }
    }
}
