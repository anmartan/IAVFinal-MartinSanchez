using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface
/// The algorithms used for solving the maze will implement this interface.
/// </summary>
public class Solver : MonoBehaviour
{
    protected Rigidbody rigidbody_;
    protected Vector3 direction_;
    //protected long cellsVisited_;
    //protected float timeToSolve_;

    virtual protected void FindPath() { }
}
