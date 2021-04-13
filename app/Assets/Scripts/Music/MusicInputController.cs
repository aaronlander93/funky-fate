using UnityEngine;

public class MusicInputController : MonoBehaviour
{
    public KeyCode musicAttack;

    public delegate void PlayerInput();
    public static event PlayerInput ActionOnePressed;

    // TODO: Prevent key holding for multiple input. 
    void OnGUI()
    {
        if (anonymousKeyDown(musicAttack))
        {
            ActionOnePressed?.Invoke();
        }
    }

    private bool anonymousKeyDown(KeyCode key)
    {
        if (Event.current.type == EventType.KeyDown)
            return (Event.current.keyCode == key);
        return false;
    }
}