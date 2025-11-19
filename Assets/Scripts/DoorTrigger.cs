using UnityEngine;

/// <summary>
/// Simple script to trigger the door's rotation.
/// This script can now control multiple doors simultaneously.
/// </summary>
public class DoorTrigger : MonoBehaviour
{
    [Tooltip("Drag all GameObjects with the DoorRotator script onto this list.")]
    public DoorRotator[] targetDoors; // Changed to an array to hold multiple doors

    /// <summary>
    /// Call this method when the external condition (e.g., puzzle complete) is met.
    /// This now opens all doors in the targetDoors array simultaneously.
    /// </summary>
    public void ActivateDoorOpen()
    {
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
        ActivateDoorOpen();
    }
}