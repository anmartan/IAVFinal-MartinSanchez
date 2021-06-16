using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Auxiliary script. Saves global variables and enums that will be used in different scenes
/// </summary>
public static class Configuration
{
    public enum Solvers { Player = 0, Random, LeftWall, RightWall, Tremaux, LeftPledge, RightPledge };
    public static readonly string[] MAZES_ = { "Braid.map", "Braid1.map", "Perfect.map", "Perfect1.map", "NoExit.map", "Complex.map" };

    public static string CONFIG_FILE_ = "config.cfg";       // the file that will save the configuration
    public static float PLAYER_SPEED_ = 2.5f;               // the speed the player will have

    public const float WORLD_SCALE = 1;
    public const char WALL_CHAR = '#';
    public const char PLAYER_CHAR = 'P';
    public const char EXIT_CHAR = 'E';
}
