using UnityEngine;

[CreateAssetMenu(fileName = "LaserStats", menuName = "Scriptable Objects/LaserStats")]
public class LaserStats : ScriptableObject
{
    [Header("General Laser Stats")]
    public float damage;
    public float forwardSpeed;

    [Header("Moving Horizontal Laser Stats")]
    public bool canMoveSideways;
    public float sidewaysDistanceBoundary;
    public float sidewaysSpeed;

    [Header("Moving Vertical Laser Stats")]
    public bool canMoveVertical;
    public float verticalDistanceBoundary;
    public float verticalSpeed;
}
