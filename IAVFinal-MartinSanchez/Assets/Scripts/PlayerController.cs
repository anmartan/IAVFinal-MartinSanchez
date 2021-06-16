using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Moves the character around the map as the player wants.
/// </summary>
public class PlayerController : MonoBehaviour
{

    private Rigidbody rigidbody_;    
    private Vector3 direction_;      // where the character is headed to

    void Start()
    {
        rigidbody_ = GetComponent<Rigidbody>();
    }
    void Update()
    {
        Vector2 playerPos = GameManager.instance().GetMapPosition(transform.position);

        // if its arrived to the exit, go back to the main menu
        bool finish = false;
        int x = Mathf.RoundToInt(playerPos.x);
        int y = Mathf.RoundToInt(playerPos.y);
        finish = (GameManager.instance().GetLevel().map_[x, y] == Configuration.EXIT_CHAR);

        if (finish)
            GameManager.instance().FinishLevel();

        else
        {
            // if the player is encharged of moving the character, the input is read
            direction_.x = Input.GetAxis("Horizontal");
            direction_.z = Input.GetAxis("Vertical");

            // the direction depends on the player speed
            direction_ *= Configuration.PLAYER_SPEED_;
        }
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
