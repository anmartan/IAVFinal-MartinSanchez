using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Drawing;

/// <summary>
/// Creates a maze from a given bitmap.
/// Reads a file.map and substitutes FLOOR_CHAR by floor cells, and WALL_CHAR by walls.
/// </summary>
namespace IAVMazeSolver
{
    public class MazeCreator : MonoBehaviour
    {
        [SerializeField] private uint size_ = 0;            // size of the maze (square dimensions)
        [SerializeField] private string filePath_ = "";     // where the map is saved (the file to be read)
        [SerializeField] private GameObject wallPrefab_;
        [SerializeField] private GameObject floorPrefab_;
        [SerializeField] private GameObject playerPrefab_;

        private void Start()
        {
            CreateMaze();
        }

        /// <summary>
        /// Used to create the maze. 
        /// Reads a file and transforms it into a world scene
        /// </summary>
        void CreateMaze()
        {
            StreamReader file = new StreamReader(filePath_);
            size_ = uint.Parse(file.ReadLine());

            // instantiates a single floor
            GameObject floor = Instantiate(floorPrefab_, new Vector3(size_ * 0.5f * GlobalVariables.WORLD_SCALE, 0, size_ * 0.5f * GlobalVariables.WORLD_SCALE), Quaternion.identity);
            floor.transform.localScale = floor.transform.localScale * GlobalVariables.WORLD_SCALE * size_;

            string line = "";
            // instantiates all the walls that should be created
            for (int i = 0; i < size_; i++)
            {
                line = file.ReadLine();
                for (int j = 0; j < size_; j++)
                {
                    Vector3 pos = new Vector3(j * GlobalVariables.WORLD_SCALE, 0, i * GlobalVariables.WORLD_SCALE);
                    pos += 0.5f * wallPrefab_.transform.lossyScale;
                    if (line[j] == GlobalVariables.WALL_CHAR)
                    {
                        // if the file says there is a wall in this position, it is created
                        Instantiate(wallPrefab_, pos, Quaternion.identity);
                    }
                }
            }

            // finds the position where the player shall start
            bool player = false;
            int x = 0;
            while (x < size_ && !player) 
            {
                player = (line[x] == GlobalVariables.FLOOR_CHAR);
                if (!player) x++;
            }

            // create the player in that position
            Vector3 playerPos = new Vector3(x * GlobalVariables.WORLD_SCALE, 0, (size_ - 1) * GlobalVariables.WORLD_SCALE);
            playerPos += 0.5f * playerPrefab_.transform.lossyScale;

            Instantiate(playerPrefab_, playerPos, Quaternion.identity);

        }
    }
}