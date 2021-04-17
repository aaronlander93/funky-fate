using UnityEngine;
using System;
using Photon.Pun;

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
    public static event NoteTiming ValidHit;
    public static event NoteTiming MissedHit;

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
            MusicInputController.ActionOnePressed += CheckForValidHit;

            StartMusic();
        }
    }

    private void OnDisable()
    {
        MusicInputController.ActionOnePressed -= CheckForValidHit;
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

    public void CheckForValidHit()
    {
        long currentTick = DateTime.Now.Ticks;
        TimeSpan span = new TimeSpan(currentTick - beatTick);

        if(span.TotalSeconds <  forgiveness || BeatSystem.secPerBeat - span.TotalSeconds < forgiveness)
        {
            Debug.Log("ON BEAT");
        }
        else
        {
            Debug.Log("OFF BEAT");
        }
    }

    public void StartMusic()
    {
        Conductor.CreateBeatInstance(song);
        Conductor.StartMusic();
    }
}