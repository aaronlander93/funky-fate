using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TriggerAudio : MonoBehaviour
{
    public MusicManager mm;

    [FMODUnity.EventRef]
    public string run;

    [FMODUnity.EventRef]
    public string jump;

    public void PlayOneShot(string sound)
    {
        switch (sound)
        {
            case "Run":
                FMODUnity.RuntimeManager.PlayOneShotAttached(run, gameObject);
                break;
            case "Jump":
                FMODUnity.RuntimeManager.PlayOneShotAttached(jump, gameObject);
                break;
        }
        
    }

    public void PlayOneShot(string sound, float volume)
    {
        switch (sound)
        {
            case "Run":
                Conductor.PlaySFX(run, volume);
                break;
            case "Jump":
                Conductor.PlaySFX(jump, volume);
                break;
        }
    }
}