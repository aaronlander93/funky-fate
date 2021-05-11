using System;
using System.Runtime.InteropServices;
using UnityEngine;

class BeatSystem : MonoBehaviour
{
    /// <summary>
    /// Used to parse data recieved from FMOD BeatEventCallback. 
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    class TimelineInfo
    {
        public int currentMusicBeat = 0;
        public FMOD.StringWrapper lastMarker = new FMOD.StringWrapper();
        public static float bpm;
    }

    TimelineInfo timelineInfo;
    GCHandle timelineHandle;

    public delegate void BeatAction();

    /// <summary>
    /// Subscribable event that fires every beat. 
    /// </summary>
    public static event BeatAction OnBeat;

    /// <summary>
    /// Subscribable event that fires every other beat. 
    /// </summary>
    public static event BeatAction OnOtherBeat;

    public delegate void MarkerAction();

    /// <summary>
    /// Subscribable event that fires every time a marker is passed. 
    /// </summary>
    public static event MarkerAction OnMarker;

    /// <summary>
    /// FMOD event callback that fires every beat. Used within the class to gather data about the track assigned to the BeatSystem. 
    /// </summary>
    private FMOD.Studio.EVENT_CALLBACK beatCallback;

    /// <summary>
    ///  Returns the current beat in the song.  
    /// </summary>  
    public static int beat;

    /// <summary>
    ///  Returns the beat index of the song.
    /// </summary>  
    public static int beatIndex = 0;

    /// <summary>
    ///  Returns a string of the most recent marker passed. 
    /// </summary>     
    public static string marker;


    /// <summary>
    ///  Returns the current playhead time in miliseconds. 
    /// </summary> 
    public static int timelinePosition;

    /// <summary>
    /// A float that returns the song's bpm.  
    /// </summary> 
    public static float bpm;

    /// <summary>
    ///  Returns the amount of seconds between each beat. 
    /// </summary>     
    public static float secPerBeat;

    /// <summary>
    ///  Returns the current song time in beats. 
    /// </summary> 
    public static float songPosInBeats;

    /// <summary>
    ///  Returns the current marker time in miliseconds. 
    /// </summary>    
    public static float markerTimeLinePosition;

    public static FMOD.Studio.EventInstance _instance;

    /// <summary>
    ///  Returns the current playhead time in seconds. 
    /// </summary>
    public static float Time { get { return timelinePosition / 1000f; } }

    /// <summary>
    /// Assigns an FMOD.Studio.EventInstance Beat Event to BeatSystem. 
    /// </summary>
    /// <param name="instance"> And FMOD.Studio.EventInstance.</param>
    public void AssignBeatEvent(FMOD.Studio.EventInstance instance)
    {
        _instance = instance;
        timelineInfo = new TimelineInfo();
        timelineHandle = GCHandle.Alloc(timelineInfo, GCHandleType.Pinned);
        beatCallback = new FMOD.Studio.EVENT_CALLBACK(BeatEventCallback);
        instance.setUserData(GCHandle.ToIntPtr(timelineHandle));
        instance.setCallback(beatCallback, FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_BEAT | FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_MARKER);
    }


    /// <summary>
    ///  Stops the instance from playing any more sound, and releases the instance from the timelineHandle. 
    /// </summary>
    public void StopAndClear(FMOD.Studio.EventInstance instance)
    {
        instance.setUserData(IntPtr.Zero);
        instance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        instance.release();
        timelineHandle.Free();
    }

    public void Pause(FMOD.Studio.EventInstance instance)
    {
        instance.setPaused(true);
    }

    public void Resume(FMOD.Studio.EventInstance instance)
    {
        instance.setPaused(false);
    }


    private void Update()
    {
        _instance.getTimelinePosition(out timelinePosition);
        songPosInBeats = Time / secPerBeat;
    }


    [AOT.MonoPInvokeCallback(typeof(FMOD.Studio.EVENT_CALLBACK))]
    static FMOD.RESULT BeatEventCallback(FMOD.Studio.EVENT_CALLBACK_TYPE type, IntPtr instancePtr, IntPtr parameterPtr)
    {
        FMOD.Studio.EventInstance instance = new FMOD.Studio.EventInstance(instancePtr);
        // Retrieve the user data
        IntPtr timelineInfoPtr;
        FMOD.RESULT result = instance.getUserData(out timelineInfoPtr);
        if (result != FMOD.RESULT.OK)
        {
            Debug.LogError("Timeline Callback error: " + result);
        }

        else if (timelineInfoPtr != IntPtr.Zero)
        {
            // Get the object to store beat and marker details
            GCHandle timelineHandle = GCHandle.FromIntPtr(timelineInfoPtr);
            TimelineInfo timelineInfo = (TimelineInfo)timelineHandle.Target;

            switch (type)
            {
                case FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_BEAT:
                    {
                        var parameter = (FMOD.Studio.TIMELINE_BEAT_PROPERTIES)Marshal.PtrToStructure(parameterPtr, typeof(FMOD.Studio.TIMELINE_BEAT_PROPERTIES));
                        timelineInfo.currentMusicBeat = parameter.beat;
                        beat = timelineInfo.currentMusicBeat;
                        beatIndex += 1;

                        bpm = parameter.tempo;
                        secPerBeat = 60f / bpm;


                        // Event firing for on beat events. 
                        if (OnBeat != null)
                        {
                            OnBeat();
                        }

                        // Event firing for every other beat. 
                        if (beat % 2 != 0)
                        {
                            if (OnOtherBeat != null)
                            {
                                OnOtherBeat();
                            }
                        }
                    }
                    break;

                case FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_MARKER:
                    {
                        var parameter = (FMOD.Studio.TIMELINE_MARKER_PROPERTIES)Marshal.PtrToStructure(parameterPtr, typeof(FMOD.Studio.TIMELINE_MARKER_PROPERTIES));
                        timelineInfo.lastMarker = parameter.name;
                        marker = timelineInfo.lastMarker;


                        markerTimeLinePosition = timelinePosition;

                        // Event firing for OnMarker events. 
                        OnMarker?.Invoke();
                    }
                    break;

            }

        }
        return FMOD.RESULT.OK;
    }
}