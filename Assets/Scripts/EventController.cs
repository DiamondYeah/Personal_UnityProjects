using System;
using UnityEngine;

public class EventController : MonoBehaviour
{

    public static event Action<float> DamagePlayer;
    public static event Action PlayerLostAllHealth;
    public static event Action ResetGame;
    public static event Action<float> PlayerHeal;
    public static event Action<Powerups, float> ActivateEffect;
    public static event Action PlayerIsAtEnd;

    public static void DamagePlayerEvent(float damage)
    {
        DamagePlayer?.Invoke(damage);
    }

    public static void HealPlayerEvent(float healAmount)
    {
        PlayerHeal?.Invoke(healAmount);
    }

    public static void PlayerLostAllHealthEvent()
    {
        PlayerLostAllHealth?.Invoke();
    }

    public static void ResetGameEvent()
    {
        ResetGame?.Invoke();
    }

    public static void ActivatePlayerEffect(Powerups powerup, float duration)
    {
        ActivateEffect?.Invoke(powerup, duration);
    }

    public static void PlayerReachedEnd()
    {
        PlayerIsAtEnd?.Invoke();
    }
}
