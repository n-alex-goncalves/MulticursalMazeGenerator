using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze_Creation_2 : MonoBehaviour
{

    void Start()
    {
        GameObject maze = GameObject.Find("Maze");
        Instantiate(maze);
        maze.transform.position = new Vector2(25,0);
    }
}   
