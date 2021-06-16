using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Sticks to the selected wall and follows it until the exit is found.
/// Either the right wall or the left wall can be chosen.
/// </summary>
public class HandFollowerSolver : Solver
{
    [SerializeField] private bool rightWall_ = true;      //  whether it follows the right or left wall

    /// <summary>
    /// Depending on whether it is following the right or left wall, 
    /// the character chooses the direction it should follow.
    /// </summary>
    override protected void FindPath(Vector2 playerPos)
    {
        // if it cannot go anywhere else, it goes back again
        if (possibleDirections_.Count == 0)
        {
            possibleDirections_.Add(-transform.forward);
        }

        // the possibleDirections are added from left to right, always
        // therefore, if the left wall should be chosen, the direction is always the first available
        // on the other hand, if the right wall is chosen, the direction is always the last one added to the list
        int index = 0;
        if (rightWall_) index = possibleDirections_.Count - 1;
        direction_ = possibleDirections_[index];
    }

    public void SetRightHand(bool right) { rightWall_ = right; }


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
