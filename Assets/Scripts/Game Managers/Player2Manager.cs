using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2Manager : MonoBehaviour
{
    [Header("Maze Generation")]
    public MazeManager mazeManager;
    
    [Header("Camera Reference")]
    public Camera sceneCamera;
    
    private SpriteRenderer sr;
    
    // Start is called before the first frame update
    void Start()
    {
        // Get or add SpriteRenderer component once
        sr = GetComponent<SpriteRenderer>();
        if (sr == null)
        {
            sr = gameObject.AddComponent<SpriteRenderer>();
        }
        
        // Verify mazeManager is assigned
        if (mazeManager == null)
        {
            Debug.LogError("MazeManager is not assigned to MazeRushManager!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void GenerateMaze()
    {        
        // Generate the maze and get the sprite
        Sprite mazeSprite = mazeManager.GenerateMazeRush();
        
        // Display the maze sprite
        sr.sprite = mazeSprite;
        
        // Make sure SpriteRenderer is enabled and visible
        sr.enabled = true;
        sr.sortingOrder = 10; 
        sr.color = Color.white;
        
        // Set the position of the mazes
        PositionMaze();
    }


    // Position the player two maze to fit the screen properly
    private void PositionMaze()
    {
        Camera cam = sceneCamera; // Try assigned camera first

        // If no camera assigned, try to find main camera
        if (cam == null)
        {
            cam = Camera.main;
        }

        // If main camera not found, try to find any active camera
        if (cam == null)
        {
            cam = FindObjectOfType<Camera>();
        }

        // Scale the maze to fit better in view
        float cameraHeight = cam.orthographicSize * 2f;
        float cameraWidth = cameraHeight * cam.aspect;

        // Get the maze bounds
        Bounds mazeBounds = sr.sprite.bounds;

        // Calculate scale to fit maze in camera view 
        float scaleX = (cameraWidth * 0.03f) / mazeBounds.size.x;
        float scaleY = (cameraHeight * 0.03f) / mazeBounds.size.y;
        float finalScale = Mathf.Min(scaleX, scaleY, 1f);

        transform.localScale = Vector3.one * finalScale;

        // Position maze at center of camera view
        Vector3 cameraPos = cam.transform.position;
        transform.position = new Vector3(cameraPos.x + 0.3f, cameraPos.y, cameraPos.z + 1f);

    }
}