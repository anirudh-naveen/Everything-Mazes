using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Maze
{
    public string title;
    public Texture2D mazeTex;

    public Maze(Texture2D inputTex)
    {
        this.title = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        this.mazeTex = inputTex;
    }

    public void UpdateTitle(InputField inputField)
    {
        title = inputField.text;
    }

    public void UpdateTitle(string date)
    {
        title = date;
    }

    public void UpdateMazeTex(Texture2D inputTex)
    {
        mazeTex = inputTex;
    }
}
