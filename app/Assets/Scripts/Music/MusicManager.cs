using UnityEngine;
using System;
using System.Collections;
using Photon.Pun;
using UnityEngine.Experimental.Rendering.LWRP;

public class MusicManager : MonoBehaviour
{
    public PhotonView pv;
    public Renderer rend;
    public UnityEngine.Experimental.Rendering.Universal.Light2D light2D;

    public Material playerMat;
    public Material onBeatMat;
    public Material offBeatMat;

    private Color origColor;
    private Color onBeatColor = new Color(.278f, 1.0f, .043f, 1.0f);
    private Color offBeatColor = new Color(.87f, 0f, .067f, 1.0f);

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

    private bool validHit;

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
            MusicInputController.ActionOneReleased += ToggleColor;

            StartMusic();
        }

        if (!GameConfig.Multiplayer)
        {
            Movement2D.JumpEvent += PlaySound;
        }

        origColor = light2D.color;
    }

    private void OnDisable()
    {
        MusicInputController.ActionOnePressed -= CheckForValidHit;
    }

    private void MarkBeat()
    {
        beatTick = DateTime.Now.Ticks;
    }

    private void ToggleColor()
    {
        if(validHit)
        {
            rend.material = onBeatMat;
            light2D.color = onBeatColor;
        }
        else
        {
            rend.material = offBeatMat;
            light2D.color = offBeatColor;
        }

        light2D.intensity = 3;

        StartCoroutine(ApplyOriginalMaterial());
    }

    // Puts back the original material after .3 seconds
    IEnumerator ApplyOriginalMaterial()
    {
        yield return new WaitForSeconds(.4f);

        rend.material = playerMat;
        light2D.color = origColor;
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
            validHit = true;
        }
        else
        {
            validHit = false;
        }
    }

    public void StartMusic()
    {
        Conductor.CreateBeatInstance(song);
        Conductor.StartMusic();
    }
}