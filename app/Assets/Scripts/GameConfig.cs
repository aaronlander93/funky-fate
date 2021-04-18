using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameConfig
{
    private static bool multiplayer = false;
    public static bool Multiplayer { get; set; }

    private static string nickname;
    public static string Nickname { get; set; }
}
