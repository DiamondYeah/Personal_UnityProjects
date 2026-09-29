using System;
using TMPro;
using UnityEngine;

public class PlatformController : MonoBehaviour
{

    [Header("General Platform Settings")]
    [SerializeField]
    private Color color;

    [Header("Platform Rotate Settings")]
    [SerializeField]
    private Vector3 rotateDirection = Vector3.zero;
    [SerializeField]
    private float rotateSpeed = 1.0f;
    [SerializeField]
    private float rotateCheckBoundary = 0.05f;

    [Header("Platform Movement Settings")]
    [SerializeField]
    private Vector3 startPos = Vector3.zero;
    [SerializeField]
    private Vector3 endPos = Vector3.zero;
    [SerializeField]
    private float travelSpeed = 1.0f;
    [SerializeField]
    private float distanceCheckBoundary = 0.05f;

    public Vector3 basePosition { get; set; }
    public Quaternion baseRotation { get; set; }

    public bool isConnectedToButton { get; set; } = false;
    public bool isRotationActive { get; set; } = false;
    public bool hasRotatedToPosition { get; set; } = false;

    public bool isMovingActive { get; set; } = false;
    public Vector3 startOffset { get; set; }
    public Vector3 endOffset { get; set; }
    private Vector3 currentEularAngle;
    private bool hasEndPostition;
    private bool isGoingBack = false;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!isConnectedToButton)
        {
            ChangeColor(); // Change color of platform
        }
        
        // Get the initial position and rotation of platform
        basePosition = transform.position;
        baseRotation = transform.rotation;

        // Get the inital eular angle/rotation of platform
        currentEularAngle = transform.eulerAngles;

        // Compute for start and end offset if platform is moving
        hasEndPostition = endPos != Vector3.zero;
        if (hasEndPostition)
        {
            transform.position = basePosition + startPos;
            startOffset = transform.position;
            endOffset = basePosition + endPos;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isConnectedToButton && isRotationActive && !hasRotatedToPosition)
        {
            RotatePlatform();
        }
    }

    // Type of Update that is called at regular, fixed intervals for physics updates
    void FixedUpdate()
    {
        if(!isConnectedToButton || (isConnectedToButton && isMovingActive))
        {
            MovePlatform();
        }
    }

    // OnTriggerStay is called when a collider continues to collide with the trigger
    private void OnTriggerStay(Collider other)
    {
        if(hasEndPostition && other.gameObject.CompareTag("Player"))
        {
            other.transform.parent = transform;
        }
    }

    // OnTriggerExit is called when a collider exits the trigger
    private void OnTriggerExit(Collider other)
    {
        if(hasEndPostition && other.gameObject.CompareTag("Player"))
        {
            other.transform.parent = null;
        }
    }

    // Method that uses Quaternions to rotate the object smoothly towards the newRotation destination
    public void RotatePlatform()
    {
        // Compute for the rotation location via Euler and perform a Quaternion.RotateTowards for a smooth rotation
        Quaternion newRotation = Quaternion.Euler(currentEularAngle + rotateDirection);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation, rotateSpeed * Time.deltaTime);

        // If platform has rotated to the desired rotation, add upon currentEularAngle to allow rotation again
        if(Quaternion.Angle(transform.rotation, newRotation) <= rotateCheckBoundary)
        {
            currentEularAngle += rotateDirection;
            hasRotatedToPosition = true;
        }
    }

    // Method moves platform from startOffset to endOffset and vice versa via MoveTowards
    public void MovePlatform()
    {
        // Moves towards the offset if the Distance between the platform's position and the offset is greater than the boundary check
        if(Vector3.Distance(transform.position, endOffset) > distanceCheckBoundary && !isGoingBack)
        {
            transform.position = Vector3.MoveTowards(transform.position, endOffset, travelSpeed * Time.fixedDeltaTime);
        }
        else if(Vector3.Distance(transform.position, startOffset) > distanceCheckBoundary && isGoingBack)
        {
            transform.position = Vector3.MoveTowards(transform.position, startOffset, travelSpeed * Time.fixedDeltaTime);
        }

        // Toggles isGoingBack depending if the distance between the platform's position and the offset is less than the boundary check
        if(Vector3.Distance(transform.position, endOffset) < distanceCheckBoundary && !isGoingBack)
        {
            isGoingBack = true; // Move back to startOffset
        }
        else if(Vector3.Distance(transform.position, startOffset) < distanceCheckBoundary && isGoingBack)
        {
            isGoingBack = false; // Move back to endOffset
        }
    }

    // Method reset all values for a fresh state for rotating and moving platforms
    public void ResetTransformation()
    {
        hasRotatedToPosition = false;
        isRotationActive = false;
        isMovingActive = false;
        isGoingBack = false;

        currentEularAngle = transform.eulerAngles;
    }

    // Method changes the gameObject material color by getting the MeshRenderer
    private void ChangeColor()
    {
        GetComponent<MeshRenderer>().material.color = color;
    }

}
