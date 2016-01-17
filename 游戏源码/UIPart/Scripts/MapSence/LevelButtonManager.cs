using UnityEngine;
using System.Collections;

public class LevelButtonManager : MonoBehaviour {

    public GameObject Cover;
    public GameObject[] Star;

    private UISprite sprite = null;

    void Awake()
    {
        sprite = this.GetComponent<UISprite>();


        Cover.SetActive(false);
        for (int i = 0; i < Star.Length; i++)
        {
            Star[i].SetActive(false);
        }
    }

    public void SetCoverAndStar(bool isLevelOpen, int StarNum)
    {
        if (isLevelOpen)
            Cover.SetActive(false);
        else
        {
            Cover.SetActive(true);
        }

        if (Cover.activeInHierarchy)
        {
            sprite.spriteName = "方框";
            sprite.color = new Vector4(0.8f, 0.8f, 0.8f, 1f);
        }
        else
        {
            sprite.spriteName = "背框";
            sprite.color = new Vector4(1f, 1f, 1f, 1f);
        }

        for (int i = 0; i < Star.Length; i++)
        {
            if (i < StarNum)
                Star[i].SetActive(true);
            else
                Star[i].SetActive(false);
        }
    }
}
