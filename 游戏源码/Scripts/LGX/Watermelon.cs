using UnityEngine;
using System.Collections;

public class Watermelon : MonoBehaviour
{

    Rigidbody2D myRigid2D;
    Vector2 velocity = new Vector2(13,0);
    Rigidbody2D rigidPlayer;
    Transform transPlayer;
    bool isHitPlay;
    bool isHitHaze;
    Animator anim;
    Transform target;
    Vector3 propTranslate;
    PlayerControl scriPlayCon;
    HazeControl scriHazeCon;

    public Sprite m_GlueEffectSpri;

    //实例化的时候Start函数总是没有被调用
    void MyStart()
    {
        myRigid2D = this.GetComponent<Rigidbody2D>();
        GameObject playerObject = (GameObject)GameObject.FindGameObjectWithTag("Player");
        Transform transPlayer = playerObject.GetComponent<Transform>();
        rigidPlayer = playerObject.GetComponent<Rigidbody2D>();

        anim = this.GetComponent<Animator>();
        isHitPlay = false;
        isHitHaze = false;
    }
    //外界对西瓜进行操作的函数都通过SetInit来实现

    public void SetInit(Vector2 paraVelocity)
    {
        MyStart();
        float custPass = Application.loadedLevelName[2] - '0'; 
        velocity.x = paraVelocity.x + 10 + 0.3f * custPass;
        Vector2 temp = new Vector2(velocity.x, 0);
        myRigid2D.velocity = temp;
    }


    void Update()
    {
        if (isHitHaze == false && isHitPlay == false)
            this.transform.Rotate(new Vector3(0, 0, -10));
        else
        {   //设置胶水在目标身上
            this.transform.position = target.position + propTranslate;
        }
    }
    void HitPlay(Collider2D col)
    {
        target = col.GetComponent<Transform>();
        StartCoroutine(DestroyThis());//胶水抓住人之后一段时间，销毁物体
    }
    IEnumerator DestroyThis()
    {
        float timeCount = 3;
        while (timeCount > 0)
        {
            timeCount -= Time.deltaTime;
            yield return null;
        }
        Destroy(this.gameObject);
        if (isHitPlay == true)
            scriPlayCon.GetGlueHit(false);
        if (isHitHaze == true)
            scriHazeCon.GetGlueHit(false);
    }
    public void OnTriggerEnter2D(Collider2D col)
    {
        //确保胶水只砸中一个目标
        if (isHitHaze == false && this.name == "Player" && col.tag == "Haze")
        {
            isHitHaze = true;
            propTranslate = new Vector3(0, -0.2f, 0);//用于设置胶水相对主角或者NPC的偏移量
            HitPlay(col);
            scriHazeCon = col.GetComponent<HazeControl>();
            this.GetComponent<SpriteRenderer>().sprite = m_GlueEffectSpri;
            this.transform.rotation = Quaternion.identity;
            scriHazeCon.GetGlueHit(true);
        }
        else if (isHitPlay == false && this.name == "HazeAttack" && col.tag == "Player")
        {
            isHitPlay = true;
            propTranslate = new Vector3(0, -0.5f, 0);
            HitPlay(col);
            scriPlayCon = col.GetComponent<PlayerControl>();
            scriPlayCon.GetGlueHit(true);
            scriPlayCon.SetGlue(this.gameObject);
            this.GetComponent<SpriteRenderer>().sprite = m_GlueEffectSpri;
            this.transform.rotation = Quaternion.identity;
        }
        if (col.tag == "EndOfGame")
            Destroy(this.gameObject);
    }
}
