using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Creates a maze from a given bitmap.
/// Reads a file.map and substitutes FLOOR_CHAR by floor cells, and WALL_CHAR by walls.
/// </summary>
public class MazeCreator : MonoBehaviour
{
    [SerializeField] private GameObject[] wallPrefab_;
    [SerializeField] private GameObject playerPrefab_;
    [SerializeField] private GameObject floorPrefab_;
    [SerializeField] private GameObject signPrefab_;
    [SerializeField] private Transform parent_;         // parent of every gameobject that will be created. This keeps the inspector cleaner

    readonly Vector2[] DIRECTIONS = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
    public const float WORLD_SCALE = 1;
    public const char WALL_CHAR = '#';
    public const char FLOOR_CHAR = ' ';

    level level_;
    
    /// <summary>
    /// Used to create the maze.
    /// Gets a level and transforms it into a level in the scene.
    /// </summary>
    /// <returns>The entrance of the map, so it is ignored when taking decisions</returns>
    public Vector2 CreateMaze()
    {
        level_ = GameManager.instance().GetLevel();

        // instantiates a single floor
        GameObject floor = Instantiate(floorPrefab_, new Vector3(level_.size_ * 0.5f * WORLD_SCALE, 0, -level_.size_ * 0.5f * WORLD_SCALE), Quaternion.identity, parent_);
        floor.transform.localScale = floor.transform.localScale * WORLD_SCALE * level_.size_;

        // instantiates all the walls that should be created
        for (int i = 0; i < level_.size_; i++)
        {
            for (int j = 0; j < level_.size_; j++)
            {
                Vector3 pos = new Vector3(j * WORLD_SCALE, 0, -i * WORLD_SCALE);
                int tree = Random.Range(0, wallPrefab_.Length);
                pos.x += 0.5f * wallPrefab_[tree].transform.lossyScale.x;
                pos.y += 0.5f * wallPrefab_[tree].transform.lossyScale.y;
                pos.z -= 0.5f * wallPrefab_[tree].transform.lossyScale.z;
                if (level_.map_[i, j] == WALL_CHAR)
                {
                    // if there is a wall in this position, it is created
                    Instantiate(wallPrefab_[tree], pos, Quaternion.identity, parent_);
                }
            }
        }

        // finds the position where the player shall startz
        bool player = false;
        int x = 0;
        while (x < level_.size_ && !player)
        {
            player = (level_.map_[level_.size_-1, x] == FLOOR_CHAR);
            if (!player) x++;
        }

        // creates a post sign at the entrance
        //Vector3 pos = new Vector3(x * WORLD_SCALE, 0, -(level_.size_ - 1) * WORLD_SCALE);
        //pos.x += 0.5f * signPrefab_.transform.lossyScale.x;
        //pos.z -= 0.5f * signPrefab_.transform.lossyScale.z;
        //Instantiate(signPrefab_, pos, Quaternion.identity, parent_);

        // creates the player in that position, inside the maze
        Vector3 playerPos = new Vector3(x * WORLD_SCALE, 0, -(level_.size_ - 2) * WORLD_SCALE);
        playerPos.x += 0.5f * playerPrefab_.transform.lossyScale.x;
        playerPos.z -= 0.5f * playerPrefab_.transform.lossyScale.z;

        GameManager.instance().GetPlayer().transform.position = playerPos;
        return new Vector2(level_.size_ - 1, x);
    }
}