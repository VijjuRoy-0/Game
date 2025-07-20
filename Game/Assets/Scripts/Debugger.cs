using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Debugger : MonoBehaviour
{
    // Drag your main Canvas from the Hierarchy onto this slot in the Inspector
    public GraphicRaycaster graphicRaycaster;
    private PointerEventData pointerEventData;
    private EventSystem eventSystem;

    void Start()
    {
        // Automatically find the EventSystem
        eventSystem = FindObjectOfType<EventSystem>();
        if (graphicRaycaster == null)
        {
            Debug.LogError("Please assign the Graphic Raycaster from your Canvas to the debugger script.");
        }
    }

    void Update()
    {
        // When you left-click the mouse...
        if (Input.GetMouseButtonDown(0))
        {
            // Set up the event system
            pointerEventData = new PointerEventData(eventSystem);
            pointerEventData.position = Input.mousePosition;

            // Create a list to store the results of the raycast
            List<RaycastResult> results = new List<RaycastResult>();

            // Ask the Graphic Raycaster what the mouse is over
            graphicRaycaster.Raycast(pointerEventData, results);

            // Check if we hit anything
            if (results.Count > 0)
            {
                // Print the name of the TOPMOST object we hit. This is the blocker.
                Debug.LogWarning("CLICK HIT: " + results[0].gameObject.name);
            }
            else
            {
                Debug.Log("Clicked on empty space (no UI element hit).");
            }
        }
    }
}