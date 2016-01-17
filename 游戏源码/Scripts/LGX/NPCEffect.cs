using UnityEngine;
using System.Collections;

public class NPCEffect : MonoBehaviour {
    public void DestroyEffect()
    {
        Destroy(this.gameObject);
    }
}
