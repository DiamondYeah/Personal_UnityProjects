using UnityEngine;
using TMPro;

public class WinTrigger : MonoBehaviour
{

    [Header("Text Controller Settings")]
    public TextMeshProUGUI winText;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        winText.gameObject.SetActive(false); // Disable win text at start
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            winText.gameObject.SetActive(true); // Show win text
        }
    }
}
