using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SfxController : MonoBehaviour {

    private void Awake()
    {
        GameObject[] obj = GameObject.FindGameObjectsWithTag("Sfx");
        if (obj.Length > 2)
        {
            Destroy(obj[0]);
            Destroy(obj[1]);
        }

        DontDestroyOnLoad(this);
    }
}
