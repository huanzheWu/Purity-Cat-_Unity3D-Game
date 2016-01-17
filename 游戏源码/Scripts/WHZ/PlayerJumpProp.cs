using UnityEngine;
using System.Collections;

public class PlayerJumpProp : MonoBehaviour {

    private GameObject Player;
    private Vector2 velocity;
    private float timer;
    private bool isJump;
    // Use this for initialization
	void Start () {
        timer = 0;
        Player = GameObject.Find("Player");
	}
	
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag=="Player")
        {
            PlayerState playState = Player.GetComponent<PlayerControl>().GetState();
            if (playState == PlayerState.Hitted)
                return;
            this.GetComponent<Animator>().enabled = true;
            isJump = true;
            velocity= Player.GetComponent<Rigidbody2D>().velocity;
            velocity.y=10.0f;
            velocity.x = 5.0f;
            Player.GetComponent<Rigidbody2D>().velocity= velocity;
            Player.GetComponent<blureffect>().enabled = true;
        }
    }
    void FixedUpdate()
    {
        if (isJump == true)
        {
            timer += Time.deltaTime;
            if(timer>2.0f)
            {
                isJump = false;
                timer = 0;
                 Player.GetComponent<blureffect>().enabled = false;
            }
        }
    }
}
