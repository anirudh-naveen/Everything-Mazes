using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class Maze
{
    public string title;
    public int width;
    public int height;
    public Color32[] pixels;

    public Maze(Texture2D texture)
    {
        title = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        width = texture.width;
        height = texture.height;
        pixels = texture.GetPixels32();
    }

    public void UpdateTitle(InputField inputField)
    {
        title = inputField.text;
    }

    public void UpdateTitle(string date)
    {
        title = date;
    }


    // Method to recreate texture from stored data
    public Texture2D ToTexture()
    {
        if (pixels == null || width == 0 || height == 0)
            return null;
            
        Texture2D texture = new Texture2D(width, height, TextureFormat.RGB24, false);
        texture.filterMode = FilterMode.Point;
        texture.SetPixels32(pixels);
        texture.Apply();
        return texture;
    }
}
