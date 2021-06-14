using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// In every intersection, chooses whether to turn or to keep going straight. 
/// </summary>
public class RandomSolver : Solver
{
    /// <summary>
    /// Chooses a random direction among the possible ones.
    /// </summary>
    override protected void FindPath(Vector2 playerPos)
    {
        // if it cannot go anywhere else, it goes back again
        if(possibleDirections_.Count == 0)
        {
            possibleDirections_.Add(-transform.forward);
        }

        // chooses a random direction among the possible ones
        int index = Random.Range(0, possibleDirections_.Count);
        direction_ = possibleDirections_[index];
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
