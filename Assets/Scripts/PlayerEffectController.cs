using UnityEngine;
using System.Collections;

public class PlayerEffectController : MonoBehaviour
{
    private PlayerStats runtimeStats;

    // Store coroutines fields that will be called for checking
    private Coroutine speedUpCoroutine;
    private Coroutine jumpUpCoroutine;
    private Coroutine beInvincibleCoroutine;

    // Store speed and jump boost for resetting in conflicts
    private float currentSpeedBoost;
    private float currentJumpBoost;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        runtimeStats = gameObject.GetComponent<PlayerController>().runtimeStats;
    }

    // Add events to be observed when active
    private void OnEnable()
    {
        EventController.ResetGame += ClearEffects;
    }

    // Disable events when disabled or destroyed as common pracitce for events
    private void OnDisable()
    {
        EventController.ResetGame -= ClearEffects;
    }

    // Method stops all current coroutines and resets all current buffs
    void ClearEffects()
    {
        StopAllCoroutines();
        speedUpCoroutine = null;
        jumpUpCoroutine = null;
        beInvincibleCoroutine = null;
}


    // Method reduces health of player depending on damage passed
    public void HealPlayer(float healAmount)
    {
        runtimeStats.health += healAmount;
    }

    // Method speeds up player with a given speed boost based on a certain duration (Assuming its not already in effect)
    public void SpeedUp(float speedBoost, float duration)
    {
        // Check if coroutine is active (Effect is active)
        if(speedUpCoroutine != null)
        {
            StopCoroutine(speedUpCoroutine);
            runtimeStats.movementSpeedMultiplier -= currentSpeedBoost;
        }

        speedUpCoroutine = StartCoroutine(SpeedUpRoutine(speedBoost, duration)); // Start coroutine to apply effect
    }

    // Method performs coroutine for SpeedUp
    public IEnumerator SpeedUpRoutine(float speedBoost, float duration)
    {
        currentSpeedBoost = speedBoost;
        runtimeStats.movementSpeedMultiplier += speedBoost;
        yield return new WaitForSeconds(duration);
        runtimeStats.movementSpeedMultiplier -= speedBoost;

        speedUpCoroutine = null; // Remove Coroutine
    }

    // Method makes player jump higher with a given jump boost based on a certain duration (Assuming its not already in effect)
    public void JumpUp(float jumpBoost, float duration)
    {
        // Check if coroutine is active (Effect is active)
        if (jumpUpCoroutine != null)
        {
            StopCoroutine(jumpUpCoroutine);
            runtimeStats.jumpHeight -= currentJumpBoost;
        }

        jumpUpCoroutine = StartCoroutine(JumpUpRoutine(jumpBoost, duration)); // Start coroutine to apply effect
    }

    // Method performs coroutine for JumpUp
    public IEnumerator JumpUpRoutine(float jumpBoost, float duration)
    {
        currentJumpBoost = jumpBoost;
        runtimeStats.jumpHeight += jumpBoost;
        yield return new WaitForSeconds(duration);
        runtimeStats.jumpHeight -= jumpBoost;

        jumpUpCoroutine = null; // Remove Coroutine
    }

    /// Method makes player take no damage from laser based on a certain duration (Assuming its not already in effect)
    public void BeInvincible(float duration)
    {
        // Check if coroutine is active (Effect is active)
        if (beInvincibleCoroutine != null)
        {
            StopCoroutine(beInvincibleCoroutine);
        }

        beInvincibleCoroutine = StartCoroutine(BeInvincibleRoutine(duration)); // Start coroutine to apply effect
    }

    // Method performs coroutine for JumpUp
    public IEnumerator BeInvincibleRoutine(float duration)
    {
        runtimeStats.isVulnerable = false;
        yield return new WaitForSeconds(duration);
        runtimeStats.isVulnerable = true;

        beInvincibleCoroutine = null; // Remove Coroutine
    }
}
