using System;
using System.Collections.Generic;
using UnityEngine;

// Make a public enum for all of the current powerups so that other scripts can reference this lists
public enum Powerups { HealthGain, SpeedBoost, JumpBoost, Invulnerability};

public class GivePowerup : MonoBehaviour
{
    [Header("Powerup Selector")]
    [SerializeField]
    private Powerups powerup;

    [Header("HealthGain Settings")]
    [SerializeField]
    private float healthGain;

    [Header("SpeedBoost Settings")]
    [SerializeField]
    private float speedBoost;
    [SerializeField]
    private float speedBoostDuration;

    [Header("JumpBoost Settings")]
    [SerializeField]
    private float jumpBoost;
    [SerializeField]
    private float jumpBoostDuration;

    [Header("Invulnerability Settings")]
    [SerializeField]
    private float invulnerabilityDuration;


    private Dictionary<string, Action<PlayerEffectController>> powerSelector;


    // Awake is called when loading an instance of a script component. Mainly used for initializing and instantiating objects
    void Awake()
    {
        powerSelector = new Dictionary<string, Action<PlayerEffectController>>
        {
            {"HealthGain",  GiveHealth},
            {"SpeedBoost", IncreaseSpeed},
            {"JumpBoost", IncreaseJump},
            {"Invulnerability", MakePlayerInvulenrable},
        };
    }

    // Helper method that gets the duration of the effect depending on the selected powerup
    private float getEffectDuration()
    {
        switch (powerup.ToString())
        {
            case "SpeedBoost":
                return speedBoostDuration;
            case "JumpBoost":
                return jumpBoostDuration;
            case "Invulnerability":
                return invulnerabilityDuration;
            default: 
                return 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerEffectController playerEffects = other.GetComponent<PlayerEffectController>();
            powerSelector[powerup.ToString()].Invoke(playerEffects); // Invoke powerup from dictionary
            EventController.ActivatePlayerEffect(powerup, getEffectDuration());
        }
    }

    void GiveHealth(PlayerEffectController playerEffects)
    {
        playerEffects.HealPlayer(healthGain);
        EventController.HealPlayerEvent(healthGain);
    }

    void IncreaseSpeed(PlayerEffectController playerEffects)
    {
        playerEffects.SpeedUp(speedBoost, speedBoostDuration);
    }

    void IncreaseJump(PlayerEffectController playerEffects)
    {
        playerEffects.JumpUp(jumpBoost, jumpBoostDuration);
    }

    void MakePlayerInvulenrable(PlayerEffectController playerEffects)
    {
        playerEffects.BeInvincible(invulnerabilityDuration);
    }
}
