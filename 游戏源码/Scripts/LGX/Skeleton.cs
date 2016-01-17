using UnityEngine;
using System.Collections;

public class Skeleton : MonoBehaviour {

    Vector3 roVelo = new Vector3(0, 0, -15);
    HazeControl target;
    Transform transTarget;
    bool isHitted = false;
    bool isInit = false;
    PropUI scriPropUI;
    public Object effectOfEat;
    public Object musicOfEat;
    

    public void Init(Vector3 posi)
    {
        this.transform.position = posi;
        Rigidbody2D rigid2D = this.GetComponent<Rigidbody2D>();
        rigid2D.velocity = new Vector2(15, 0);
        GameObject gamePropUI = (GameObject)GameObject.FindGameObjectWithTag("PropUI");
        scriPropUI = gamePropUI.GetComponent<PropUI>();
        isInit = true;
        Transform transChild = transform.FindChild("Hotspot").GetComponent<Transform>();
        transChild.parent = null;
        Destroy(transChild.gameObject);
    }

	void Start () {
        GameObject gamePropUI = (GameObject)GameObject.FindGameObjectWithTag("PropUI");
        scriPropUI = gamePropUI.GetComponent<PropUI>();
	}
	
	void FixedUpdate () {
        if (isInit == true)
        {
            this.transform.Rotate(roVelo);
            if (transTarget != null)
                this.transform.position = transTarget.position + new Vector3(-0.3f,0.6f,0);     
        }
	}
    void OnTriggerEnter2D(Collider2D col)
    {
        if (isInit == false && col.tag == "Player")
        {
            scriPropUI.AddProp(PropType.Skelon);
            Instantiate(effectOfEat, this.transform.position, Quaternion.identity);
            Instantiate(musicOfEat, this.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
        else if (isInit == true && isHitted == false && col.tag == "Haze")
        {
            isHitted = true;
            roVelo = new Vector3(0, 0, 0);
            Animator anim = this.GetComponent<Animator>();
            target = col.GetComponent<HazeControl>();
            transTarget = col.GetComponent<Transform>();
            StartCoroutine("AttackSucc");
        }

    }
    IEnumerator AttackSucc()
    {
        target.IsSkelon(true);
        float timeCount = 3;
        do
        {
            timeCount -= Time.deltaTime;
            yield return null;
        } while (timeCount > 0);
        target.IsSkelon(false);
        Destroy(this.gameObject);
        StopCoroutine("AttackSucc");
    }
}
