using UnityEngine;

public class GameController : MonoBehaviour
{
    public static int currentLevel = 1;

    [Header("Reset Game Settings")]
    [SerializeField]
    private Vector3 playerResetLoc;

    private GameObject player;
    private PlayerStats playerStats;

    private bool isResetting = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get required components
        player = GameObject.FindGameObjectWithTag("Player");
        playerStats = player.GetComponent<PlayerController>().runtimeStats; 
    }

    // Update is called once per frame
    void Update()
    {
        // Reset game if player's health is zero
        if(playerStats.health <= 0f && !isResetting)
        {
            ResetGame();
        }
    }

    // Add events to be observed when active
    private void OnEnable()
    {
        EventController.PlayerLostAllHealth += ResetGame;
        EventController.PlayerIsAtEnd += ProgressToNextLevel;
    }

    // Disable events when disabled or destroyed as common pracitce for events
    private void OnDisable()
    {
        EventController.PlayerLostAllHealth -= ResetGame;
        EventController.PlayerIsAtEnd -= ProgressToNextLevel;
    }

    // Method handles resetting the game
    private void ResetGame()
    {
        if (isResetting)
        {
            return;
        }
        isResetting = true;


        currentLevel = 1;

        EventController.ResetGameEvent();

        player.transform.position = playerResetLoc; // Reset player's position to playerResetLoc

        // Destroy all lasers
        GameObject[] lasers = GameObject.FindGameObjectsWithTag("Laser");
        foreach (GameObject laser in lasers)
        {
            Destroy(laser);
        }
        
        playerStats = player.GetComponent<PlayerController>().runtimeStats; // Get runTimeStats for player again to reset values
    }

    // Method handles resetting the game
    public void ProgressToNextLevel()
    {
        currentLevel++; // Increment level

        player.transform.position = playerResetLoc; // Reset player's position to playerResetLoc

        // Destroy all lasers
        GameObject[] lasers = GameObject.FindGameObjectsWithTag("Laser");
        foreach (GameObject laser in lasers)
        {
            Destroy(laser);
        }

        // Destroy all powerup panels
        GameObject[] panels = GameObject.FindGameObjectsWithTag("Powerup");
        foreach (GameObject panel in panels)
        {
            Destroy(panel);
        }

        isResetting = false;
    }
}
