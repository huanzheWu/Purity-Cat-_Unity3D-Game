using UnityEngine;
using System.Collections;

public class starAnimatorControl : MonoBehaviour {

    Animator anim;
    AnimatorStateInfo animatorInfo;
	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();

	}
	
	// Update is called once per frame
	void Update () {
        //animatorInfo = anim.GetCurrentAnimatorStateInfo(0);
        //if(animatorInfo.IsName("star"))
        //{
        //    anim.speed = 0;
        //}
	
	}
}
