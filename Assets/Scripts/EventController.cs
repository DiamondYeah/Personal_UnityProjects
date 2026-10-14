using System;
using UnityEngine;

public class EventController : MonoBehaviour
{
    public static event Action<float> ObjectThrown;
    public static event Action NoMoreBottles;
    public static event Action NoMoreThrowsLeft;
    public static event Action PlayerWon;
    public static event Action PlayerLoss;
    
    public static void ObjectThrownEvent(float numOfThrowsLeft)
    {
        ObjectThrown?.Invoke(numOfThrowsLeft);
    }

    public static void NoMoreBottlesEvent()
    {
        NoMoreBottles?.Invoke();
    }

    public static void NoMoreThrowsLeftEvent()
    {
        NoMoreThrowsLeft?.Invoke();
    }

    public static void PlayerWonEvent()
    {
        PlayerWon?.Invoke();
    }

    public static void PlayerLossEvent()
    {
        PlayerLoss?.Invoke();
    }
}
