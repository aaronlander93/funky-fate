using UnityEngine;

public class MusicInputController : MonoBehaviour
{
    public KeyCode musicAttack;

    public delegate void PlayerInput();
    public static event PlayerInput ActionOnePressed;
    public static event PlayerInput ActionOneReleased;

    // TODO: Prevent key holding for multiple input. 
    void OnGUI()
    {
        if (anonymousKeyDown(musicAttack))
        {
            ActionOnePressed?.Invoke();
        }
        else if (anonymousKeyUp(musicAttack))
        {
            ActionOneReleased?.Invoke();
        }
    }

    private bool anonymousKeyDown(KeyCode key)
    {
        if (Event.current.type == EventType.KeyDown)
            return (Event.current.keyCode == key);
        return false;
    }

    private bool anonymousKeyUp(KeyCode key)
    {
        if (Event.current.type == EventType.KeyUp)
            return (Event.current.keyCode == key);
        return false;
    }
}