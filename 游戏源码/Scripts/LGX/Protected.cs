using UnityEngine;
using System.Collections;

public class Protected : MonoBehaviour {

    float exitTime = 5;
    Transform playerTrans;
    bool isRelease;
    PropUI propUI;
    public Object effectOfEat;
    public Object musicOfEat;
    Animator anim;
    PlayerControl scriPlay;

	void Start () {
        GameObject temp = (GameObject)GameObject.FindGameObjectWithTag("Player");
        playerTrans = temp.GetComponent<Transform>();
        scriPlay = temp.GetComponent<PlayerControl>();
        GameObject objectPropUI = (GameObject)GameObject.FindGameObjectWithTag("PropUI");
        propUI = objectPropUI.GetComponent<PropUI>();
       
	}
    void FixedUpdate()
    {
        if(isRelease == true)
        {
            exitTime -= Time.deltaTime;
            if (exitTime <= 0)
            {
                playerTrans.GetComponent<PlayerControl>().isProtect = false;
                Destroy(this.gameObject);
            } 
            this.transform.position = playerTrans.position;
        }
    }
    public void Init()
    {
        if (scriPlay == null)
        {
            Start();
        }
        scriPlay.isProtect = true;
        anim = this.GetComponent<Animator>();
        isRelease = true;
        this.transform.localScale = new Vector3(2, 2, 2);
        Transform child = this.transform.FindChild("Hotspot");
        Destroy(child.gameObject);
        //child.localScale = new Vector3(0, 0, 0);
    }
    public void OnTriggerEnter2D(Collider2D col)
    {   
        //处在跑道上
        if (isRelease == false && col.tag == "Player")
        {
            Debug.Log("添加保护jfasasdfasf");
            Instantiate(effectOfEat, this.transform.position, Quaternion.identity);
            Instantiate(musicOfEat, this.transform.position, Quaternion.identity);
            propUI.AddProp(PropType.Protected);
            Destroy(this.gameObject);
        }
        else if(isRelease == true)//已经是释放的道具
        {
            switch (col.tag)
            {
                case "Cell":
                    Destroy(col.gameObject);
                    break;
            }
        }
    }


}
