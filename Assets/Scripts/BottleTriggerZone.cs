using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class BottleTriggerZone : MonoBehaviour
{
    [Header("UI Settings")]
    [SerializeField]
    private GameObject bottlesCounterDisplay;

    private HashSet<Collider> bottlesInZone = new HashSet<Collider>();
    private TextMeshPro displayText;

    private bool hasBeaten = false;
    private bool hasBottlesSpawned = false; // Check for when restarting scene to not immediately trigger win condition

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        displayText = bottlesCounterDisplay.GetComponent<TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {
        displayText.text = bottlesInZone.Count.ToString(); // Updates display text with number of bottles left standing

        if(hasBottlesSpawned && bottlesInZone.Count <= 0 && !hasBeaten)
        {
            EventController.NoMoreBottlesEvent();
            hasBeaten = true;
        }
    }

    // Trigger Zone checks for how any bottles exists in its zone (bottles that havent been knocked down yet)
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bottle"))
        {
            bottlesInZone.Add(other);
            hasBottlesSpawned = true;
        }

    }

    // Trigger Zone checks for how any bottles exists in its zone (bottles that havent been knocked down yet)
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Bottle"))
        {
            bottlesInZone.Remove(other);
        }

    }
}
