using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// In every intersection, chooses whether to turn or to keep going straight. 
/// </summary>
public class RandomSolver : Solver
{
    const float DISTANCE_TO_COLLIDE = MazeCreator.WORLD_SCALE;
    const char WALL_CHAR = MazeCreator.WALL_CHAR;

    List<Vector3> possibleDirections_ = new List<Vector3>();    // possible directions to take when calculating a path
    Vector2 lastPositionChanged_ = new Vector2();               // so it doesnt change the path every frame
    bool needUpdate_ = false;
    level level_;


    /// <summary>
    /// Checks if the character is about to collide with a wall in front of them.
    /// </summary>
    /// <param name="playerPos">Where the player is in the map_(row, column).</param>
    /// <returns>true if the character will soon collide with a wall; false otherwise.</returns>
    private bool CheckCollisions(Vector2 playerPos)
    {
        Vector2 wallPos = playerPos;
        wallPos.x -= transform.forward.z;     // to see where the player is going to
        wallPos.y += transform.forward.x;     // and check if there is a wall in front of them

        // if there is no wall in such position, no further calculation is needed
        if (level_.map_[(int)wallPos.x, (int)wallPos.y] != WALL_CHAR) 
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
    private bool CheckIntersections(Vector2 playerPos)
    {
        // clear the possible directions, until a new direction must be calculated
        possibleDirections_.Clear();

        //RaycastHit raycast;
        Vector3 originalDir = Quaternion.Euler(0, -90, 0) * transform.forward;

        //// checks if it is possible to turn left or right or to keep forwards
        for (int i = 0; i < 3; i++)
        {
            Vector3 dir = Quaternion.Euler(0, 90 * i, 0) * originalDir;

            Vector2 turnPos = playerPos;
            turnPos.x -= dir.z;     // to see if the player could go in that direction
            turnPos.y += dir.x;     // and check if there is a wall in front of them

            // if there is a wall in such position, that is not a possible turn
            if (level_.map_[(int)turnPos.x, (int)turnPos.y] != WALL_CHAR)
                possibleDirections_.Add(dir);
        }

        // if there is more than one possible direction, there is an intersection
        return possibleDirections_.Count > 1;
    }

    override protected void FindPath()
    {
        if(possibleDirections_.Count == 0)
        {
            // adds the possibility of going back again
            possibleDirections_.Add(-transform.forward);
        }

        // chooses a random direction among the possible ones
        int index = Random.Range(0, possibleDirections_.Count);
        direction_ = possibleDirections_[index];
    }

    /// <summary>
    /// Calculates where an object is, according to the position given.
    /// Returns the center of the cell in the world, according to the WORLD_SCALE
    /// </summary>
    /// <param name="pos">The position in the map_</param>
    /// <returns>The position in the world</returns>
    private Vector3 GetWorldPosition(Vector2 pos)
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
    private Vector2 GetMapPosition(Vector3 pos)
    {
        Vector2 aux = new Vector2(-(int)pos.z, (int)pos.x);
        pos.x = (int)(pos.x / MazeCreator.WORLD_SCALE);
        pos.y = (int)(pos.y / MazeCreator.WORLD_SCALE);

        Vector2 ret = aux;
        if (aux.x - ret.x > 0.5) ret.x++;
        if (aux.y - ret.y > 0.5) ret.y++;
        return ret;
    }

    /// <summary>
    /// Lets the character move until it has found the center of the cell, and can therefore, change its path
    /// </summary>
    /// <param name="playerPos"></param>
    private void Move(Vector2 playerPos)
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

    private void Start()
    {
        rigidbody_ = GetComponent<Rigidbody>();
    }

    private void Update()
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

    private void FixedUpdate()
    {
        // the velocity is set according to what has been previously calculated
        rigidbody_.velocity = direction_ * PlayerController.PLAYER_SPEED;
    }

    private void LateUpdate()
    {
        // change the lookat so that the character faces its destination
        transform.LookAt(transform.position + direction_);
    }

    private void OnEnable()
    {
        level_ = GameManager.instance().GetLevel();
        possibleDirections_.Clear();

        // when enabled, a path must be calculated
        Vector2 playerPos = GetMapPosition(transform.position);
        CheckCollisions(playerPos);
        CheckIntersections(playerPos);
        //Vector3 originalDir = Quaternion.Euler(0, -90, 0) * transform.forward;
        //for (int i = 0; i < 3; i++)
        //{
        //    Vector3 dir = Quaternion.Euler(0, 90 * i, 0) * originalDir;
        //    possibleDirections_.Add(dir);
        //}
        FindPath();
    }
}
