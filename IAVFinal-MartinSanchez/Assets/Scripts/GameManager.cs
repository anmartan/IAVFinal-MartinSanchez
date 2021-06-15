using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// struct to save the info about the level:
/// the size of the maze (understood as the length of any side)
/// and the maze itself, understood as a char array
/// </summary>
public struct level
{
    public uint size_;
    public char[,] map_;
}

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject player_;
    [SerializeField] public string filePath_ = "";     // where the map is saved (the file to be read)

    static private GameManager instance_;
    public enum Solvers { Player = 0, Random, LeftWall, RightWall, Tremaux, LeftPledge, RightPledge };


    level level_;
    bool moving = true;

    /// <summary>
    /// Reads the file where the level is saved. It is then saved in map_, so it can be accessed from other scripts.
    /// </summary>
    void ReadLevel()
    {
        StreamReader file = new StreamReader(Application.dataPath + "/Maps/" + filePath_);
        level_.size_ = uint.Parse(file.ReadLine());
        level_.map_ = new char[level_.size_, level_.size_];

        // initialize the map
        string line = "";
        for (int i = 0; i < level_.size_; i++)
        {
            line = file.ReadLine();
            for (int j = 0; j < level_.size_; j++)
            {
                level_.map_[i, j] = line[j];
            }
        }
    }

    public void VisitCell(Vector2 pos)
    {
        GetComponent<PathTracker>().VisitCell(pos);
    }

    void Awake()
    {
        if (!instance_)
            instance_ = this;
        else
            Destroy(this);

        ReadLevel();
    }

    private void Start()
    {
        Vector2 pos = GetComponent<MazeCreator>().CreateMaze();
        int x = Mathf.RoundToInt(pos.x);
        int y = Mathf.RoundToInt(pos.y);
        level_.map_[x, y] = MazeCreator.WALL_CHAR;    // so it is ignored when taking decisions

        GetComponent<PathTracker>().CreateMap();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            moving = !moving;

            player_.GetComponent<PlayerController>().enabled = moving;
            player_.GetComponent<HandFollowerSolver>().enabled = !moving;

        }
        else if(Input.GetKeyDown(KeyCode.Escape))
        {
            GetComponent<PathTracker>().VisitCell(new Vector2(0, 0));
        }
    }

    static public GameManager instance() { return instance_; }
    public GameObject GetPlayer() { return player_; }
    public level GetLevel() { return level_; }

    /// <summary>
    /// Calculates where an object is, according to the position given.
    /// Returns the center of the cell in the world, according to the WORLD_SCALE
    /// </summary>
    /// <param name="pos">The position in the map_</param>
    /// <returns>The position in the world</returns>
    public Vector3 GetWorldPosition(Vector2 pos)
    {
        Vector3 position = new Vector3(pos.y, 0, -pos.x);
        position *= MazeCreator.WORLD_SCALE;
        position.x += MazeCreator.WORLD_SCALE * 0.5f;       // gives the center of the cell
        position.z -= MazeCreator.WORLD_SCALE * 0.5f;       // gives the center of the cell
        return position;
    }

    /// <summary>
    /// Calculates the position in the map_, given a position in the scene.
    /// </summary>
    /// <param name="pos">The position in the scene.</param>
    /// <returns>The position in the map_, as (row, column).</returns>
    public Vector2 GetMapPosition(Vector3 pos)
    {
        return new Vector2(-Mathf.RoundToInt(pos.z / MazeCreator.WORLD_SCALE),  // x
                            Mathf.RoundToInt(pos.x / MazeCreator.WORLD_SCALE)); // y
    }

}
