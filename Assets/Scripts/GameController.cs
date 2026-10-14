using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private int numOfStandToBeat = 2;

    private int beatenCounter = 0;

    // Add events to be observed when active
    private void OnEnable()
    {
        EventController.NoMoreBottles += IncrementNumOfStands;
        EventController.NoMoreThrowsLeft += PlayerHasLost;
    }

    // Disable events when disabled or destroyed as common pracitce for events
    private void OnDisable()
    {
        EventController.NoMoreBottles -= IncrementNumOfStands;
        EventController.NoMoreThrowsLeft -= PlayerHasLost;
    }

    // Method increments numOfStandToBeat whenever a stand is beaten
    public void IncrementNumOfStands()
    {
        beatenCounter++;

        if (beatenCounter >= numOfStandToBeat)
        {
            PlayerHasWon();
        }
    }

    // Method calls event controller to invoke player win event
    public void PlayerHasWon()
    {
        beatenCounter = 0;
        EventController.PlayerWonEvent();
    }

    // Method calls event controller to invoke player loss event
    public void PlayerHasLost()
    {
        beatenCounter = 0;
        EventController.PlayerLossEvent();
    }
}
