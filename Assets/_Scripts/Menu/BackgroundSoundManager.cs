using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundSoundManager : MonoBehaviour {

    AudioSource audioSource;

    private void Awake()
    {
        GameObject[] obj = GameObject.FindGameObjectsWithTag("BackgroundMusic");
        if (obj.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this);

        audioSource = GetComponent<AudioSource>();
        audioSource.volume = mSettings.musicVolume;
    }


    public void MusicVolume(float f)
    {
        audioSource.volume = f;
        mSettings.musicVolume = f;
    }

}
