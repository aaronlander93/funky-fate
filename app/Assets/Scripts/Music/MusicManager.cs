using UnityEngine;
using System;
using System.Collections;
using Photon.Pun;
using UnityEngine.Experimental.Rendering.LWRP;

public class MusicManager : MonoBehaviour
{
    public PhotonView pv;
    

    public static float keyDownTime;

    public float forgiveness;

    [FMODUnity.EventRef]
    public string song;

    [FMODUnity.EventRef]
    public string jumpSFX;

    [FMODUnity.EventRef]
    public string walkSFX;

    public delegate void NoteTiming();
    public static event NoteTiming OnBeat;
    public static event NoteTiming OffBeat;

    private static long beatTick;

    private void Start()
    {
        if(GameConfig.Multiplayer && !pv.IsMine)
        {
            Destroy(gameObject);
        }
        else
        {
            BeatSystem.OnBeat += MarkBeat;
            MusicInputController.ActionOnePressed += CheckForOnBeat;

            StartMusic();
        }

        if (!GameConfig.Multiplayer)
        {
            Movement2D.JumpEvent += PlaySound;
        }
    }

    private void MarkBeat()
    {
        beatTick = DateTime.Now.Ticks;
    }

    public void PlaySound(string sound)
    {
        switch (sound)
        {
            case "Jump":
                Conductor.PlaySFX(jumpSFX);
                break;
        }
        
    }

    public void PlaySound(string sound, float volume)
    {
        switch (sound)
        {
            case "Jump":
                Conductor.PlaySFX(jumpSFX, volume);
                break;
        }

    }

    public void CheckForOnBeat()
    {
        long currentTick = DateTime.Now.Ticks;
        TimeSpan span = new TimeSpan(currentTick - beatTick);

        if(span.TotalSeconds <  forgiveness || BeatSystem.secPerBeat - span.TotalSeconds < forgiveness)
        {
            OnBeat();
        }
        else
        {
            OffBeat();
        }
    }

    public void StartMusic()
    {
        Conductor.CreateBeatInstance(song);
        Conductor.StartMusic();
    }
    
    void OnDestroy()
    {
        Conductor.StopAndClear();
        Movement2D.JumpEvent -= PlaySound;
    }
}