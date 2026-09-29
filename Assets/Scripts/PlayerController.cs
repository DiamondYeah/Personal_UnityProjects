using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Player Movement Settings")]
    [SerializeField]
    private float movementSpeedMultiplier;
    [SerializeField]
    private float maxVelocity;
    [SerializeField]
    private float jumpHeight;
    [SerializeField]
    private float groundDetectionDistance = 0.2f;

    private Rigidbody rb;

    // x and z movement values in Vector2 for horizontal movement
    private float xMovement;
    private float zMovement;

    // y movement values for vertical movement
    private Boolean pressedJump = false;
    private Boolean isGrounded;
    private float yMovement;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.freezeRotation = true; // Prevents rigidbody from rotating when moving
    }

    // Type of Update that is called at regular, fixed intervals for physics updates
    void FixedUpdate()
    {
        MovePlayer();

        RaycastHit hit; // Hit detection for raycast
        isGrounded = Physics.Raycast(transform.position, Vector3.down, out hit, groundDetectionDistance);

        // If player is in the ground and jumped, create force that moves the player up
        if (isGrounded && pressedJump)
        {
            JumpPlayer();
        }
    }

    // Method adds force to add horizontal directions for player movement.
    void MovePlayer()
    {
        // Calculatess the force to be moved to rigidBody. movementSpeed adjusts how the strength of force
        Vector3 movementForce = new Vector3(xMovement * movementSpeedMultiplier, 0, zMovement * movementSpeedMultiplier);
        rb.AddForce(movementForce, ForceMode.VelocityChange); // Add force to rigidbody to create movement depending on x and z movement

        // Clamp velocity of x and z axis to prevent speeding up infinitely
        Vector3 currentVelocity = rb.linearVelocity;
        currentVelocity.x = Mathf.Clamp(xMovement, -maxVelocity, maxVelocity);
        currentVelocity.z = Mathf.Clamp(zMovement, -maxVelocity, maxVelocity);
        rb.linearVelocity = currentVelocity;
    }

    // Method adds force that goes up to make the player jump. Also works when player is grounded
    void JumpPlayer()
    {
        rb.AddForce(Vector3.up * jumpHeight, ForceMode.VelocityChange);
        pressedJump = false;
    }


    // Called by InputAction when input is performed. Gets the values of the input for horizontal movement
    public void OnMove(InputValue value)
    {
        Vector2 movementVector = value.Get<Vector2>();

        xMovement = movementVector.x;
        zMovement = movementVector.y;
    }

    // Called by InputAction when input is performed. Gets the values of the input for vertical movement
    public void OnJump(InputValue value)
    {
        pressedJump = true;
    }

}
