using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIThrowController : MonoBehaviour
{
    [Header("UI Settings")]
    [SerializeField]
    private TextMeshProUGUI numOfThrowablesUI;
    [SerializeField]
    private TextMeshProUGUI endText;
    [SerializeField]
    private Button restartButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Hide end text and restart button when game starts
        endText.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);

        // Restart Scene if restart button is pressed
        restartButton.onClick.AddListener(() => { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); });
    }

    // Add events to be observed when active
    private void OnEnable()
    {
        EventController.ObjectThrown += UpdateThrowCount;
        EventController.PlayerWon += ShowWinUI;
        EventController.PlayerLoss += ShowLoseUI;

    }

    // Disable events when disabled or destroyed as common pracitce for events
    private void OnDisable()
    {
        EventController.ObjectThrown -= UpdateThrowCount;
        EventController.PlayerWon -= ShowWinUI;
        EventController.PlayerLoss -= ShowLoseUI;
    }

    // Method updates the counter of number of throws left
    private void UpdateThrowCount(float numOfThrowsLeft)
    {
        numOfThrowablesUI.text = "Balls Left: " + numOfThrowsLeft; 
    }

    // Method updates UI and displays winning text and restart button
    private void ShowWinUI()
    {
        // Makes cursor and visible and movable
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        endText.text = "You Win!";
        endText.color = Color.darkGreen;
        restartButton.gameObject.SetActive(true);
        endText.gameObject.SetActive(true);
    }

    // Method updates UI and displays losing text and restart button
    private void ShowLoseUI()
    {
        // Makes cursor and visible and movable
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        endText.text = "You Lost!";
        endText.color = Color.darkRed;
        restartButton.gameObject.SetActive(true);
        endText.gameObject.SetActive(true);
    }

}
