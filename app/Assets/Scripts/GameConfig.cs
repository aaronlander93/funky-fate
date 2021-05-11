using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameConfig
{
    public static bool Multiplayer { get; set; }

    public static string Nickname { get; set; }

    public static int PlayerColor { get; set; } = -1;

    public static float MusicVolume { get; set; } = 0.75f;

    public static float MasterVolume { get; set; } = 0.75f;
}
