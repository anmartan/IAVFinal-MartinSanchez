using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Saves some data about the path calculated, such as the number of tiles the character has walked on, or the time since the simulation started
/// Shows a map with information about which tiles have been walked on, in a graphic way.
/// </summary>
public class PathTracker : MonoBehaviour
{
    [SerializeField] private GameObject cellPrefab_;
    [SerializeField] private Transform parent_;
    [SerializeField] private Color unvisited_;          // the color used to paint the visited tiles
    [SerializeField] private Color visited_;            // the color used to paint the visited tiles
    [SerializeField] private float alpha_;              // how much less transparency a tile will have if the character walks over it

    private GameObject[,] map_;     // the cells that will be painted over the map

    private int[,] valuesMap_;      // how many times a tile has been walked over
    private level level_;           // info about the maze

    /// <summary>
    /// Creates the tiles that will draw the path Red Riding Hood is following.
    /// </summary>
    public void CreateMap()
    {
        level_ = GameManager.instance().GetLevel();
        map_ = new GameObject[level_.size_, level_.size_];
        valuesMap_ = new int[level_.size_, level_.size_];


        for (int i = 0; i < level_.size_; i++)
        {
            for (int j = 0; j < level_.size_; j++)
            {
                valuesMap_[i, j] = 0;

                Vector3 pos = new Vector3((j + 0.5f) * Configuration.WORLD_SCALE, 0.1f, -(i + 0.5f) * Configuration.WORLD_SCALE);
                map_[i,j] = Instantiate(cellPrefab_, pos, Quaternion.identity, parent_);
                map_[i, j].GetComponent<MeshRenderer>().material.color = unvisited_;
            }
        }
    }

    /// <summary>
    /// Changes the alpha of the tile to show the path. 
    /// Updates the valuesMap_.
    /// </summary>
    /// <param name="cell">The tile the character has stepped into.</param>
    /// <returns>The ammount of times the character has walked over this tile.</returns>
    public int VisitCell(Vector2 cell)
    {
        float alpha = map_[(int)cell.x, (int)cell.y].GetComponent<MeshRenderer>().material.color.a + alpha_;
        Color color = visited_;
        color.a = Mathf.Min(alpha, 1);  // so it does not go over 1

        map_[(int)cell.x, (int)cell.y].GetComponent<MeshRenderer>().material.color = color;

        return valuesMap_[(int)cell.x, (int)cell.y];
    }
}
