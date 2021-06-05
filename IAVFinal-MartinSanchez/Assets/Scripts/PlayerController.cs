using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    const float PLAYER_SPEED = 5f;

    private Rigidbody rigidbody;    
    private Vector3 direction;      // where the character is headed to

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // the direction is calculated
    void Update()
    {
        // if the player is encharged of moving the character, the input is read
        direction.x = Input.GetAxis("Horizontal");
        direction.z = Input.GetAxis("Vertical");

        // the direction depends on the player speed
        direction *= PLAYER_SPEED;
    }

    private void FixedUpdate()
    {
        // the velocity is set according to what has been previously calculated
        rigidbody.velocity = direction;
    }


    private void LateUpdate()
    {
        // change the lookat so that the character faces its destination
        transform.LookAt(transform.position + direction);
    }
    private void OnDisable()
    {
        rigidbody.velocity = new Vector3(0, 0, 0);
    }
}
