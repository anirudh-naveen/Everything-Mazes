using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MazeManager : MonoBehaviour
{
    public Slider sliderLength;
    public Slider sliderHeight;

    private Sprite maze;
    private SpriteRenderer sr;
    
    private void Start()
    {
        sr = gameObject.AddComponent<SpriteRenderer>();
    }

    public void GenerateMaze()
    {
        int width = (int)sliderLength.value * 3;
        int height = (int)sliderHeight.value * 3;

        // Create texture
        var tex = new Texture2D(width, height, TextureFormat.RGB24, false);

        // Set colors
        Color[] colors = new Color[width * height];
        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = Color.white;
        }
        tex.SetPixels(colors);
        tex.Apply();

        // Generate maze sprite
        maze = Sprite.Create(tex,
            new Rect(0, 0, width, height),  // texture dimensions
            new Vector2(0.5f, 0.5f),        // pivot at center
            1f                              // 1 pixel per unit
        );

        sr.sprite = maze;

        // set position of maze
        Camera cam = Camera.main;
        Vector3 screenPosition = new Vector3(Screen.width / 2f, Screen.height * (2f / 3f), cam.nearClipPlane + 1f);
        Vector3 worldPosition = cam.ScreenToWorldPoint(screenPosition);
        worldPosition.z = 0f;
        transform.position = worldPosition;
    }
}