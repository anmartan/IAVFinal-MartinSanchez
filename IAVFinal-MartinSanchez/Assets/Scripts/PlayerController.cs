using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    public const float PLAYER_SPEED = 5f;

    private Rigidbody rigidbody_;    
    private Vector3 direction_;      // where the character is headed to

    void Start()
    {
        rigidbody_ = GetComponent<Rigidbody>();
    }

    // the direction is calculated
    void Update()
    {
        // if the player is encharged of moving the character, the input is read
        direction_.x = Input.GetAxis("Horizontal");
        direction_.z = Input.GetAxis("Vertical");

        // the direction depends on the player speed
        direction_ *= PLAYER_SPEED;
    }

    private void FixedUpdate()
    {
        // the velocity is set according to what has been previously calculated
        rigidbody_.velocity = direction_;
    }


    private void LateUpdate()
    {
        // change the lookat so that the character faces its destination
        transform.LookAt(transform.position + direction_);
    }
    private void OnDisable()
    {
        rigidbody_.velocity = new Vector3(0, 0, 0);
    }
}
