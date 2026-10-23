using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class FlashlightController : MonoBehaviour
{
    [Header("Detection Settings")]
    [SerializeField]
    private Light flashlightLight;
    [SerializeField]
    private LayerMask validInivisiblityMasks;
    [SerializeField]
    private float spotRange = 0f;
    [SerializeField]
    private float detectionRadius = 0f;

    [Header("Light Settings")]
    [SerializeField]
    private float lightIntensity = 0f;


    private Vector3 flashlightDetectArea;
    private GiveInvisibility heldInvisibilityScript;
    private bool isFlashlightToggled = false;
    private List<Collider> heldColliders = new List<Collider>();
    private List<Collider> hitColliders = new List<Collider>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Set flashlight to camera so it moves with camera
        transform.SetParent(Camera.main.transform);

        flashlightLight.range = spotRange;

        ToggleLight(false); // Set flashlight to closed at start
    }

    // Provides visual of draw sphere in the scene
    private void OnDrawGizmos()
    {
        // Get the flashlight's spotlight position
        flashlightDetectArea = flashlightLight.transform.position + flashlightLight.transform.forward * spotRange;

        // Draw wireframe sphere around the posiiton
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(flashlightDetectArea, detectionRadius);
    }

    // Update is called once per frame
    void Update()
    {
        // Visualize raycast in scene
        Debug.DrawRay(flashlightLight.transform.position, flashlightLight.transform.forward * spotRange, Color.green);

        // Detect if left mouse was clicked, if it is, toggle flashlight
        bool wasLeftMouseClicked = Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame;
        if (wasLeftMouseClicked)
        {
            isFlashlightToggled = !isFlashlightToggled;
            ToggleLight(isFlashlightToggled); // Toggle's light
        }

        // If flashlight is on, start with detection
        if (isFlashlightToggled)
        {
            DetectWithFlashlight();
        }
    }

    // Method toggles flashlight's light on or off by adjusting intensity and turning any colliders invisible if off
    private void ToggleLight(bool toggle)
    {
        if (toggle)
        {
            flashlightLight.intensity = lightIntensity;
        }
        else
        {
            flashlightLight.intensity = 0f;

            // If currently holding hit colliders, set visibility to false and set hitColliders null to let invisible object hide
            if (heldColliders != null && heldColliders.Count > 0)
            {
                // For each collider, accesss their script to make them hidden
                foreach (Collider hit in heldColliders)
                {
                    heldInvisibilityScript = hit.GetComponent<Collider>().gameObject.GetComponent<GiveInvisibility>();
                    heldInvisibilityScript.isVisible = false;
                }

                heldColliders.Clear();

            }
        }

    }

    // Method uses Overlap sphere to find invisible objects within sphere radius
    private void DetectWithFlashlight()
    {
        // Get the flashlight's spotlight position
        flashlightDetectArea = flashlightLight.transform.position + flashlightLight.transform.forward * spotRange;

        // Get all colliders within sphere
        hitColliders = new List<Collider>(Physics.OverlapSphere(flashlightDetectArea, detectionRadius, validInivisiblityMasks));

        if( hitColliders.Count > 0)
        {
            // For each collider, accesss their script to make them visible
            foreach (Collider hit in hitColliders)
            {
                heldInvisibilityScript = hit.GetComponent<Collider>().gameObject.GetComponent<GiveInvisibility>();
                heldInvisibilityScript.isVisible = true;
                
                // If hit isn't already within the held list, add it to the list
                if (!heldColliders.Contains(hit))
                {
                    heldColliders.Add(hit);
                }
            }

            return;
        }
        else
        {
            // Check for any held colliders to turn invisible
            if(heldColliders != null && heldColliders.Count > 0)
            {
                // For each collider, accesss their script to make them invisible
                foreach (Collider hit in heldColliders)
                {
                    heldInvisibilityScript = hit.GetComponent<Collider>().gameObject.GetComponent<GiveInvisibility>();
                    heldInvisibilityScript.isVisible = false;
                }
            }

        }
    }
}
