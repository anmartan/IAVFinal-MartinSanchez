using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Drawing;

/// <summary>
/// Creates a maze from a given bitmap.
/// Reads a file.map and substitutes FLOOR_CHAR by floor cells, and WALL_CHAR by walls.
/// </summary>
public class MazeCreator : MonoBehaviour
{
    readonly Vector2[] DIRECTIONS = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
    public const float WORLD_SCALE = 1;
    const char WALL_CHAR = '#';
    const char FLOOR_CHAR = ' ';


    [SerializeField] public string filePath_ = "";     // where the map is saved (the file to be read)
    [SerializeField] private Transform parent_;         // parent of every gameobject that will be created. This keeps the inspector cleaner
    [SerializeField] private GameObject wallPrefab_;
    [SerializeField] private GameObject floorPrefab_;
    [SerializeField] private GameObject playerPrefab_;

    /// <summary>
    /// Used to create the maze. 
    /// Reads a file and transforms it into a world scene
    /// </summary>
    public void CreateMaze()
    {
        filePath_ = Application.dataPath + "/Maps/" + filePath_;   // where the maps are saved
        StreamReader file = new StreamReader(filePath_);
        uint size_ = uint.Parse(file.ReadLine());

        // instantiates a single floor
        GameObject floor = Instantiate(floorPrefab_, new Vector3(size_ * 0.5f * WORLD_SCALE, 0, -size_ * 0.5f * WORLD_SCALE), Quaternion.identity, parent_);
        floor.transform.localScale = floor.transform.localScale * WORLD_SCALE * size_;

        string line = "";
        // instantiates all the walls that should be created
        for (int i = 0; i < size_; i++)
        {
            line = file.ReadLine();
            for (int j = 0; j < size_; j++)
            {
                Vector3 pos = new Vector3(j * WORLD_SCALE, 0, - i * WORLD_SCALE);
                pos.x += 0.5f * wallPrefab_.transform.lossyScale.x;
                pos.z -= 0.5f * wallPrefab_.transform.lossyScale.z;
                if (line[j] == WALL_CHAR)
                {
                    // if there is a wall in this position, it is created
                    Instantiate(wallPrefab_, pos, Quaternion.identity, parent_);
                }
            }
        }

        // finds the position where the player shall start
        bool player = false;
        int x = 0;
        while (x < size_ && !player) 
        {
            player = (line[x] == FLOOR_CHAR);
            if (!player) x++;
        }

        // creates the player in that position
        Vector3 playerPos = new Vector3(x * WORLD_SCALE, 0,  - (size_ - 1) * WORLD_SCALE);
        playerPos.x += 0.5f * playerPrefab_.transform.lossyScale.x;
        playerPos.z -= 0.5f * playerPrefab_.transform.lossyScale.z;

        //Instantiate(playerPrefab_, playerPos, Quaternion.identity, parent_);
        GameManager.instance().GetPlayer().transform.position = playerPos;
    }
}
