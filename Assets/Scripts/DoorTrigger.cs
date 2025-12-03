using UnityEngine;

/// <summary>
/// Simple script to trigger the door's rotation.
/// This script can now control multiple doors simultaneously.
/// </summary>
[RequireComponent(typeof(AudioSource))] // Ensures the AudioSource component is present
public class DoorTrigger : MonoBehaviour
{
    [Tooltip("Drag all GameObjects with the DoorRotator script onto this list.")]
    public DoorRotator[] targetDoors; // Changed to an array to hold multiple doors

    [Header("Sound Settings")]
    [Tooltip("The sound effect to play when the doors open.")]
    public AudioClip doorOpenClip;

    private AudioSource audioSource; // Private reference to the AudioSource component

    void Awake()
    {
        // Get the AudioSource component on startup
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Call this method when the external condition (e.g., puzzle complete) is met.
    /// This now opens all doors in the targetDoors array simultaneously.
    /// </summary>
    public void ActivateDoorOpen()
    {
        // 1. Play the sound effect
        if (doorOpenClip != null && audioSource != null)
        {
            // Play the assigned clip once
            audioSource.PlayOneShot(doorOpenClip);
        }
        else if (doorOpenClip == null)
        {
            Debug.LogWarning("Door Open Clip is not assigned on the DoorTrigger script!");
        }

        // 2. Open the doors
        if (targetDoors != null && targetDoors.Length > 0)
        {
            // Loop through every door in the array
            foreach (DoorRotator door in targetDoors)
            {
                if (door != null)
                {
                    // The 'true' argument tells the DoorRotator to open the door.
                    door.OpenDoor(true);
                }
                else
                {
                    Debug.LogWarning("One of the door slots in targetDoors is null or empty!");
                }
            }

            // Optional: Disable the trigger after the first use
            // this.enabled = false;
        }
        else
        {
            Debug.LogError("Target Doors array is empty or not assigned to the trigger!");
        }
    }

    // --- Example of a simple one-way interaction trigger (using Colliders) ---
    private void OnTriggerEnter(Collider other)
    {
        // This is an example if you want a player walking through a trigger volume
        // to open the door. You'd typically check for the player's tag or component.

        if (other.CompareTag("Player"))
        {
            ActivateDoorOpen();
        }
    }

    void Start()
    {
        ///ActivateDoorOpen();
    }
}