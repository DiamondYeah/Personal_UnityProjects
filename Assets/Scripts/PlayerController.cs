using System;
using System.Collections;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Player Stats")]
    [SerializeField]
    private PlayerStats playerStats;

    private Rigidbody rb;

    // Copy of player stats for adjusting values at runtime 
    public PlayerStats runtimeStats { get; set; }

    // x and z movement values in Vector2 for horizontal movement and current speed
    private float xMovement;
    private float zMovement;

    // Values for sprinting and crouching
    private bool isCrouching = false;
    private bool hasAdjustedCrouchPos = false;
    private bool isSprinting = false;

    // y movement values for vertical movement
    private bool pressedJump = false;
    private bool isGrounded;



    // Awake is called when loading an instance of a script component. Mainly used for initializing and instantiating objects
    void Awake()
    {
        runtimeStats = Instantiate(playerStats); // Create a copy of playerStats that will be edited on runtime
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Prevents rigidbody from rotating when moving
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    // Add events to be observed when active
    private void OnEnable()
    {
        EventController.DamagePlayer += DamagePlayer;
    }

    // Disable events when disabled or destroyed as common pracitce for events
    private void OnDisable()
    {
        EventController.DamagePlayer -= DamagePlayer;
    }

    // Update is called once per frame
    void Update()
    {
        // Call functions that determine player speed, and whether they crouched or not
        AdjustPlayerSpeed();
        CrouchPlayer();
    }
    
    // Type of Update that is called at regular, fixed intervals for physics updates
    void FixedUpdate()
    {

        // Call function that determine horizontal movement
        MovePlayer();

        RaycastHit hit; // Hit detection for raycast
        isGrounded = Physics.Raycast(transform.position, Vector3.down, out hit, runtimeStats.groundDetectionDistance);

        // If player is in the ground and jumped, create force that moves the player up
        if (isGrounded && pressedJump)
        {
            JumpPlayer();
        }

    }

    // Method adds force to add horizontal directions for player movement.
    private void MovePlayer()
    {

        //Debug.Log($"isGrounded?: {isGrounded} Crouching?: {isCrouching}, Sprinting?: {isSprinting}");

        Vector3 directionToMove = ((transform.right * xMovement) + (transform.forward * zMovement)).normalized; // Find the normalized direction of player to move
        rb.AddForce(directionToMove * (runtimeStats.currentSpeed + runtimeStats.movementSpeedMultiplier), ForceMode.VelocityChange); // Add force to the direction the player is moving 

        // Check linear velocity magnitude if it has exceeded the maxVelocity, if so, clamp it so that player doesn't continously get faster
        Vector3 rbVeloctiy = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        if (rbVeloctiy.magnitude >= runtimeStats.maxVelocity)
        {
            Vector3 clampedVelocty = rbVeloctiy.normalized * runtimeStats.maxVelocity;
            rb.linearVelocity = new Vector3(clampedVelocty.x, rb.linearVelocity.y, clampedVelocty.z);
        }
    }

    // Method adds force to add horizontal directions for player movement.
    private void AdjustPlayerSpeed()
    {
        // Return sprint/crouch speed multipler if player is sprinting or crouching, else reuturn base speed
        if (isSprinting)
        {
            runtimeStats.currentSpeed = runtimeStats.sprintSpeedMultiplier;
        }
        else if(!isSprinting && isCrouching)
        {
            runtimeStats.currentSpeed = runtimeStats.crouchSpeedMultiplier;
        }
        else
        {
            runtimeStats.currentSpeed = runtimeStats.baseSpeedMultipler;
        }
        
    }

    // Method makes player crouch by making their y hitbox smaller if isCrouching is enabled
    // Kinda scuffed ngl, could be improved upon by changing the center so we dont have to adjust position when crouching
    private void CrouchPlayer()
    {
        if (isCrouching)
        {
            transform.localScale = new Vector3(transform.localScale.x, playerStats.crouchYScale, transform.localScale.z);

            if (!hasAdjustedCrouchPos)
            {
                rb.position -= new Vector3(0f, playerStats.crouchYOffset, 0f);
                hasAdjustedCrouchPos = true;
            } 
        }
        else
        {
            transform.localScale = Vector3.one;

            if (hasAdjustedCrouchPos)
            {
                rb.position += new Vector3(0f, playerStats.crouchYOffset, 0f);
                hasAdjustedCrouchPos = false;
            }
        }
    }

    // Method adds force that goes up to make the player jump. Also works when player is grounded
    private void JumpPlayer()
    {
        rb.AddForce(Vector3.up * runtimeStats.jumpHeight, ForceMode.VelocityChange);
        pressedJump = false;
    }

    // Method reduces health of player depending on damage passed
    private void DamagePlayer(float damage)
    {
        runtimeStats.health -= runtimeStats.health > 0f ? Mathf.Clamp(damage, 0f, damage) : 0f;

        if(runtimeStats.health <= 0f)
        {
            EventController.PlayerLostAllHealthEvent();
        }
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

    // Called by InputAction when input is performed. Gets the values of the input for sprinting if player is grounded
    public void OnSprint(InputValue value)
    {
        isSprinting = value.isPressed && isGrounded && !isCrouching;
    }

    // Called by InputAction when input is performed. Gets the values of the input for sprinting if player is crouching
    public void OnCrouch(InputValue value)
    {
        isCrouching = value.isPressed && isGrounded && !isSprinting;
    }
}
