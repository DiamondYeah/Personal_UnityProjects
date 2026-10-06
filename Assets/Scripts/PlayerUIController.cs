using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;



[Serializable]
public class EffectUI
{

    public GameObject effectContainer;
    public Slider effectSlider;
    public Powerups effectType;

    [NonSerialized]
    public Coroutine effectCoroutine;
}

public class PlayerUIController : MonoBehaviour
{
    [Header("Player Health UI Settings")]
    [SerializeField]
    private Slider hpSlider;

    [Header("Player Level Settings")]
    [SerializeField]
    private TextMeshProUGUI levelText;

    [Header("Player Effects UI Settings")]
    [SerializeField]
    private EffectUI[] effectsUI;

    private GameObject player;
    private PlayerStats playerStats;
    private PlayerStats defaultStats;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get required components
        player = GameObject.FindGameObjectWithTag("Player");
        playerStats = player.GetComponent<PlayerController>().runtimeStats;

        // Create a copy for default stats
        defaultStats = Instantiate(playerStats);

        // Set slider max value and current value to playerStats health
        hpSlider.maxValue = playerStats.health;
        hpSlider.value = playerStats.health;

        // Disable all effect UI at the start
        foreach(EffectUI effect in effectsUI)
        {
            effect.effectContainer.SetActive(false);
        }

    }

    // Add events to be observed when active
    private void OnEnable()
    {
        EventController.DamagePlayer += DecreasePlayerHealth;
        EventController.PlayerHeal += IncreasePlayerHealth;
        EventController.ResetGame += ResetPlayerHealth;
        EventController.ResetGame += ResetPlayerLevel;
        EventController.ResetGame += ClearAllUIEffects;
        EventController.ActivateEffect += EnableEffect;
        EventController.PlayerIsAtEnd += IncrementPlayerLevel;
    }

    // Disable events when disabled or destroyed as common pracitce for events
    private void OnDisable()
    {
        EventController.DamagePlayer -= DecreasePlayerHealth;
        EventController.PlayerHeal -= IncreasePlayerHealth;
        EventController.ResetGame -= ResetPlayerHealth;
        EventController.ResetGame -= ResetPlayerLevel;
        EventController.ResetGame -= ClearAllUIEffects;
        EventController.ActivateEffect -= EnableEffect;
        EventController.PlayerIsAtEnd -= IncrementPlayerLevel;
    }

    // Method lowers the slider's value depending on damage passed
    private void DecreasePlayerHealth(float damage)
    {
        hpSlider.value -= damage;
    }

    // Method resets the player's health display to default values
    private void IncreasePlayerHealth(float healAmount)
    {
        hpSlider.value += healAmount;
    }

    // Method resets the player's health display to default values
    private void IncrementPlayerLevel()
    {
        levelText.text = "Level " + GameController.currentLevel;
    }

    // Method resets the player's health display to default values
    private void ResetPlayerLevel()
    {
        levelText.text = "Level 1";
    }

    // Method resets the player's health display to default values
    private void ResetPlayerHealth()
    {
        hpSlider.maxValue = defaultStats.health;
        hpSlider.value = defaultStats.health;
    }

    // Method enables effect UI depending on what effect was activated
    private void EnableEffect(Powerups powerup, float duration)
    {
        // Goes through the effectsUI array to see if the passed enum matches the type stored in each instance
        foreach (EffectUI effect in effectsUI)
        {
            // If equal activate container
            if (effect.effectType.ToString() == powerup.ToString())
            {
                if(effect.effectCoroutine  != null)
                {
                    StopCoroutine(effect.effectCoroutine);
                }

                // Enable slider and set the slider's value based on duration
                effect.effectContainer.SetActive(true);
                effect.effectSlider.maxValue = duration;
                effect.effectSlider.value = duration;

                effect.effectCoroutine = StartCoroutine(ShowEffectTimer(effect, duration)); // Start coroutine to show how long the effect lasts
            }
        }
    }

    // Coroutine that shows the effect's timer by draining the slider's value 
    private IEnumerator ShowEffectTimer(EffectUI effect, float duration)
    {
        float durationLeft = duration;

        // While loop to create a countdown for the slider
        while(durationLeft > 0f)
        {
            durationLeft -= Time.deltaTime;
            effect.effectSlider.value = Mathf.Clamp(durationLeft, 0f, duration);
            yield return null;
        }

        // Disable UI and coroutine
        DisableEffect(effect); 
        effect.effectCoroutine = null;
    }

    // Method disables effect UI depending on what effect was activated
    private void DisableEffect(EffectUI effect)
    {
        effect.effectContainer.SetActive(false);
    }

    // Method resets all currently running UI effects and hides their UI
    private void ClearAllUIEffects()
    {
        // Goes through the effectsUI array to reset them
        foreach (EffectUI effect in effectsUI)
        {

            if (effect.effectCoroutine != null)
            {
                StopCoroutine(effect.effectCoroutine);
                effect.effectCoroutine = null;
            }

            effect.effectContainer.SetActive(false);

        }

    }

}
