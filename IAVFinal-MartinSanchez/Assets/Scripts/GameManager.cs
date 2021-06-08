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

    static public GameManager instance_;


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


    void Awake()
    {
        if (!instance_)
            instance_ = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        ReadLevel();
        GetComponent<MazeCreator>().CreateMaze();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            moving = !moving;

            player_.GetComponent<PlayerController>().enabled = moving;
            player_.GetComponent<RandomSolver>().enabled = !moving;

        }
    }


    static public GameManager instance() { return instance_; }
    public GameObject GetPlayer() { return player_; }
    public level GetLevel() { return level_; }
}
