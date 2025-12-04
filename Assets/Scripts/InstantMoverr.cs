using UnityEngine;

/// <summary>
/// Moves the GameObject to a target position and plays a sound.
/// </summary>
[RequireComponent(typeof(AudioSource))] // 1. Guarantees the AudioSource is present
public class InstantMover : MonoBehaviour
{
    [Header("Target Position")]
    public Vector3 targetPosition;

    [Header("Audio Settings")]
    [Tooltip("The sound effect to play when the object teleports.")]
    public AudioClip teleportSound;

    private AudioSource audioSource; // Private reference to the AudioSource component

    void Awake()
    {
        // 2. Get the AudioSource component immediately on startup
        audioSource = GetComponent<AudioSource>();
    }

    // Call this from another script
    public void TeleportToTarget()
    {
        // 1. Play the sound effect
        if (teleportSound != null && audioSource != null)
        {
            // PlayOneShot is ideal for simple, one-off sound effects like a teleport.
            audioSource.PlayOneShot(teleportSound);
        }
        else if (teleportSound == null)
        {
            Debug.LogWarning("Teleport Sound Clip is not assigned on the InstantMover script!");
        }

        // 2. Existing teleport logic
        transform.position = targetPosition;
    }

    // Optional: A helper method if you ever want to set a target dynamically
    public void TeleportTo(Vector3 newPosition)
    {
        // Play sound when this function is called too
        if (teleportSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(teleportSound);
        }

        transform.position = newPosition;
    }
}