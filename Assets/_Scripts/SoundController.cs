using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundController : MonoBehaviour {

    public Slider masterSlider, musicSlider, sfxSlider;
    public GameObject backgroundSound;
    
    private void Start()
    {
        backgroundSound = GameObject.FindGameObjectWithTag("BackgroundMusic");

        masterSlider.value = Settings.masterVolume;
        musicSlider.value = Settings.musicVolume;
        sfxSlider.value = Settings.sfxVolume;
        AudioListener.volume = Settings.masterVolume;
        
        masterSlider.onValueChanged.AddListener(delegate { MasterVolume(); });
        musicSlider.onValueChanged.AddListener(delegate { MusicVolume(); });
        sfxSlider.onValueChanged.AddListener(delegate { SfxVolume(); });
    }

    void MasterVolume()
    {
        float vol = masterSlider.value;
        AudioListener.volume = vol;
        Settings.masterVolume = vol;
    }

    void MusicVolume()
    {
        float vol = musicSlider.value;
        backgroundSound.GetComponent<BackgroundSoundManager>().MusicVolume(vol);
    }

    void SfxVolume()
    {

    }

}
