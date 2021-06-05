using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface
/// The algorithms used for solving the maze will implement this interface.
/// </summary>
public class Solver : MonoBehaviour
{
    protected Rigidbody rigidbody;
    protected Vector3 direction;
    virtual protected void findPath() { }

    virtual protected void OnEnable()
    {
        findPath();
    }
}
