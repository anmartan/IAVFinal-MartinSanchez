using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Improvement of the Hand Follower Algorithm.
/// This algorithm can help jumping from an aisle to another, so that the character can get out of a complex maze,
/// where the exit is on an outer "ring" than the player is initially on.
/// It doesnt work the other way round: when the exit is on an inner "ring" than the player is initially on.
/// </summary>
public class PledgeSolver : Solver
{
    [SerializeField] private Vector3 favoriteDirection_;
    [SerializeField] private bool rightWall_;

    private bool followingWall_ = false;
    private int turns_ = 0;

    /// <summary>
    /// As long as there are no obstacles, the character will follow its favoriteDirection_.
    /// When facing an obstacle, the character will start following the wall (right or left, as it is specified), until two conditions are reached:
    /// - The character is facing its favoriteDirection_ again.
    /// - The number of turns taken equals 0 (0 != 360)
    /// 
    /// If the right walls are followed, a clockwise turn is considered positive; a counter-clockwise turn is negative.
    /// If the left wall are followed, it is the other way round.
    /// </summary>
    /// <param name="playerPos"></param>
    override protected void FindPath(Vector2 playerPos)
    {
        // if it cannot go forwards, follows the wall
        if(!followingWall_)
        {
            int xA = Mathf.RoundToInt(favoriteDirection_.x);
            int zA = Mathf.RoundToInt(favoriteDirection_.z);

            int i = 0;
            bool found = false;
            while (i < possibleDirections_.Count && !found) 
            {
                int xB = Mathf.RoundToInt(possibleDirections_[i].x);
                int zB = Mathf.RoundToInt(possibleDirections_[i].z);

                found = (xA == xB && zA == zB);
                if (!found) i++;
            }

            followingWall_ = !found;
        }

        // follows its "nose" until it hits a wall
        if (!followingWall_)
        {
            direction_ = favoriteDirection_;
            turns_ = 0;
        }
        // similar to the Hand Follower Solver
        else
        {
            // if it cannot go anywhere else, it goes back again
            if (possibleDirections_.Count == 0)
            {
                possibleDirections_.Add(-transform.forward);

                // when turning 180 degrees, there is always two negative turns
                turns_ -= 2;
            }

            // the possibleDirections are added from left to right, always
            // therefore, if the left wall should be chosen, the direction is always the first available
            // on the other hand, if the right wall is chosen, the direction is always the last one added to the list
            int index = 0;
            if (rightWall_) index = possibleDirections_.Count - 1;


            // checks if the turns_ need to be updated
            if (possibleDirections_[index] != direction_) 
            {
                if (rightWall_)
                    turns_ += possibleDirections_[index] == transform.right ? 1 : -1;
                else
                    turns_ += possibleDirections_[index] == transform.right ? -1 : 1;
            }

            direction_ = possibleDirections_[index];


            // if turns_ is 0, stops following the walls
            followingWall_ = (turns_ != 0);
        }
            Debug.Log(turns_);
    }



    //------ Unity functions. All of them call the base method, defined in Solver.cs ------//

    protected override void Start()
    {
        base.Start();
    }
    protected override void Update()
    {
        base.Update();
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
    protected override void LateUpdate()
    {
        base.LateUpdate();
    }
    protected override void OnEnable()
    {
        base.OnEnable();
    }
}
