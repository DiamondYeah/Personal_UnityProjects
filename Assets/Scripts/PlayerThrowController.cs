using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerThrowController : MonoBehaviour
{


    
    [Header("Spawn Instances Settings")]
    [SerializeField]
    private Camera playerCamera;
    [SerializeField]
    private GameObject throwableObject;

    [Header("Throwable Settings")]
    [SerializeField]
    private float throwStrength;
    [SerializeField]
    private float numOfThrows;
    [SerializeField]
    private Vector3 throwingOffset; // How far does the object spawn from player's position

    private GameObject throwableInstance;
    private bool throwDisabled = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerCamera.gameObject.transform.SetParent(gameObject.transform); // Make player parent of camera

        EventController.ObjectThrownEvent(numOfThrows); // Set initial number of throws
    }

    // Add events to be observed when active
    private void OnEnable()
    {
        EventController.PlayerWon += () => { throwDisabled = true; };
        EventController.PlayerLoss += () => { throwDisabled = true; };
    }

    // Disable events when disabled or destroyed as common pracitce for events
    private void OnDisable()
    {
        EventController.PlayerWon -= () => { throwDisabled = true; };
        EventController.PlayerLoss -= () => { throwDisabled = true; };
    }

    // Update is called once per frame
    void Update()
    {
        bool wasLeftMouseClicked = Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame;
        bool hasThrowsLeft = numOfThrows != 0;

        // If left mouse was clicked and player still has throws, allow object to be thrown
        if (wasLeftMouseClicked && hasThrowsLeft && !throwDisabled)
        {
            ThrowObject();
        }
        else if (!hasThrowsLeft)
        {
            EventController.NoMoreThrowsLeftEvent();
        }
    }


    // Method create instance of throwable object to be thrown
    private void ThrowObject()
    {
        // Find position for throwable and instantiate it to where the player is looking
        Vector3 instanceSpawnPos = playerCamera.transform.position + playerCamera.transform.TransformDirection(throwingOffset);
        throwableInstance = Instantiate(throwableObject, instanceSpawnPos, Quaternion.Euler(transform.localEulerAngles));

        // Add force to move the throwable forward like its being thrown
        Rigidbody throwableRB = throwableInstance.GetComponent<Rigidbody>();
        throwableRB.AddForce(playerCamera.transform.forward * throwStrength, ForceMode.Impulse);

        // Decrement number of throws and call ObjectThrownEvent
        numOfThrows--; 
        EventController.ObjectThrownEvent(numOfThrows);
    }
}
