using UnityEngine;

public class LaserMove : MonoBehaviour
{

    [SerializeField]
    private LaserStats laserStats;



    private bool isSideGoingBack = false;
    private bool isVertGoingBack = false;

    private Transform startSideLoc;
    private Transform endSideLoc;
    private Transform startVertLoc;
    private Transform endVertLoc;

    // Method called for initializing various transform positions for moving
    public void Initialize(Transform sSideLoc, Transform eSideLoc, Transform sVertLoc, Transform eVertLoc)
    {
        this.startSideLoc = sSideLoc;
        this.endSideLoc = eSideLoc;
        this.startVertLoc = sVertLoc;
        this.endVertLoc = eVertLoc;

    }

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
    }

    // Method calls event when player hits laser, dealing damage to player
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && other.GetComponent<PlayerController>().runtimeStats.isVulnerable)
        {
            EventController.DamagePlayerEvent(laserStats.damage);
        }

        if (other.gameObject.CompareTag("Despawn")) 
        {
            DestroyLaser();
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
