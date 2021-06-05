using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// In every intersection, chooses wether to turn or to keep going straight. 
/// </summary>
public class RandomSolver : Solver
{
    private float time_ = 0;
    private const float timeToUpdate_ = 2.5f;
    private const float distanceToCollide_ = 0.75f;
    private bool needUpdate_ = false;       // if there is going to be a collision with a wall, there needs to be an update


    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    virtual protected void findPath()
    {
        // chooses a random direction, and starts moving that way
        int nextDir = Random.Range(0, 4);
        
        switch(nextDir)
        {
            case 0:
                direction = new Vector3(-1, 0, 0);
                break;
            case 1:
                direction = new Vector3(1, 0, 0);
                break;
            case 2:
                direction = new Vector3(0, 0, -1);
                break;
            default:
                direction = new Vector3(0, 0, 1);
                break;
        }
        Debug.Log(direction);
    }

    private void Update()
    {
        // checks if the path should be updated before its time
        RaycastHit raycast;
        Physics.Raycast(transform.position, transform.forward, out raycast);
        Debug.DrawRay(transform.position, raycast.point - transform.position, Color.blue);

        // if there raycast collides, treats the collision
        if (raycast.collider && raycast.distance < distanceToCollide_ * MazeCreator.WORLD_SCALE)
        {
            needUpdate_ = true;
        }

        // every few seconds, the solver sets a new direction
        if (time_ >= timeToUpdate_ || needUpdate_)
        {
            findPath();
            time_ = 0;
            needUpdate_ = false;
        }
        time_ += Time.deltaTime;
    }
    private void FixedUpdate()
    {
        // the velocity is set according to what has been previously calculated
        rigidbody.velocity = direction;
    }

    private void OnEnable()
    {
        findPath();
    }
    private void LateUpdate()
    {
        // change the lookat so that the character faces its destination
        transform.LookAt(transform.position + direction);
    }
}
