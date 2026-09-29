using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Player Object")]
    [SerializeField]
    private GameObject player;

    [Header("Camera Follow Settings")]
    [SerializeField]
    private Vector3 offset;
    [SerializeField]
    private float cameraSpeed;

    // Type of Update that is called after Update(), used for camera movement
    void LateUpdate()
    {
        Vector3 cameraPos = player.transform.position + offset; // Camera's position relative to player's position via offset

        // Smoothly follows player via Vector3.Lerp
        transform.position = Vector3.Lerp(transform.position, cameraPos, cameraSpeed * Time.deltaTime);
    }
}
