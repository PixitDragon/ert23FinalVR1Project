using UnityEngine;

public class MaterialChanger : MonoBehaviour
{
    // The Renderer component on this object
    private Renderer objectRenderer;

    // Materials to switch between (assign these in the Inspector)
    public Material initialMaterial;
    public Material newMaterial;

    void Awake()
    {
        // Get the Renderer component when the object starts
        objectRenderer = GetComponent<Renderer>();

        // Set the initial material
        if (objectRenderer != null && initialMaterial != null)
        {
            objectRenderer.material = initialMaterial;
        }
    }

    /// <summary>
    /// Public method to be called by the signaling script.
    /// </summary>
    public void ChangeToNewMaterial()
    {
        if (objectRenderer != null && newMaterial != null)
        {
            objectRenderer.material = newMaterial;
            Debug.Log(gameObject.name + " material changed!");
        }
        else
        {
            Debug.LogError("Missing Renderer or New Material.");
        }
    }
}