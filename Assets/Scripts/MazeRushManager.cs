using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeRushManager : MonoBehaviour
{

    [Header("Items")]
    public Canvas canvas;

    public MazeManager mazeManager;


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
        mazeManager.GenerateMazeRush();
    }
}
