using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface
/// The algorithms used for solving the maze will implement this interface.
/// </summary>
public class Solver : MonoBehaviour
{
    protected const float DISTANCE_TO_COLLIDE = Configuration.WORLD_SCALE;
    protected const char WALL_CHAR = Configuration.WALL_CHAR;

    protected Rigidbody rigidbody_;

    protected List<Vector3> possibleDirections_;    // possible directions to take when calculating a path
    protected Vector2 lastDirectionChanged_;        // so it doesnt change the path every frame
    protected Vector2 lastTileStepped_;             // so it can mark its path
    protected bool needUpdate_ = false;             // whether the direction followed must change or not
    protected Vector3 direction_;                   // the direction it follows on every update
    protected level level_;                         // information about the maze

    //protected long cellsVisited_;
    //protected float timeToSolve_;

    /// <summary>
    /// Checks if the character is about to collide with a wall in front of them.
    /// </summary>
    /// <param name="playerPos">Where the player is in the map_(row, column).</param>
    /// <returns>true if the character will soon collide with a wall; false otherwise.</returns>
    protected bool CheckCollisions(Vector2 playerPos)
    {
        Vector2 wallPos = playerPos;
        wallPos.x -= transform.forward.z;     // sees where the player is going to and
        wallPos.y += transform.forward.x;     // checks if there is a wall in front of them

        CheckIntersections(playerPos);  // checks the directions the character could take

        // checks if the character is too close to the wall 
        Vector3 wallCell = GameManager.instance().GetWorldPosition(wallPos);
        return (transform.position - wallCell).magnitude <= DISTANCE_TO_COLLIDE || possibleDirections_.Count > 0;
    }

    /// <summary>
    /// Checks if the character is at an intersection of roads.
    /// Checks for intersections in the form of a cross, a corner or a T (in any of its variants).
    /// Does not check for hallways (where the character can go forwards or backwards).
    /// </summary>
    /// <param name="playerPos">Where the player is in the map_(row, column).</param>
    /// <returns>true if the character is at an intersection; false otherwise.</returns>
    protected bool CheckIntersections(Vector2 playerPos)
    {
        // clears the possible directions
        possibleDirections_.Clear();

        Vector3 originalDir = Quaternion.Euler(0, -90, 0) * transform.forward;

        // checks if it is possible to turn left or right or to keep forwards
        for (int i = 0; i < 3; i++)
        {
            Vector3 dir = (Quaternion.Euler(0, 90 * i, 0) * originalDir).normalized;

            int x = Mathf.RoundToInt(playerPos.x - dir.z);
            int y = Mathf.RoundToInt(playerPos.y + dir.x);

            // if the player tries to go outside the array bounds (the exit), that is a possible turn; it is added automatically
            try
            {
                // if there is a wall in such position, that is not a possible turn
                if (level_.map_[x, y] != WALL_CHAR)
                    possibleDirections_.Add(dir);
            }
            catch   { possibleDirections_.Add(dir); }
        }

        // if there are some possible directions, there is an intersection
        return possibleDirections_.Count > 1;
    }

    /// <summary>
    /// Implemented by each solver algorithm. 
    /// Finds the next direction that the character should follow.
    /// </summary>
    virtual protected void FindPath(Vector2 playerPos)
    {
        Debug.LogError("FindPath not implemented");
    }


    /// <summary>
    /// Lets the character move until it has found the center of the cell, and can therefore, change its path.
    /// If its changed the tile its on, marks it
    /// </summary>
    /// <param name="playerPos"></param>
    protected void Move(Vector2 playerPos)
    {
        Vector3 center = GameManager.instance().GetWorldPosition(playerPos);
        float distance = (transform.position - center).magnitude;

        // if it is needed, a new path is calculated
        if (needUpdate_ && distance < Configuration.WORLD_SCALE * 0.25f)
        {
            FindPath(playerPos);
            needUpdate_ = false;
            lastDirectionChanged_ = playerPos;
        }
        if(lastTileStepped_ != playerPos)
        {
            GameManager.instance().VisitCell(playerPos);
            lastTileStepped_ = playerPos;
        }
    }


    //------------------------- Unity functions -------------------------//

    protected virtual void Start()
    {
        rigidbody_ = GetComponent<Rigidbody>();
        level_ = GameManager.instance().GetLevel();
    }
    
    protected virtual void Update()
    {
        Vector2 playerPos = GameManager.instance().GetMapPosition(transform.position);
        int x = Mathf.RoundToInt(playerPos.x);
        int y = Mathf.RoundToInt(playerPos.y);

        // if its arrived to the exit, goes back to the main menu
        bool finish = (level_.map_[x, y] == Configuration.EXIT_CHAR);

        if (playerPos != lastDirectionChanged_)
        {
            // if the location calculated is a wall and the player is close to it
            if (CheckCollisions(playerPos))
                needUpdate_ = true;

            // checks if there are intersections
            if (CheckIntersections(playerPos))
                needUpdate_ = true;
        }

        // moves the character
        Move(playerPos);

        if (finish)
            GameManager.instance().FinishLevel();
    }

    protected virtual void FixedUpdate()
    {
        // the velocity is set according to what has been previously calculated
        rigidbody_.velocity = direction_ * Configuration.PLAYER_SPEED_;
    }

    protected virtual void LateUpdate()
    {
        // change the lookat so that the character faces its destination
        transform.LookAt(transform.position + direction_);
    }

    protected virtual void OnEnable()
    {
        possibleDirections_ = new List<Vector3>();
    }
}
