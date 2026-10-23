using System.Runtime.CompilerServices;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class CameraDetection : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField]
    private float damage = 0f;

    [Header("Detection Settings")]
    [SerializeField]
    private Light cameraLight;
    [SerializeField]
    private float detectionRadius;
    [SerializeField]
    private float nextDetectCheck;
    [SerializeField]
    private Vector3 offsetDirection;
    [SerializeField]
    private LayerMask detectionMask;

    // Variables for movement for detection
    private float detectCheck = 0f;
    private Vector3 spotlightPos;
    private bool hasHitOnce = false; // Ignore timer when it's first hit 



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get the spotlight position
        spotlightPos = new Vector3(cameraLight.transform.position.x + cameraLight.range + offsetDirection.x,
                                           cameraLight.transform.position.y + offsetDirection.y,
                                           cameraLight.transform.position.z + offsetDirection.z);
    }

    // Provides visual of draw sphere in the scene
    private void OnDrawGizmos()
    {
        // Get the spotlight position
        spotlightPos = new Vector3(cameraLight.transform.position.x + cameraLight.range + offsetDirection.x, 
                                           cameraLight.transform.position.y + offsetDirection.y, 
                                           cameraLight.transform.position.z + offsetDirection.z);

        // Draw wireframe sphere around the posiiton
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(spotlightPos, detectionRadius);
    }


    // Update is called once per frame
    void Update()
    {
        // Use Time.time to perform a detection check every n seconds, where n is nextDetectCheck
        if (Time.time >= detectCheck || !hasHitOnce)
        {
            detectCheck = Time.time + nextDetectCheck;

            // Check if player has entered spotight pos via Physics.CheckSphere and damage them if they did
            if (Physics.CheckSphere(spotlightPos, detectionRadius, detectionMask))
            {
                EventController.DamagePlayerEvent(damage);
                hasHitOnce = true;
            }
        }
    }

}
