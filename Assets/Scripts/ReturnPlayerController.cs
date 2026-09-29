using System;
using UnityEngine;

public class ReturnPlayerController : MonoBehaviour
{

    [Header("Reset Player Settings")]
    [SerializeField]
    private Vector3 resetPos;
    [SerializeField]
    private float resetSpeed;
    [SerializeField]
    private float distanceCheckBoundary = 0.05f;

    private GameObject player;
    private bool isResettingPlayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        ReturnPlayerToSpawn();
    }

    // OnTriggerEnter is called when a collider enters the trigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isResettingPlayer = true;
        }
    }

    // Method returns the player position to the start position via MoveTowards
    private void ReturnPlayerToSpawn()
    {
        if (isResettingPlayer)
        {
            // Get player components that will be disabled when moving them towards starting area
            CapsuleCollider collider = player.GetComponent<CapsuleCollider>();
            PlayerController movementScript = player.GetComponent<PlayerController>();
            Rigidbody rb = player.GetComponent<Rigidbody>();

            if (Vector3.Distance(player.transform.position, resetPos) > distanceCheckBoundary)
            {
                player.transform.position = Vector3.MoveTowards(player.transform.position, resetPos, resetSpeed * Time.deltaTime);

                // Disable collider, movement script, useGravity, and reset velocity when moving towards reset position
                if (collider)
                {
                    collider.enabled = false;
                }
                if (movementScript)
                {
                    movementScript.enabled = false;
                }
                if (rb)
                {
                    rb.useGravity = false;
                    rb.linearVelocity = Vector3.zero;
                }
            }
            else
            {   // Enable everything back
                collider.enabled = true;
                movementScript.enabled = true;
                rb.useGravity = true;
                isResettingPlayer = false;
            }
        }
    }
}
