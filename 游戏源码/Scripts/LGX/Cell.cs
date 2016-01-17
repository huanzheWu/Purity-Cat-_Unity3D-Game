using UnityEngine;
using System.Collections;

public class Cell : MonoBehaviour {

    Animator anim;
    PlayerControl playCon;
    bool isHit = false;
    Rigidbody2D myRigid2D;
    Vector2 velocity = new Vector2(7, 0);
    Transform target;

    public void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == "Player" && isHit == false)
        {
            if (playCon.isProtect == true)
            {
                Destroy(this.gameObject);
                //return;
            }
            else if (playCon.GetState() == PlayerState.Hitted)
            {
                isHit = true;
                Debug.Log("增加了气泡的时间");
                playCon.AddCellTime();
                Destroy(this.gameObject);//假如原本已经处于气泡的状态，只需要将气泡的时间延长，然后将当前的气泡销毁
                return;
            }
            anim.SetBool("Success", true);
            playCon.GetGlueHit(true);
            playCon.SetGlue(this.gameObject);
            isHit = true;
            target = col.GetComponent<Transform>();
            myRigid2D.velocity = new Vector2(0, 0);
            StartCoroutine("DestroyThis");
            Debug.Log("成功攻击了主角");
        }
    }

    void MyStart()
    {
        myRigid2D = this.GetComponent<Rigidbody2D>();
        GameObject player = GameObject.FindWithTag("Player");
        playCon = player.GetComponent<PlayerControl>();
        anim = this.GetComponent<Animator>();
    }

    public void SetInit(Vector2 paraVelocity)
    {
        int minVelo = 11;
        MyStart();
        velocity.x += paraVelocity.x;
        if (velocity.x < minVelo)
            velocity.x = minVelo;
        Vector2 temp = new Vector2(velocity.x, 0);
        myRigid2D.velocity = temp;
    }
    float timeCount = 1f;
    IEnumerator DestroyThis()
    {
        while (timeCount > 0)
        {
            timeCount -= Time.deltaTime;
            yield return null;
        }
        playCon.GetGlueHit(false);
        Destroy(this.gameObject);

    }
    void FixedUpdate()
    {
        if (isHit == true)
            this.transform.position = target.position;
    }
    public void AddKeepTime()
    {
        timeCount = 1f;
    }
}
