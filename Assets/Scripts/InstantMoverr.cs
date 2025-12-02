using UnityEngine;

public class InstantMoverr : MonoBehaviour
{
    [Header("Target Position")]
    public Vector3 targetPositionn;

    // Call this from another script
    public void TeleportToTargett()
    {
        transform.position = targetPositionn;
    }

    // Optional: A helper method if you ever want to set a target dynamically
    public void TeleporttTo(Vector3 newPosition)
    {
        transform.position = newPosition;
    }
}