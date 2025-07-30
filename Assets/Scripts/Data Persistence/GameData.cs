using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int width;
    public int height;
    public int scale;
    public Maze maze;


    // Initial values the game will start with
    public GameData()
    {
        width = 21;
        height = 21;
        scale = 10;
        maze = null;
    }
}
