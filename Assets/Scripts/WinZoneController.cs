using UnityEngine;

public class WinZoneController : MonoBehaviour
{

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Check OnTrigger if player has entered trigger to invoke event for player reaching the end
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            EventController.PlayerReachedEnd();
        }
    }
}
