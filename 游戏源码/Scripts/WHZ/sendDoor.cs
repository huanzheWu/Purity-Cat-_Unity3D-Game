using UnityEngine;
using System.Collections;

public class sendDoor : MonoBehaviour {

    private GameObject Player;
    private Vector2 velocity;
    public Transform anotherDoor;

    // Use this for initialization
    void Start()
    {
        Player = GameObject.Find("Player");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
      
        if (other.tag == "Player")
        {
            PlayerState playState = Player.GetComponent<PlayerControl>().GetState();
            if (playState == PlayerState.Hitted)
                return;
            Player.transform.position = anotherDoor.transform.position;
            //传送主角的同时也应该要移动摄像机。
           float  cameraMove =  anotherDoor.position.x - this.transform.position.x;
           Camera.main.transform.position = new Vector3(Camera.main.transform.position.x + cameraMove, Camera.main.transform.position.y, Camera.main.transform.position.z);
        }
    }
}
