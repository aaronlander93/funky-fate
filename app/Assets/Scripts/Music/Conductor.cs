using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conductor : MonoBehaviour
{
    private static FMOD.Studio.EventInstance _instance;
    private static FMOD.Studio.EventInstance _sfx;

    private static BeatSystem bS;

    void Awake()
    {
        bS = GameObject.Find("BeatSystem").GetComponent<BeatSystem>();
    }

    public static void CreateBeatInstance(string song)
    {
        _instance = FMODUnity.RuntimeManager.CreateInstance(song);
        _instance.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(bS.transform));
    }

    public static void CreateSFXInstance(string sfx)
    {
        _sfx = FMODUnity.RuntimeManager.CreateInstance(sfx);
        _sfx.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(bS.transform));
    }

    public static void PlaySFX(string sfx)
    {
        CreateSFXInstance(sfx);
        _sfx.start();
        _sfx.release();
    }

    public static void PlaySFX(string sfx, float volume)
    {
        CreateSFXInstance(sfx);
        _sfx.setParameterByName("Volume", volume);
        _sfx.start();
        _sfx.release();
    }
    public static void StartMusic()
    {
        _instance.start();
        bS.AssignBeatEvent(_instance);
    }

    public static void StopAndClear()
    {
        bS.StopAndClear(_instance);
    }

    public static void PauseMusic()
    {
        bS.Pause(_instance);
    }

    public static void ResumeMusic()
    {
        bS.Resume(_instance);
    }
}