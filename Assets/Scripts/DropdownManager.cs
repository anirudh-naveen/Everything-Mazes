using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DropdownManager : MonoBehaviour
{
    [Header("Dropdown Components")]
    public TMP_Dropdown ddMaze;
    public TMP_Dropdown ddPath;
    public TextMeshProUGUI ddMazeLabel;
    public TextMeshProUGUI ddPathLabel;

    void Start()
    {    
        // Set initial colors
        UpdatePathLabelColor();
        UpdateMazeLabelColor();
    }



    public void OnPathDropdownChanged(int value)
    {
        UpdatePathLabelColor();
    }

    public void OnMazeDropdownChanged(int value)
    {
        UpdateMazeLabelColor();
    }


    // Change path label colors based on dropdown selection
    void UpdatePathLabelColor()
    {
        switch (ddPath.value)
        {
            case 0:
                ddPathLabel.color = Color.black;
                break;
            case 1:
                ddPathLabel.color = Color.red;
                break;
            case 2:
                ddPathLabel.color = Color.green;
                break;
            case 3:
                ddPathLabel.color = Color.blue;
                break;
            case 4:
                ddPathLabel.color = Color.yellow;
                break;
        }
    }

    // Change maze label colors based on dropdown selection
    void UpdateMazeLabelColor()
    {
        switch (ddMaze.value)
        {
            case 0:
                ddMazeLabel.color = Color.black;
                break;
            case 1:
                ddMazeLabel.color = Color.red;
                break;
            case 2:
                ddMazeLabel.color = Color.green;
                break;
            case 3:
                ddMazeLabel.color = Color.blue;
                break;
            case 4:
                ddMazeLabel.color = Color.yellow;
                break;
        }
    }
}