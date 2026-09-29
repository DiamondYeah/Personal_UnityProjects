using System;
using System.Collections.Generic;
using UnityEngine;


enum ConnectionColor {Red, Blue, LightGreen, DarkBlue, Green, Yellow, Coral};

public class ButtonTrigger : MonoBehaviour
{


    [Header("Button-Platform Connection Settings")]
    [SerializeField]
    private GameObject buttonHead;
    [SerializeField]
    private GameObject platform;
    [SerializeField]
    private ConnectionColor connectionColor;

    [Space(20)]
    [SerializeField]
    public bool canRotatePlatform = false;
    public bool canMovePlatform = false;

    private PlatformController script;
    
    private Dictionary<string, Color> colorList = new Dictionary<string, Color>
    {
        {"Red",  Color.red},
        {"Blue", Color.blue},
        {"LightGreen", Color.lightGreen},
        {"DarkBlue", Color.darkBlue},
        {"Green",  Color.green},
        {"Yellow", Color.yellow},
        {"Coral", Color.coral},
    };


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get platform script and set the boolean value of isConnectedToButton to true
        script = platform.GetComponent<PlatformController>();
        script.isConnectedToButton = true;

        ChangeColor();
    }

    // OnTriggerStay is called when a collider continues to collide inside the trigger
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Check if the platform can rotate, if so, check if it has rotated to position. If not, make it true so it starts rotating, else false
            if (canRotatePlatform && !script.hasRotatedToPosition)
            {
                
               script.isRotationActive = true;
               return;
                
            }

            script.isRotationActive = false;

            // Check if the platform can move, if so, check if its currently moving. If not, make it true so it starts moving, else false
            if (canMovePlatform)
            {
                if (!script.isMovingActive)
                {
                    script.isMovingActive = true;
                    return;
                }

                script.isRotationActive = false;
            }

        }
    }

    // OnTriggerExit is called when a collider exits the trigger
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Turn false to stop rotating platforms
            if (canRotatePlatform && script.hasRotatedToPosition)
            {
                script.hasRotatedToPosition = false;

            }

            // Turn false to stop moving platforms
            if (canMovePlatform)
            {
                script.isMovingActive = false;
            }  
        }
    }

    // Method changes the gameObject material color by getting the MeshRenderer
    private void ChangeColor()
    {
        Color chosenColor = colorList[connectionColor.ToString()]; // Get color from dictionary based on selected enum choice

        MeshRenderer buttonMesh = buttonHead.GetComponent<MeshRenderer>();
        MeshRenderer platformMesh = platform.GetComponent<MeshRenderer>();

        buttonMesh.material.color = chosenColor;
        platformMesh.material.color = chosenColor;
    }
}
