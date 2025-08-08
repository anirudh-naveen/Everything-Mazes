using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LineDrawer : MonoBehaviour
{
    [Header("Existing Drawable Object")]
    public GameObject drawableObject; 

    [Header("Maze Details")]
    public Slider brushSize;
    public TMP_Dropdown ddMaze;
    public TMP_Dropdown ddPath;
 
    private Color32 brushColor;
    private Color32 mazeColor;
    private Color32 pathColor;

    private List<LineRenderer> lineRenderers = new List<LineRenderer>();
    private List<EdgeCollider2D> edgeColliders = new List<EdgeCollider2D>();
    private LineRenderer currentLineRenderer;
    private EdgeCollider2D currentEdgeCollider;
    private List<Vector2> currentStrokePoints = new List<Vector2>();

    private const float SensitivityThreshold = 0.3f;
    private const float MinYPosition = 350f;
    private bool isDrawing = false;


    private void Update()
    {
        // Update line width in real-time for current stroke
        if (currentLineRenderer != null)
        {
            float lineWidth = brushSize.value;
            currentLineRenderer.startWidth = lineWidth;
            currentLineRenderer.endWidth = lineWidth;
        }

        // Check if mouse button 0 (left mouse button) is pressed down.
        if (Input.GetMouseButtonDown(0))
        {
            HandleClickDown();
        }
        // Check if mouse button 0 (left mouse button) is held down.
        else if (Input.GetMouseButton(0) && isDrawing)
        {
            HandleMoving();
        }
        // Check if mouse button 0 (left mouse button) is released.
        else if (Input.GetMouseButtonUp(0))
        {
            HandleClickUp();
        }
    }



    private void HandleMoving()
    {
        // Get the current mouse position in world space.
        Vector2 tempTouchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Don't draw if below the boundary
        if (tempTouchPos.y < MinYPosition)
        {
            return;
        }

        // Check if a new line segment can be created.
        if (CanCreateNewLine(tempTouchPos))
        {
            // Call the DrawNewLine method with the new touch position.
            DrawNewLine(tempTouchPos);
        }
    }



    private void HandleClickDown()
    {
        SetColors();

        // Only proceed if we have a valid drawable object
        if (drawableObject == null)
        {
            Debug.LogError("No drawable object found! Cannot start drawing.");
            return;
        }

        // Get the current mouse position in world space.
        Vector2 startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        // Don't start drawing if below the boundary
        if (startPos.y < MinYPosition)
        {
            return;
        }

        // Create a new stroke for this drawing session
        CreateNewStroke();
        
        isDrawing = true;
        currentStrokePoints.Clear();

        // Add starting point twice (for minimum line segment)
        currentStrokePoints.Add(startPos);
        currentStrokePoints.Add(startPos);

        // Update the current line renderer
        UpdateCurrentStroke();
    }



    private void CreateNewStroke()
    {
        // Create a new child object for this stroke
        GameObject strokeObject = new GameObject("Stroke_" + lineRenderers.Count);
        strokeObject.transform.SetParent(drawableObject.transform);
        
        // Add LineRenderer and EdgeCollider2D components
        currentLineRenderer = strokeObject.AddComponent<LineRenderer>();
        currentEdgeCollider = strokeObject.AddComponent<EdgeCollider2D>();
        
        // Set initial line width
        float lineWidth = brushSize.value;
        currentLineRenderer.startWidth = lineWidth;
        currentLineRenderer.endWidth = lineWidth;
        
        // Set default material/color properties if needed
        // currentLineRenderer.material = yourMaterial;
        // currentLineRenderer.color = yourColor;
        
        // Add to tracking lists
        lineRenderers.Add(currentLineRenderer);
        edgeColliders.Add(currentEdgeCollider);
    }

    private void DrawNewLine(Vector2 newFingerPos)
    {
        // Add the new position to current stroke only
        currentStrokePoints.Add(newFingerPos);

        // Update the current stroke
        UpdateCurrentStroke();
    }

    private void UpdateCurrentStroke()
    {
        if (currentLineRenderer == null) return;

        // Set the position count to match current stroke points
        currentLineRenderer.positionCount = currentStrokePoints.Count;

        // Set positions for current stroke only
        for (int i = 0; i < currentStrokePoints.Count; i++)
        {
            currentLineRenderer.SetPosition(i, currentStrokePoints[i]);
        }

        // Update edge collider for current stroke
        if (currentEdgeCollider != null)
        {
            currentEdgeCollider.points = currentStrokePoints.ToArray();
        }
    }

    private void HandleClickUp()
    {
        isDrawing = false;
        currentStrokePoints.Clear();
    }

    private bool CanCreateNewLine(Vector2 tempTouchPos)
    {
        // Check if the distance between the current touch position and the last touch position is greater than the sensitivity threshold
        if (currentStrokePoints.Count > 0)
        {
            return Vector2.Distance(tempTouchPos, currentStrokePoints[currentStrokePoints.Count - 1]) > SensitivityThreshold;
        }
        return true;
    }

    // Method to clear all drawn lines
    public void ClearAllLines()
    {
        // Destroy all stroke game objects
        foreach (LineRenderer lr in lineRenderers)
        {
            if (lr != null && lr.gameObject != null)
            {
                DestroyImmediate(lr.gameObject);
            }
        }
        
        // Clear tracking lists
        lineRenderers.Clear();
        edgeColliders.Clear();
        currentStrokePoints.Clear();
        currentLineRenderer = null;
        currentEdgeCollider = null;
    }



    // Get color choices
    private void SetColors() {
        switch (ddMaze.value)
        {
            case 0:
                mazeColor = new Color32(0, 0, 0, 255);      // black
                break;
            case 1:
                mazeColor = new Color32(255, 0, 0, 255);    // red
                break;
            case 2:
                mazeColor = new Color32(0, 255, 0, 255);    // green
                break;
            case 3:
                mazeColor = new Color32(0, 0, 255, 255);    // blue
                break;
            case 4:
                mazeColor = new Color32(255, 255, 0, 255);  // yellow
                break;
            case 5:
                mazeColor = new Color32(255, 0, 255, 255);  // magenta
                break;
            case 6:
                mazeColor = new Color32(0, 255, 255, 255);  // cyan
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
    }
}