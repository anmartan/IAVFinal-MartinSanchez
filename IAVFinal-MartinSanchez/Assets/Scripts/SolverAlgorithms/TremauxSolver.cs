using UnityEngine;

/// <summary>
/// Improvement of the random solver: in every intersection, chooses a random direction to follow.
/// Remembers where it has already been, so it doesnt repeat the same path over and over again.
/// This is known as the Tremaux algorithm
/// </summary>
public class TremauxSolver : Solver
{
    /// <summary>
    /// The path chosen depends on how many times a road has been already used.
    /// This indicates if a road will be chosen for the first time, if its been walked over once, or if it is useless to choose it again
    /// </summary>
    private enum STATE {NOT_CROSSED = 0, CROSSED, FORBIDDEN};

    private STATE[,] map_;       // representation of the intersections in the scene


    /// <summary>
    /// First rule: dont go into a corridor that has two marks on it (FORBIDDEN).
    /// Second rule: if you step into a corridor with a mark on it (CROSSED), go back.
    /// Third rule: if youre supposed to go back but you cant because theres two marks on that corridor,
    /// go into the hallway with the least marks on it (NOT_CROSSED or CROSSED).
    /// </summary>
    override protected void FindPath(Vector2 playerPos)
    {
        // if it cannot go anywhere else, it goes back again
        if (possibleDirections_.Count == 0)
        {
            direction_ = -transform.forward;
            return;
        }
        
        //if it is at a corner and can only go one way, it does so
        if(possibleDirections_.Count == 1)
        {
            direction_ = possibleDirections_[0];
            return;
        }

        //---------- Tremaux algorithm starts here ----------//

        // marks the path it comes from
        Vector2 from = new Vector2(Mathf.RoundToInt(transform.forward.z), 
                                  -Mathf.RoundToInt(transform.forward.x));
        VisitRoad(playerPos + from);

        // checks for unmarked paths
        Vector3 dir = CheckState(playerPos, STATE.NOT_CROSSED);

        // if there are not unmarked paths, checks for those with one mark
        bool goback = false;
        if (dir == Vector3.zero) 
        {
            dir = CheckState(playerPos, STATE.CROSSED);
            goback = true;
        }

        // if there are not marked paths, every turn is forbidden. There is no exit
        if (dir == Vector3.zero)
        {
            direction_ = Vector3.zero;
            return;
        }

        Vector2 to = new Vector2(-Mathf.RoundToInt(dir.z),
                                Mathf.RoundToInt(dir.x));

        // if its supposed to go back, two things can happen
        if (goback)
        {
            // if it cant go back because its forbidden, it takes another direction
            if (GetState(playerPos + from) == STATE.FORBIDDEN)
            {
                VisitRoad(playerPos + to);
                direction_ = dir;
            }
            // if it can go back, it does
            else
            {
                VisitRoad(playerPos + from);
                direction_ = -transform.forward;
            }
        }
        // it goes in the direction calculated
        else
        {
            VisitRoad(playerPos + to);
            direction_ = dir;
        }
    }

    /// <summary>
    /// Checks if there are any corridors the player could use, because the have the state specified.
    /// Checks it in a random order.
    /// </summary>
    /// <param name="playerPos">The position of the player and the intersection.</param>
    /// <param name="state">The state of the corridor that the player is interested in.</param>
    /// <returns>A corridor, if there is any with that state. Otherwise, a Vector3 in which all of its components are 0.</returns>
    private Vector3 CheckState(Vector2 playerPos, STATE state)
    {
        Vector3 direction = Vector3.zero;

        // checks if there is any unmarked path, starting at a random direction
        int i = Random.Range(0, possibleDirections_.Count);
        int loops = 0;
        bool found = false;
        while (loops < possibleDirections_.Count && !found)
        {
            Vector2 dir = new Vector2(-Mathf.RoundToInt(possibleDirections_[i].z),
                                        Mathf.RoundToInt(possibleDirections_[i].x));
            found = map_[(int)(playerPos.x + dir.x), (int)(playerPos.y + dir.y)] == state;

            if (!found)
            {
                i = (i + 1) % possibleDirections_.Count;
                loops++;
            }
            else
                direction = possibleDirections_[i];
        }

        return direction;
    }

    /// <summary>
    /// Returns the state of the road given.
    /// </summary>
    /// <param name="pos">The position of the intersection</param>
    /// <returns>The state of the road in that direction</returns>
    private STATE GetState(Vector2 pos)
    {
        return map_[(int)pos.x, (int)pos.y];
    }

    /// <summary>
    /// Marks the road specified. If it is forbidden, it does not add an extra mark.
    /// </summary>
    /// <param name="pos">The road that will be marked</param>
    private void VisitRoad(Vector2 pos)
    {
        if (map_[(int)pos.x, (int)pos.y] < STATE.FORBIDDEN)
            map_[(int)pos.x, (int)pos.y]++;
    }


    //------ Unity functions. All of them call the base method, defined in Solver.cs ------//

    protected override void Start()
    {
        base.Start();
        map_ = new STATE[level_.size_, level_.size_];
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
