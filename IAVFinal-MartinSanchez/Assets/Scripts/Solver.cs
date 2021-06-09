using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface
/// The algorithms used for solving the maze will implement this interface.
/// </summary>
public class Solver : MonoBehaviour
{
    protected const float DISTANCE_TO_COLLIDE = MazeCreator.WORLD_SCALE;
    protected const char WALL_CHAR = MazeCreator.WALL_CHAR;

    protected Rigidbody rigidbody_;                 

    protected List<Vector3> possibleDirections_;    // possible directions to take when calculating a path
    protected Vector2 lastPositionChanged_;         // so it doesnt change the path every frame
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

        // if there is no wall in such position, no further calculation is needed
        if (level_.map_[Mathf.RoundToInt(wallPos.x), Mathf.RoundToInt(wallPos.y)] != WALL_CHAR)
            return false;

        // checks if the character is too close to the wall 
        Vector3 wallCell = GetWorldPosition(wallPos);
        return (transform.position - wallCell).magnitude <= DISTANCE_TO_COLLIDE;
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

            // if there is a wall in such position, that is not a possible turn
            if (level_.map_[x, y] != WALL_CHAR)
                possibleDirections_.Add(dir);
        }

        // if there are some possible directions, there is an intersection
        return possibleDirections_.Count != 0;
    }

    /// <summary>
    /// Implemented by each solver algorithm. 
    /// Finds the next direction that the character should follow.
    /// </summary>
    virtual protected void FindPath()
    {
        Debug.LogError("FindPath not implemented");
    }

    /// <summary>
    /// Calculates where an object is, according to the position given.
    /// Returns the center of the cell in the world, according to the WORLD_SCALE
    /// </summary>
    /// <param name="pos">The position in the map_</param>
    /// <returns>The position in the world</returns>
    protected Vector3 GetWorldPosition(Vector2 pos)
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
    protected Vector2 GetMapPosition(Vector3 pos)
    {
        return new Vector2(-Mathf.RoundToInt(pos.z / MazeCreator.WORLD_SCALE),  // x
                            Mathf.RoundToInt(pos.x / MazeCreator.WORLD_SCALE)); // y
    }

    /// <summary>
    /// Lets the character move until it has found the center of the cell, and can therefore, change its path
    /// </summary>
    /// <param name="playerPos"></param>
    protected void Move(Vector2 playerPos)
    {
        Vector3 center = GetWorldPosition(playerPos);
        float distance = (transform.position - center).magnitude;

        // if it is needed, a new path is calculated
        if (needUpdate_ && distance < MazeCreator.WORLD_SCALE * 0.25f)
        {
            FindPath();
            needUpdate_ = false;
            lastPositionChanged_ = playerPos;
        }
    }



    protected virtual void Start()
    {
        rigidbody_ = GetComponent<Rigidbody>();
    }
    
    protected virtual void Update()
    {
        Vector2 playerPos = GetMapPosition(transform.position);
        if (playerPos != lastPositionChanged_)
        {
            // if the location calculated is a wall and the player is close to it
            if (CheckCollisions(playerPos))
                needUpdate_ = true;

            // checks if there are intersections
            if (CheckIntersections(playerPos))
                needUpdate_ = true;
        }

        Move(playerPos);
    }

    protected virtual void FixedUpdate()
    {
        // the velocity is set according to what has been previously calculated
        rigidbody_.velocity = direction_ * PlayerController.PLAYER_SPEED;
    }

    protected virtual void LateUpdate()
    {
        // change the lookat so that the character faces its destination
        transform.LookAt(transform.position + direction_);
    }

    protected virtual void OnEnable()
    {
        level_ = GameManager.instance().GetLevel();
        possibleDirections_ = new List<Vector3>();

        // when enabled, a path must be calculated
        Vector2 playerPos = GetMapPosition(transform.position);
        CheckCollisions(playerPos);
        CheckIntersections(playerPos);

        FindPath();
    }
}
