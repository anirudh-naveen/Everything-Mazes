using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalRushManager : MonoBehaviour
{
    [Header("UI Items")]
    public Canvas canvas;

    [Header("Player Managers")]
    public Player1Manager player1;
    public Player2Manager player2;

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
            player1.GenerateMaze();
            player2.GenerateMaze();
        }
    }
}
