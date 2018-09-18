using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SfxController : MonoBehaviour {

    private void Awake()
    {
        GameObject[] click = GameObject.FindGameObjectsWithTag("SfxClick");
        if (click.Length > 1)
        {
            Destroy(click[0]);
        }
        GameObject[] swipe = GameObject.FindGameObjectsWithTag("SfxSwipe");
        if (swipe.Length > 1)
        {
            Destroy(swipe[0]);
        }
        GameObject[] select = GameObject.FindGameObjectsWithTag("SfxSelect");
        if (select.Length > 1)
        {
            Destroy(select[0]);
        }

        DontDestroyOnLoad(this);
    }
}
