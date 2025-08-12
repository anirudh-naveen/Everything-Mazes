using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class ButtonManager : MonoBehaviour
{
    [Header("Canvas")]
    public Canvas leftCanvas;
    public Canvas rightCanvas;

    [Header("Actions")]
    public MazeManager mazeManager;
    public GameObject drawableController;
    public LineDrawer lineDrawer;
    


    // Start is called before the first frame update
    void Start()
    {
        rightCanvas.GetComponent<CanvasGroup>().alpha = 0;
        rightCanvas.GetComponent<CanvasGroup>().interactable = false;
        rightCanvas.GetComponent<CanvasGroup>().blocksRaycasts = false;
        drawableController.SetActive(false);
    }



    // Swaps the active canvas upon a button press
    public void Swap()
    {
        if (rightCanvas.GetComponent<CanvasGroup>().interactable == false) {
            leftCanvas.GetComponent<CanvasGroup>().alpha = 0;
            leftCanvas.GetComponent<CanvasGroup>().interactable = false;
            leftCanvas.GetComponent<CanvasGroup>().blocksRaycasts = false;

            rightCanvas.GetComponent<CanvasGroup>().alpha = 1;
            rightCanvas.GetComponent<CanvasGroup>().interactable = true;
            rightCanvas.GetComponent<CanvasGroup>().blocksRaycasts = true;

            mazeManager.CleanupMaze();
            lineDrawer.ClearAllLines();
            
            drawableController.SetActive(true);
        } 
        else if (leftCanvas.GetComponent<CanvasGroup>().interactable == false)
        {
            rightCanvas.GetComponent<CanvasGroup>().alpha = 0;
            rightCanvas.GetComponent<CanvasGroup>().interactable = false;
            rightCanvas.GetComponent<CanvasGroup>().blocksRaycasts = false;

            leftCanvas.GetComponent<CanvasGroup>().alpha = 1;
            leftCanvas.GetComponent<CanvasGroup>().interactable = true;
            leftCanvas.GetComponent<CanvasGroup>().blocksRaycasts = true;

            mazeManager.CleanupMaze();
            lineDrawer.ClearAllLines();
        
            drawableController.SetActive(false);
        }
    }
}
