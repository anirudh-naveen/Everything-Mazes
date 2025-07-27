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
        this.width = 10;
        this.height = 10;
        this.scale = 10;
    }
}
