using Unity.VisualScripting;
using UnityEngine;

public class LaserMove : MonoBehaviour
{

    [SerializeField]
    private LaserStats laserStats;

    [SerializeField]
    private Transform startSideLoc;
    [SerializeField]
    private Transform endSideLoc;
    [SerializeField]
    private Transform startVertLoc;
    [SerializeField]
    private Transform endVertLoc;

    // Variables for laser detection
    private float detectCheck = 0f;
    private bool hasHitOnce = false; // Ignore timer when it's first hit 

    private bool isSideGoingBack = false;
    private bool isVertGoingBack = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        MoveForward();
        MoveSideways();
        MoveVertical();

        // Visualize raycast in scene for top and bottom of laser
        Debug.DrawRay(transform.position, Vector3.down * transform.position.y, Color.blue);
        Debug.DrawRay(transform.position, Vector3.up * transform.position.y, Color.blue);

        // Use Time.time to perform a detection check every n seconds, where n is nextDetectCheck
        if (Time.time >= detectCheck || !hasHitOnce)
        {
            detectCheck = Time.time + laserStats.nextDetectCheck;

            // Check if player has bit hit by laser via physics.raycast in both directions and damage them if so
            RaycastHit hit; // Hit detection for raycast
            if (Physics.Raycast(transform.position, Vector3.down, out hit, transform.position.y, laserStats.detectionMask) ||
                Physics.Raycast(transform.position, Vector3.up, out hit, transform.position.y, laserStats.detectionMask))
            {
                EventController.DamagePlayerEvent(laserStats.damage);
                hasHitOnce = true;
            }
        }

    }

    // Method moves the laser forward based on laserStats forward speed
    private void MoveForward()
    {
        transform.Translate(Vector3.forward * laserStats.forwardSpeed * Time.deltaTime);
    }

    // Method moves the laser side-to-side based on side transform position and laserStats side speed
    private void MoveSideways()
    {
        if (laserStats.canMoveSideways)
        {
             float xMovePos = isSideGoingBack ? startSideLoc.position.x : endSideLoc.position.x;
            Vector3 movePos = new Vector3(xMovePos, transform.position.y, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, movePos, laserStats.sidewaysSpeed * Time.fixedDeltaTime);

            // Toggles isGoingBack depending if the distance between the lasers's position and the offset is less than the boundary check
            if (Vector3.Distance(transform.position, movePos) < laserStats.sidewaysDistanceBoundary)
            {
                isSideGoingBack = !isSideGoingBack;
            }
        }
    }

    // Method moves the laser up-and-down based on vertical transform position and laserStats side speed
    private void MoveVertical()
    {
        if (laserStats.canMoveVertical)
        {
            float yMovePos = isVertGoingBack ? startVertLoc.position.y : endVertLoc.position.y;
            Vector3 movePos = new Vector3(transform.position.x, yMovePos, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, movePos, laserStats.verticalSpeed * Time.fixedDeltaTime);

            // Toggles isGoingBack depending if the distance between the lasers's position and the offset is less than the boundary check
            if (Vector3.Distance(transform.position, movePos) < laserStats.verticalDistanceBoundary)
            {
                isVertGoingBack = !isVertGoingBack;
            }
        }
    }

    // Method destroys gameObject if called
    private void DestroyLaser()
    {
        Destroy(gameObject);
    }
}
