using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSettings : MonoBehaviour {

    FMOD.Studio.Bus Music;
    FMOD.Studio.Bus Master;

    float MusicVolume = 0.5f;
    float MasterVolume = 1f;

    void Awake () {
        Music = FMODUnity.RuntimeManager.GetBus ("bus:/Master/Music");
        Master = FMODUnity.RuntimeManager.GetBus ("bus:/Master");
    }

    void Update () {
        Music.setVolume (MusicVolume);
        Master.setVolume (MasterVolume);
    }

    public void MasterVolumeLevel (float newMasterVolume) {
        MasterVolume = newMasterVolume;
    }

    public void MusicVolumeLevel (float newMusicVolume) {
        MusicVolume = newMusicVolume;
    }
}