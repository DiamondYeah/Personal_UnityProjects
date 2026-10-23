using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Scriptable Objects/PlayerStats")]
public class PlayerStats : ScriptableObject
{
    [Header("Player Health")]
    public float health;
    public bool isVulnerable;

    [Header("Player Horizontal Movement Settings")]
    public float baseSpeedMultipler;
    public float maxVelocity;

    [Header("Player Sprint Settings")]
    public float sprintSpeedMultiplier;
    [Header("Player Crouch Settings")]
    public float crouchSpeedMultiplier;
    public float crouchYScale;
    public float crouchYOffset;

    [Header("Player Vertical Movement")]
    public float jumpHeight;
    public float groundDetectionDistance = 0.2f;


    [HideInInspector]
    public float movementSpeedMultiplier = 0f;
    [HideInInspector]
    public float currentSpeed = 0f;
}
