using System.IO;
using UnityEngine.SceneManagement;
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
    [SerializeField] private GameObject player_;
    static private GameManager instance_;

    private Configuration.Solvers algorithm_;

    private level level_;

    /// <summary>
    /// Reads the file where the level is saved. It is then saved in map_, so it can be accessed from other scripts.
    /// </summary>
    void ReadLevel()
    {
        // reads the configuration
        StreamReader file = new StreamReader(Application.dataPath + "/Maps/" + Configuration.CONFIG_FILE_);
        algorithm_ = (Configuration.Solvers)int.Parse(file.ReadLine());
        string filePath = file.ReadLine();
        file.Close();

        // reads the map specified in configuration
        file = new StreamReader(Application.dataPath + "/Maps/" + filePath);
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

    private void SetPlayerSolver()
    {
        switch(algorithm_)
        {
            case Configuration.Solvers.Random:
                player_.AddComponent<RandomSolver>();
                break;
            case Configuration.Solvers.RightWall:
                HandFollowerSolver rightFollow = player_.AddComponent<HandFollowerSolver>();
                rightFollow.SetRightHand(true);
                break;
            case Configuration.Solvers.LeftWall:
                HandFollowerSolver leftFollow = player_.AddComponent<HandFollowerSolver>();
                leftFollow.SetRightHand(false);
                break;
            case Configuration.Solvers.Tremaux:
                player_.AddComponent<TremauxSolver>();
                break;
            case Configuration.Solvers.RightPledge:
                PledgeSolver rightPledger = player_.AddComponent<PledgeSolver>();
                rightPledger.SetRightHand(true);
                //rightPledger.RandomizeFavoriteDirection();
                break;
            case Configuration.Solvers.LeftPledge:
                PledgeSolver leftPledger = player_.AddComponent<PledgeSolver>();
                leftPledger.SetRightHand(false);
                //leftPledger.RandomizeFavoriteDirection();
                break;
            default:
                player_.AddComponent<PlayerController>();
                break;
        }
    }


    /// <summary>
    /// Calls the VisitCell of its PathTracker component.
    /// </summary>
    /// <param name="pos">The position of the tile that is visited.</param>
    public void VisitCell(Vector2 pos)
    {
        GetComponent<PathTracker>().VisitCell(pos);
    }

    public void FinishLevel()
    {
        SceneManager.LoadScene("MainMenu");
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
        level_.map_[x, y] = Configuration.WALL_CHAR;    // so it is ignored when taking decisions

        GetComponent<PathTracker>().CreateMap();

        // sets the player solver
        SetPlayerSolver();
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
        position *= Configuration.WORLD_SCALE;
        position.x += Configuration.WORLD_SCALE * 0.5f;       // gives the center of the cell
        position.z -= Configuration.WORLD_SCALE * 0.5f;       // gives the center of the cell
        return position;
    }

    /// <summary>
    /// Calculates the position in the map_, given a position in the scene.
    /// </summary>
    /// <param name="pos">The position in the scene.</param>
    /// <returns>The position in the map_, as (row, column).</returns>
    public Vector2 GetMapPosition(Vector3 pos)
    {
        return new Vector2(-Mathf.Ceil(pos.z / Configuration.WORLD_SCALE),  // x
                            Mathf.Floor(pos.x / Configuration.WORLD_SCALE)); // y
    }
}
