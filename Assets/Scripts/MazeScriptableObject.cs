using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu]
public class MazeScriptableObject : ScriptableObject
{
    public string title;
    public Texture2D maze;

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
        maze = inputTex;
    }
}
