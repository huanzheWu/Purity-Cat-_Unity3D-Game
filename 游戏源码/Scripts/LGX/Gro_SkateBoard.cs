using UnityEngine;
using System.Collections;

public class Gro_SkateBoard : MonoBehaviour {

    PropUI propUI;
    public Object musicOfEat;
    public Object effectOfEat;
	// Use this for initialization
	void Start () {
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
            propUI.AddProp(PropType.SkateBoard);
            Destroy(this.gameObject);
        }
    }
}
