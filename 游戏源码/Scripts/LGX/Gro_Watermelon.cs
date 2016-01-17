using UnityEngine;
using System.Collections;

public class Gro_Watermelon : MonoBehaviour {

    PropUI propUI;
    GameObject objectPropUI;
    public Object effectOfEat;
    public Object musicOfEat;
	void Start () 
    {
        objectPropUI = (GameObject)GameObject.FindGameObjectWithTag("PropUI");
        propUI = objectPropUI.GetComponent<PropUI>();
	}


    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            Instantiate(effectOfEat, this.transform.position, Quaternion.identity);
            Instantiate(musicOfEat, this.transform.position, Quaternion.identity);
            propUI.AddProp(PropType.Watermelon);
            Destroy(this.gameObject);
        }
    }
}
