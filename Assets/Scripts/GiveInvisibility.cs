using System;
using UnityEngine;

public class GiveInvisibility : MonoBehaviour
{

    [Header("Invisibility Settings")]
    [SerializeField]
    [Range(0f, 1f)]
    private float startingOpacityValue = 0f;
    [SerializeField]
    [Range(0f, 1f)]
    private float visibleOpacityValue = 1f;
    [SerializeField]
    [Range(0f, 1f)]
    private float hiddenOpacityValue = 0f;
    [SerializeField]
    private float colorTransparencyDuration;

    [HideInInspector]
    public bool isVisible { get; set; }

    private MeshRenderer mesh;

    private bool isShowing = false;

    float currentTime = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isVisible = false; // Hide object at start

        mesh = GetComponent<MeshRenderer>(); // Get mesh renderer
        mesh.material.color = mesh.material.color = new Color(mesh.material.color.r, mesh.material.color.g,
            mesh.material.color.b, startingOpacityValue);

    }

    // Update is called once per frame
    void Update()
    {
        // Shows or hides the object depending of isVisible is being set to true by the Flashlight ray
        if(isVisible)
        {
            ShowObject();
        }
        else{
            HideObject();
        }

        // If mesh alpha value is 0/hidden, hide mesh itself. Else, show the mesh
        if(mesh.material.color.a == 0f)
        {
            mesh.enabled = false;
        }
        else
        {
            mesh.enabled = true;
        }
    }


    // Method changes the object's opacity to be visible to player
    private void ShowObject()
    {
        if(currentTime < colorTransparencyDuration)
        {
            currentTime += Time.deltaTime;
            float currentValue = Mathf.Lerp(hiddenOpacityValue, visibleOpacityValue, currentTime / colorTransparencyDuration);
            mesh.material.color = mesh.material.color = new Color(mesh.material.color.r, mesh.material.color.g,
                        mesh.material.color.b, currentValue);
        }
    }

    // Method changes the object's opacity to hide from player
    private void HideObject()
    {
        if (currentTime > 0f)
        {
            currentTime -= Time.deltaTime;
            float currentValue = Mathf.Lerp(hiddenOpacityValue, visibleOpacityValue, currentTime / colorTransparencyDuration);
            mesh.material.color = mesh.material.color = new Color(mesh.material.color.r, mesh.material.color.g,
                        mesh.material.color.b, currentValue);
        }
    }

}
