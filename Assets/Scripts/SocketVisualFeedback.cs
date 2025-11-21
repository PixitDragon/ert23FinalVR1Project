using UnityEngine;

/// <summary>
/// Handles changing the color of a Renderer based on external events.
/// Designed to be called by the SocketOrderChecker's UnityEvents.
/// </summary>
public class SocketVisualFeedback : MonoBehaviour
{
    [Header("Configuration")]
    [Tooltip("The Renderer component on this GameObject (or a child) to change the material color.")]
    public Renderer targetRenderer;

    [Tooltip("The color to apply when the socket order is incorrect.")]
    public Color incorrectColor = Color.red;

    private Material runtimeMaterial;
    private Color originalColor;
    private const string ColorProperty = "_Color"; // Common shader property name for color

    private void Start()
    {
        // Auto-find Renderer if not assigned
        if (targetRenderer == null)
        {
            targetRenderer = GetComponent<Renderer>();
        }

        if (targetRenderer != null)
        {
            // Create a runtime instance of the material to avoid modifying the asset file
            runtimeMaterial = targetRenderer.material;

            // Store the original color property if the material supports it
            if (runtimeMaterial.HasProperty(ColorProperty))
            {
                originalColor = runtimeMaterial.color;
            }
            else
            {
                Debug.LogWarning("SocketVisualFeedback: Material does not support the '" + ColorProperty + "' property. Color tinting will not work.");
            }
        }
        else
        {
            Debug.LogWarning("SocketVisualFeedback: No Renderer component found or assigned on object " + gameObject.name);
        }
    }

    /// <summary>
    /// Tints the object to the incorrect color.
    /// </summary>
    [ContextMenu("Apply Incorrect Color")]
    public void SetIncorrectColor()
    {
        if (runtimeMaterial != null && runtimeMaterial.HasProperty(ColorProperty))
        {
            runtimeMaterial.color = incorrectColor;
        }
    }

    /// <summary>
    /// Restores the object to its original color.
    /// </summary>
    [ContextMenu("Restore Original Color")]
    public void RestoreOriginalColor()
    {
        if (runtimeMaterial != null && runtimeMaterial.HasProperty(ColorProperty))
        {
            runtimeMaterial.color = originalColor;
        }
    }

    private void OnDestroy()
    {
        // Clean up the instantiated material when the script is destroyed
        if (runtimeMaterial != null)
        {
            Destroy(runtimeMaterial);
        }
    }
}