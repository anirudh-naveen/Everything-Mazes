using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MazeManager : MonoBehaviour
{
    [Header("Slider Components")]
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
        var tex = new Texture2D(width, height, TextureFormat.RGB24, false);
    
        // Initialize list of adjacent pixels
        List<int[]>[] adjPixels = new List<int[]>[width * height];
        for (int i = 0; i < adjPixels.Length; i++)
        {
            adjPixels[i] = new List<int[]>();
        }

        // Add adjacent pixels to lists based on pixel index
        for (int x = 1; x < width - 1; x++) 
        {
            for (int y = 1; y < height - 1; y++)
            {
                int index = y * width + x;
                if (x + 1 < width - 1)
                {
                    adjPixels[index].Add(new int[] { x + 1, y });
                }
                if (x - 1 > 1)
                {
                    adjPixels[index].Add(new int[] { x - 1, y });
                }
                if (y + 1 < height - 1)
                {
                    adjPixels[index].Add(new int[] { x, y + 1 });
                }
                if (y - 1 > 1)
                {
                    adjPixels[index].Add(new int[] { x, y - 1 });
                }
            }
        }

        // Color maze
        Color[] colors = ColorMaze(width, height);
        tex.SetPixels(colors);

        List<int[]> path = DFS(adjPixels, width);
        foreach (int[] pixel in path)
        {
            tex.SetPixel(pixel[0], pixel[1], Color.black, 0);
        }
        tex.Apply();
        

        // Generate maze sprite
        maze = Sprite.Create(tex,
            new Rect(0, 0, width, height),  // texture dimensions
            new Vector2(0.5f, 0.5f),        // pivot at center
            1f                              // 1 pixel per unit
        );

        sr.sprite = maze;

        // Set position of maze
        Camera cam = Camera.main;
        Vector3 screenPosition = new Vector3(Screen.width / 2f, Screen.height * (2f / 3f), cam.nearClipPlane + 1f);
        Vector3 worldPosition = cam.ScreenToWorldPoint(screenPosition);
        worldPosition.z = 0f;
        transform.position = worldPosition;
    }




    // Automatically colors in the maze
    private Color[] ColorMaze(int width, int height)
    {
        Color[] colors = new Color [width * height];

        for (int i = 0; i < colors.Length; i++)
        {
            colors[i] = Color.white;
        }
        return colors;
    }




    // Main method for the Depth First Search algorithm
    private List<int[]> DFS(List<int[]>[] adjPixels, int width)
    {
        bool[] visited = new bool[adjPixels.Length];
        List<int[]> path = new List<int[]>();
        int[] firstPixel = new int[] { 1, 1 };

        DFSLoop(adjPixels, visited, firstPixel, path, width);
        return path;
    }




    // Recursive DFS traversal
    private void DFSLoop(List<int[]>[] adjPixels, bool[] visited, int[] pixel, List<int[]> path, int width)
    {
        int index = pixel[1] * width + pixel[0];
        visited[index] = true;
        path.Add(pixel);

        // Shuffle adjacent pixels
        List<int[]> adjShuffled = new List<int[]>(adjPixels[index]);
        for (int i = 0; i < adjShuffled.Count; i++)
        {
            int randomIndex = Random.Range(i, adjShuffled.Count);
            int[] temp = adjShuffled[i];
            adjShuffled[i] = adjShuffled[randomIndex];
            adjShuffled[randomIndex] = temp;
        }

        foreach (int[] adjPixel in adjShuffled)
        {
            int adjIndex = adjPixel[1] * width + adjPixel[0];

            if (adjIndex < 0 || adjIndex >= visited.Length)
                continue;

            if (visited[adjIndex])
                continue;

            Debug.Log(adjIndex);

            // Do not bypass walls
            int direction = index - adjIndex;
            Debug.Log(direction);

            if (adjIndex - width < 0 || adjIndex + width >= visited.Length)
            {
                continue;
            }

            if (direction == 1)
            {
                if (visited[adjIndex - 1])
                {
                    Debug.Log("1");
                    continue;
                }
            }
            else if (direction == -1)
            {
                if (visited[adjIndex + 1])
                {
                    Debug.Log("2");
                    continue;
                }
            }
            else if (direction == width)
            {
                if (visited[adjIndex - width])
                {
                    Debug.Log("3");
                    continue;
                }
            }
            else
            {
                if (visited[adjIndex + width])
                {
                    Debug.Log("4");
                    continue;
                }
            }

            Debug.Log("Made it!");
            DFSLoop(adjPixels, visited, adjPixel, path, width);
        }
    }
}