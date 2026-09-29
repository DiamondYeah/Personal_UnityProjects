using System;
using Unity.VisualScripting;
using UnityEngine;

public class ResetTrigger : MonoBehaviour
{

    [Header("Reset Button Settings")]
    [SerializeField]
    private GameObject buttonHead;
    [SerializeField]
    private GameObject[] platforms;
    [SerializeField]
    private Color color;

    [Space(20)]
    [SerializeField]
    private float resetPositionSpeed;
    [SerializeField]
    private float resetRotationSpeed;
    [SerializeField]
    private float distanceCheckBoundary = 0.05f;

    
    private bool hasPressedReset = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ChangeColor();
    }

    // Update is called once per frame
    void Update()
    {
        if (hasPressedReset)
        {
            ResetPlatforms();
        }
        
    }
    
    // OnTriggerEnter is called when a collider enters the trigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            hasPressedReset = true; 
        }
    }

    // Method resets all platforms rotation and position
    private void ResetPlatforms()
    {
        Boolean isAllReset = true;

        // Iterate through all platforms and reset both their rotation and positions
        foreach (GameObject platform in platforms)
        {
            // Obtain the platform movement script
            PlatformController platformScript = platform.GetComponent<PlatformController>();

            // Get base rotations and set RotateTowards and MoveTowards to go to their defaults
            Quaternion defaultRotation = platformScript.baseRotation;
            Vector3 defaultPosition = platformScript.basePosition;
            platform.transform.rotation = Quaternion.RotateTowards(platform.transform.rotation, defaultRotation, resetRotationSpeed * Time.deltaTime);
            platform.transform.position = Vector3.MoveTowards(platform.transform.position, defaultPosition, resetPositionSpeed * Time.deltaTime);

            // Check if platform's distance or angle has not rached their default, if not, isAllReset is false
            if (Vector3.Distance(platform.transform.position, defaultPosition) > distanceCheckBoundary || 
                Quaternion.Angle(platform.transform.rotation, defaultRotation) > distanceCheckBoundary)
            {
                isAllReset = false;
            }

        }

        if (isAllReset)
        {
            hasPressedReset = false;

            // Reset variables for each platform by calling ResetTransformation()
            foreach (GameObject platform in platforms)
            {
                platform.GetComponent<PlatformController>().ResetTransformation();
            }
        }
    }

    // Method changes the gameObject material color by getting the MeshRenderer
    private void ChangeColor()
    {
        MeshRenderer buttonMesh = buttonHead.GetComponent<MeshRenderer>();

        buttonMesh.material.color = color;
    }

    
}
