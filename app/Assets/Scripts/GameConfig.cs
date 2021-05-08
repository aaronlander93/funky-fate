using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameConfig
{
    private static bool multiplayer = false;
    public static bool Multiplayer { get; set; }

    private static string nickname;
    public static string Nickname { get; set; }

    public static float MusicVolume { get; set; }  = 0.75f;

    public static float MasterVolume { get; set; } = 0.75f;
}
