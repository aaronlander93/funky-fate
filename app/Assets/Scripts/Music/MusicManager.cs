using UnityEngine;
using System;

public class MusicManager : MonoBehaviour
{
    public static float keyDownTime;

    public float forgiveness;

    [FMODUnity.EventRef]
    public string song;

    public delegate void NoteTiming();
    public static event NoteTiming ValidHit;
    public static event NoteTiming MissedHit;

    private static long beatTick;

    private void Start()
    {
        BeatSystem.OnBeat += MarkBeat;
        MusicInputController.ActionOnePressed += CheckForValidHit;
    }

    private void OnDisable()
    {
        MusicInputController.ActionOnePressed -= CheckForValidHit;
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

    public void MarkBeat()
    {
        beatTick = DateTime.Now.Ticks;
    }

    public void StartMusic()
    {
        Conductor.CreateBeatInstance(song);
        Conductor.StartMusic();
    }
}