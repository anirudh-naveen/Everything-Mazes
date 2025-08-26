using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeRushManager : MonoBehaviour
{
    [Header("UI Items")]
    public Canvas canvas;
    
    [Header("Maze Generation")]
    public MazeManager mazeManager;
    
    private SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            canvas.GetComponent<CanvasGroup>().alpha = 0;
            canvas.GetComponent<CanvasGroup>().interactable = false;
            canvas.GetComponent<CanvasGroup>().blocksRaycasts = false;
            StartLevel();
        }
    }

    void StartLevel()
    {
        // Generate the maze and get the sprite and sprite renderer
        Sprite mazeSprite = mazeManager.GenerateMazeRush();
        sr = gameObject.AddComponent<SpriteRenderer>();

        // Display the maze sprite
        sr.sprite = mazeSprite;

        // Set the default camera of the maze
        Camera cam = Camera.main;
        if (cam != null)
        {
            Vector3 screenPosition = new Vector3(Screen.width / 2f, Screen.height * (2f / 3f), cam.nearClipPlane + 1f);
            Vector3 worldPosition = cam.ScreenToWorldPoint(screenPosition);
            worldPosition.z = 0f;
            transform.position = worldPosition;
        }
    }
}