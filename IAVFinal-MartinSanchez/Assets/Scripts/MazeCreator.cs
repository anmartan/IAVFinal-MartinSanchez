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


    level level_;
    
    /// <summary>
    /// Used to create the maze.
    /// Gets a level and transforms it into a level in the scene.
    /// </summary>
    /// <returns>The entrance of the map, so it is ignored when taking decisions</returns>
    public Vector2 CreateMaze()
    {
        Vector3 playerPos = new Vector3();

        level_ = GameManager.instance().GetLevel();

        // instantiates a single floor
        GameObject floor = Instantiate(floorPrefab_, new Vector3(level_.size_ * 0.5f * Configuration.WORLD_SCALE, 0, -level_.size_ * 0.5f * Configuration.WORLD_SCALE), Quaternion.identity, parent_);
        floor.transform.localScale = floor.transform.localScale * Configuration.WORLD_SCALE * level_.size_;

        // instantiates all the walls that should be created
        for (int i = 0; i < level_.size_; i++)
        {
            for (int j = 0; j < level_.size_; j++)
            {
                Vector3 pos = new Vector3(j * Configuration.WORLD_SCALE, 0, -i * Configuration.WORLD_SCALE);

                // if there is a wall in this position, it is created
                if (level_.map_[i, j] == Configuration.WALL_CHAR)
                {
                    int tree = Random.Range(0, wallPrefab_.Length);
                    pos.x += 0.5f * wallPrefab_[tree].transform.lossyScale.x;
                    pos.y += 0.5f * wallPrefab_[tree].transform.lossyScale.y;
                    pos.z -= 0.5f * wallPrefab_[tree].transform.lossyScale.z;

                    Instantiate(wallPrefab_[tree], pos, Quaternion.identity, parent_);
                }
                else if(level_.map_[i,j] == Configuration.PLAYER_CHAR)
                {
                    playerPos = pos;

                    // if its in the end of the maze, puts it a tile closer to the center
                    if (i == level_.size_ - 1)
                        playerPos.z += Configuration.WORLD_SCALE;

                    // transform it into a wall so it is ignored when calculating a path
                    level_.map_[i, j] = Configuration.WALL_CHAR;
                }
            }
        }

        // creates the player in that position, inside the maze
        playerPos.x += 0.5f * playerPrefab_.transform.lossyScale.x;
        playerPos.z -= 0.5f * playerPrefab_.transform.lossyScale.z;

        GameManager.instance().GetPlayer().transform.position = playerPos;


        // creates a post sign at the entrance
        Vector3 signPos = playerPos;
        signPos.x += 0.5f * signPrefab_.transform.lossyScale.x;
        signPos.z -= (0.5f * signPrefab_.transform.lossyScale.z) + Configuration.WORLD_SCALE;
        Instantiate(signPrefab_, signPos, Quaternion.identity, parent_);

        return new Vector2(-playerPos.z + 1, playerPos.x);
    }
}