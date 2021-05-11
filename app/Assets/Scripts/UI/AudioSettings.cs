using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
Author: Niall (sort-of. I stole it from https://www.youtube.com/watch?v=yQgVKR6PMqo)
This script updates the bus volumes when the in-game volume sliders are changed
*/

public class AudioSettings : MonoBehaviour {

    FMOD.Studio.Bus Music;
    FMOD.Studio.Bus Master;

    float musicVolume = 0.5f;
    float masterVolume = 1f;

    public Slider masterSlider;
    public Slider musicSlider;

    void Awake () {
        Music = FMODUnity.RuntimeManager.GetBus ("bus:/Master/Music");
        Master = FMODUnity.RuntimeManager.GetBus ("bus:/Master");

        masterSlider.value = GameConfig.MasterVolume;
        musicSlider.value = GameConfig.MusicVolume;
    }

    void Update () {
        Music.setVolume (musicVolume);
        Master.setVolume (masterVolume);
    }

    public void MasterVolumeLevel (float newMasterVolume) {
        masterVolume = newMasterVolume;

        GameConfig.MasterVolume = newMasterVolume;
    }

    public void MusicVolumeLevel (float newMusicVolume) {
        musicVolume = newMusicVolume;

        GameConfig.MusicVolume = newMusicVolume;
    }
}