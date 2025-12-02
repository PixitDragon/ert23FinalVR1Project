using UnityEngine;

public class InstantMover : MonoBehaviour
{
    [Header("Target Position")]
    public Vector3 targetPosition;

    // Call this from another script
    public void TeleportToTarget()
    {
        transform.position = targetPosition;
    }

    // Optional: A helper method if you ever want to set a target dynamically
    public void TeleportTo(Vector3 newPosition)
    {
        transform.position = newPosition;
    }
}