using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MazeManager : MonoBehaviour, IDataPersistence
{
    [Header("Slider Components")]
    public Slider sliderLength;
    public Slider sliderHeight;
    public Slider sliderScale;

    [Header("Dropdown Components")]
    public TMP_Dropdown ddMaze;
    public TMP_Dropdown ddPath;

    private Maze maze;
    private Sprite mazeSprite;
    private SpriteRenderer sr;

    private Color32 mazeColor;
    private Color32 pathColor;
    private Color32 startColor;
    private Color32 endColor;

    private int width;
    private int height;
    private int scale;

    private void Start()
    {
        sr = gameObject.AddComponent<SpriteRenderer>();
    }

    public void GenerateMaze()
    {
        // Clean up existing maze
        CleanupMaze();

        // Get color choices
        switch (ddMaze.value)
        {
            case 0:
                mazeColor = new Color32(0, 0, 0, 255);      // black
                startColor = new Color32(0, 255, 0, 255);
                endColor = new Color32(255, 0, 0, 255);
                break;
            case 1:
                mazeColor = new Color32(255, 0, 0, 255);    // red
                startColor = new Color32(85, 255, 0, 255);
                endColor = new Color32(255, 0, 128, 255);
                break;
            case 2:
                mazeColor = new Color32(0, 255, 0, 255);    // green
                startColor = new Color32(255, 85, 0, 255);
                endColor = new Color32(255, 0, 128, 255);
                break;
            case 3:
                mazeColor = new Color32(0, 0, 255, 255);    // blue
                startColor = new Color32(255, 255, 0, 255);
                endColor = new Color32(255, 85, 0, 255);
                break;
            case 4:
                mazeColor = new Color32(255, 255, 0, 255);  // yellow
                startColor = new Color32(0, 0, 255, 255);
                endColor = new Color32(255, 0, 255, 255);
                break;
            case 5:
                mazeColor = new Color32(255, 0, 255, 255);  // magenta
                startColor = new Color32(85, 255, 0, 255);
                endColor = new Color32(255, 255, 0, 255);
                break;
            case 6:
                mazeColor = new Color32(0, 255, 255, 255);  // cyan
                startColor = new Color32(255, 170, 0, 255);
                endColor = new Color32(255, 0, 0, 255);
                break;
        }

        switch (ddPath.value)
        {
            case 0:
                pathColor = new Color32(255, 255, 255, 255);    // white
                break;
            case 1:
                pathColor = new Color32(255, 200, 200, 255);    // red
                break;  
            case 2:
                pathColor = new Color32(200, 255, 200, 255);    // green
                break;
            case 3:
                pathColor = new Color32(200, 200, 255, 255);    // blue
                break;
            case 4:
                pathColor = new Color32(255, 255, 200, 255);    // yellow
                break;
            case 5:
                pathColor = new Color32(255, 200, 255, 255);    // magenta
                break;
            case 6:
                pathColor = new Color32 (200, 255, 255, 255);   // cyan
                break;
        }

        // Retrieve slider values
        int width = (int)sliderLength.value * 2 + 1;
        int height = (int)sliderHeight.value * 2 + 1;
        int scale = (int)sliderScale.value;
        
        // Initialize textures
        Texture2D tex = null;
        Texture2D scaledTex = null;
        
        try
        {
            tex = new Texture2D(width, height, TextureFormat.RGB24, false);
            tex.filterMode = FilterMode.Point;

            // Initialize maze as all walls
            Color32[] colors = new Color32[width * height];
            for (int i = 0; i < colors.Length; i++)
            {
                colors[i] = mazeColor;
            }
            tex.SetPixels32(colors);

            // Generate maze
            MazeAlgorithm(tex, width, height);

            tex.Apply();
            scaledTex = ScaleTexture(tex, scale);

            // Update the maze with the completed texture
            maze = new Maze(scaledTex);

            // Generate maze sprite
            mazeSprite = Sprite.Create(scaledTex,
                new Rect(0, 0, scaledTex.width, scaledTex.height),
                new Vector2(0.5f, 0.5f),
                1f
            );
            sr.sprite = mazeSprite;

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


    // Uses an iterative stack algorithm to create a maze
    private void MazeAlgorithm(Texture2D tex, int width, int height)
    {
        bool[,] visited = new bool[width, height];
        Stack<Vector2Int> stack = new Stack<Vector2Int>();

        GenStartPixel(visited, stack, tex, width, height);

        while (stack.Count > 0)
        {
            Vector2Int current = stack.Peek();
            Vector2Int next = GetRandomUnvisitedNeighbor(current, visited, width, height);

            if (next != Vector2Int.zero) // Found unvisited neighbor
            {
                // Remove wall between current and next
                Vector2Int wall = current + (next - current) / 2;
                tex.SetPixel(wall.x, wall.y, pathColor);
                tex.SetPixel(next.x, next.y, pathColor);

                visited[next.x, next.y] = true;
                stack.Push(next);
            }
            else
            {
                stack.Pop(); // Backtrack through stack
            }
        }

        GenEndPixel(visited, stack, tex, width, height);
    }



    // Generates starting position of the maze
    private void GenStartPixel(bool[,] visited, Stack<Vector2Int> stack, Texture2D tex, int width, int height)
    {
        Vector2Int start = new Vector2Int(0, 0);
        switch (Random.Range(1, 4))
        {
            case 1:
                start = new Vector2Int(0, Random.Range(1, height - 1));
                break;
            case 2:
                start = new Vector2Int(width - 1, Random.Range(1, height - 1));
                break;
            case 3:
                start = new Vector2Int(Random.Range(1, width - 1), 0);
                break;
            case 4:
                start = new Vector2Int(Random.Range(1, width - 1), height - 1);
                break;
        }
        stack.Push(start);

        tex.SetPixel(start.x, start.y, startColor);
        visited[start.x, start.y] = true;
    }


    // Generates ending position of the maze
    private void GenEndPixel(bool[,] visited, Stack<Vector2Int> stack, Texture2D tex, int width, int height)
    {
        int maxAttempts = 1000; // Prevent infinite loops
        int attempts = 0;
        
        while (attempts < maxAttempts)
        {
            attempts++;
            
            Vector2Int end = new Vector2Int(0, 0);
            switch (Random.Range(1, 5))
            {
                case 1:
                    end = new Vector2Int(0, Random.Range(1, height - 1));
                    break;
                case 2:
                    end = new Vector2Int(width - 1, Random.Range(1, height - 1));
                    break;
                case 3:
                    end = new Vector2Int(Random.Range(1, width - 1), 0);
                    break;
                case 4:
                    end = new Vector2Int(Random.Range(1, width - 1), height - 1);
                    break;
            }

            // Check if this position is valid
            if (!visited[end.x, end.y] && PathNearPixel(end, visited, width, height))
            {
                stack.Push(end);
                tex.SetPixel(end.x, end.y, endColor);
                visited[end.x, end.y] = true;
                return;
            }
        }
        
        // FALLBACK 1: Place end at any unvisited position with adjacent path
        for (int x = 1; x < width - 1; x++)
        {
            for (int y = 1; y < height - 1; y++)
            {
                Vector2Int end = new Vector2Int(x, y);
                if (!visited[x, y] && PathNearPixel(end, visited, width, height))
                {
                    stack.Push(end);
                    tex.SetPixel(x, y, new Color32(255, 0, 0, 50));
                    visited[x, y] = true;
                    return;
                }
            }
        }
        
        // FALLBACK 2: Place end at any unvisited position
        for (int x = 1; x < width - 1; x++)
        {
            for (int y = 1; y < height - 1; y++)
            {
                if (!visited[x, y])
                {
                    Vector2Int end = new Vector2Int(x, y);
                    stack.Push(end);
                    tex.SetPixel(x, y, endColor);
                    visited[x, y] = true;
                    return;
                }
            }
        }
    }



    // Provides randomized neighbors that aren't on the path
    private Vector2Int GetRandomUnvisitedNeighbor(Vector2Int pixel, bool[,] visited, int width, int height)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();

        // Check all four directions for walls
        Vector2Int[] directions = {
            new Vector2Int(0, 2),  // Up
            new Vector2Int(2, 0),  // Right
            new Vector2Int(0, -2), // Down
            new Vector2Int(-2, 0)  // Left
        };

        foreach (Vector2Int direction in directions)
        {
            Vector2Int neighbor = pixel + direction;

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

        return Vector2Int.zero; // Return no unvisited neighbors found
    }
    


    // Checks if there is no path connecting to a pixel
    private bool PathNearPixel(Vector2Int pixel, bool[,] visited, int width, int height)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>();

        // Check all four directions for paths
        Vector2Int[] directions = {
            new Vector2Int(0, 1),  // Up
            new Vector2Int(1, 0),  // Right
            new Vector2Int(0, -1), // Down
            new Vector2Int(-1, 0)  // Left
        };

        foreach (Vector2Int direction in directions)
        {
            Vector2Int neighbor = pixel + direction;

            // Check if neighbor is within bounds and unvisited
            if (neighbor.x > 0 && neighbor.x < width - 1 &&
                neighbor.y > 0 && neighbor.y < height - 1 &&
                visited[neighbor.x, neighbor.y])
            {
                return true;
            }
        }

        return false;
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




    public void LoadData(GameData data)
    {
        this.width = data.width;
        this.height = data.height;
        this.scale = data.scale;
        this.maze = data.maze;
    }



    public void SaveData(ref GameData data)
    {
        data.width = this.width;
        data.height = this.height;
        data.scale = this.scale;
        data.maze = this.maze;
    }



    // Clean up existing maze resources
    private void CleanupMaze()
    {
        if (mazeSprite != null)
        {
            if (mazeSprite.texture != null)
            {
                DestroyImmediate(mazeSprite.texture);
            }
            DestroyImmediate(mazeSprite);
            mazeSprite = null;
        }

        if (maze != null)
        {
            maze = null;
        }
    }



    private void OnDestroy()
    {
        CleanupMaze();
    }
}