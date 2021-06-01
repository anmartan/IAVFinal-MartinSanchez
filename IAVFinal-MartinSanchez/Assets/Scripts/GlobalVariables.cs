using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Auxiliar script, it does not add any kind of functionality
/// Used to save all the global variables in the same file
/// They are all const variables, and can be accessed by using a GlobalVariables.variable syntax.
/// </summary>
namespace IAVMazeSolver
{
    public static class GlobalVariables
    {
        public const float WORLD_SCALE = 1;
        public const char WALL_CHAR = '#';
        public const char FLOOR_CHAR = ' ';

        public static readonly Vector2[] DIRECTIONS = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

    }
}
