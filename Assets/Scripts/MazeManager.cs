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
        // Clean up existing maze
        CleanupMaze();

        // Use odd dimensions to ensure proper wall/path structure
        int width = (int)sliderLength.value * 2 + 1;
        int height = (int)sliderHeight.value * 2 + 1;
        
        Texture2D tex = null;
        Texture2D scaledTex = null;
        
        try
        {
            tex = new Texture2D(width, height, TextureFormat.RGB24, false);
            tex.filterMode = FilterMode.Point;

            // Initialize maze as all walls (white)
            Color[] colors = new Color[width * height];
            for (int i = 0; i < colors.Length; i++)
            {
                colors[i] = Color.white; // Walls are white
            }
            tex.SetPixels(colors);

            // Generate maze using DFS
            MazeAlgorithm(tex, width, height);
            
            tex.Apply();
            scaledTex = ScaleTexture(tex, 10);
            
            // Generate maze sprite
            maze = Sprite.Create(scaledTex,
                new Rect(0, 0, scaledTex.width, scaledTex.height),
                new Vector2(0.5f, 0.5f),
                1f
            );
            sr.sprite = maze;

            // Set position of maze
            Camera cam = Camera.main;
            if (cam != null)
            {
                Vector3 screenPosition = new Vector3(Screen.width / 2f, Screen.height * (2f / 3f), cam.nearClipPlane + 1f);
                Vector3 worldPosition = cam.ScreenToWorldPoint(screenPosition);
                worldPosition.z = 0f;
                transform.position = worldPosition;
            }
        }
        finally
        {
            if (tex != null)
            {
                DestroyImmediate(tex);
            }
        }
    }


    // Uses an iterative depth first search algorithm to create a maze
    private void MazeAlgorithm(Texture2D tex, int width, int height)
    {
        bool[,] visited = new bool[width, height];
        Stack<Vector2Int> stack = new Stack<Vector2Int>();

        // Start at (1,1)
        Vector2Int start = new Vector2Int(1, 1);
        stack.Push(start);

        // Mark starting cell as path
        tex.SetPixel(start.x, start.y, Color.green);
        visited[start.x, start.y] = true;

        while (stack.Count > 0)
        {
            Vector2Int current = stack.Peek();
            Vector2Int next = GetRandomUnvisitedNeighbor(current, visited, width, height);

            if (next != Vector2Int.zero) // Found unvisited neighbor
            {
                // Remove wall between current and next
                Vector2Int wall = current + (next - current) / 2;
                tex.SetPixel(wall.x, wall.y, Color.black);
                tex.SetPixel(next.x, next.y, Color.black);

                visited[next.x, next.y] = true;
                stack.Push(next);
            }
            else
            {
                stack.Pop(); // Backtrack
            }
        }
    }


    // Provides randomized neighbors that aren't on the path
    private Vector2Int GetRandomUnvisitedNeighbor(Vector2Int cell, bool[,] visited, int width, int height)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();

        // Check all four directions
        Vector2Int[] directions = {
            new Vector2Int(0, 2),  // Up
            new Vector2Int(2, 0),  // Right
            new Vector2Int(0, -2), // Down
            new Vector2Int(-2, 0)  // Left
        };

        foreach (Vector2Int dir in directions)
        {
            Vector2Int neighbor = cell + dir;

            // Check if neighbor is within bounds and unvisited
            if (neighbor.x > 0 && neighbor.x < width - 1 &&
                neighbor.y > 0 && neighbor.y < height - 1 &&
                !visited[neighbor.x, neighbor.y])
            {
                neighbors.Add(neighbor);
            }
        }

        if (neighbors.Count > 0)
        {
            return neighbors[Random.Range(0, neighbors.Count)];
        }

        return Vector2Int.zero; // No unvisited neighbors
    }



    // Clean up existing maze resources
    private void CleanupMaze()
    {
        if (maze != null)
        {
            if (maze.texture != null)
            {
                DestroyImmediate(maze.texture);
            }
            DestroyImmediate(maze);
            maze = null;
        }
    }



    // Scales up the texture
    public Texture2D ScaleTexture(Texture2D tex, int scaleFactor)
    {
        int newWidth = tex.width * scaleFactor;
        int newHeight = tex.height * scaleFactor;

        Texture2D scaledTexture = new Texture2D(newWidth, newHeight, TextureFormat.RGB24, false);
        scaledTexture.filterMode = FilterMode.Point;

        Color[] texPixels = tex.GetPixels();
        Color[] scaledPixels = new Color[newWidth * newHeight];

        for (int y = 0; y < newHeight; y++)
        {
            for (int x = 0; x < newWidth; x++)
            {
                int texX = x / scaleFactor;
                int texY = y / scaleFactor;
                int texIndex = texY * tex.width + texX;
                int scaledIndex = y * newWidth + x;

                if (texIndex < texPixels.Length && scaledIndex < scaledPixels.Length)
                {
                    scaledPixels[scaledIndex] = texPixels[texIndex];
                }
            }
        }

        scaledTexture.SetPixels(scaledPixels);
        scaledTexture.Apply();
        return scaledTexture;
    }



    private void OnDestroy()
    {
        CleanupMaze();
    }
}