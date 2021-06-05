using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    static public GameManager instance_;

    public GameObject player_;
    bool moving = true;

    void Awake()
    {
        if (!instance_)
            instance_ = this;
        else
            Destroy(this);
    }

    // Start is called before the first frame update
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // this will not be here in the final version
            MazeCreator aux = GetComponent<MazeCreator>();
            aux.CreateMaze();
        }
        else if (Input.GetKeyDown(KeyCode.M))
        {
            player_.GetComponent<PlayerController>().enabled = moving;
            player_.GetComponent<RandomSolver>().enabled = !moving;

            Debug.Log(moving);
            moving = !moving;
        }

    }


    static public GameManager instance()
    {
        return instance_;
    }

    public GameObject GetPlayer() { return player_; }
}
