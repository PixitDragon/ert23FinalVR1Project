using System.Collections;
using UnityEngine;

/// <summary>
/// Controls the rotation of a door object around an explicit hinge point.
/// </summary>
public class DoorRotator : MonoBehaviour
{
    [Header("Setup")]
    public Transform hingePivot;

    [Header("Collider Control")]
    [Tooltip("Drag the collider you want to disable/enable during door animation.")]
    public Collider targetCollider;   // <-- NEW

    [Header("Rotation Settings")]
    public float rotationAngle = 90f;
    public Vector3 rotationAxis = Vector3.up;
    public float openDuration = 1.0f;

    private bool isOpen = false;
    private bool isAnimating = false;

    private Quaternion startRotation;
    private Quaternion openRotation;

    void Start()
    {
        if (hingePivot == null)
        {
            Debug.LogError("DoorRotator requires a Hinge Pivot assigned.", this);
            enabled = false;
            return;
        }

        startRotation = transform.localRotation;
        openRotation = startRotation * Quaternion.Euler(rotationAxis.normalized * rotationAngle);

        // Optional: warn if no collider assigned
        if (targetCollider == null)
            Debug.LogWarning("No targetCollider assigned. Collider will not be disabled.");
    }

    public void OpenDoor(bool open)
    {
        if (isAnimating || isOpen == open)
            return;

        isOpen = open;
        StartCoroutine(RotateDoor(isOpen));
    }

    private IEnumerator RotateDoor(bool open)
    {
        isAnimating = true;

        // Disable chosen collider at start of animation
        if (targetCollider != null)
            targetCollider.enabled = false;

        Quaternion startRot = transform.localRotation;
        Quaternion endRot = open ? openRotation : startRotation;

        float timeElapsed = 0f;

        while (timeElapsed < openDuration)
        {
            float t = timeElapsed / openDuration;

            Quaternion targetFrameRot = Quaternion.Lerp(startRot, endRot, t);
            Quaternion deltaRotation = targetFrameRot * Quaternion.Inverse(transform.localRotation);

            Vector3 deltaAngles = deltaRotation.eulerAngles;
            float angleToRotate = Vector3.Dot(deltaAngles, rotationAxis.normalized);

            transform.RotateAround(hingePivot.position,
                                    transform.TransformDirection(rotationAxis),
                                    angleToRotate);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        transform.localRotation = endRot;
        isAnimating = false;

        // Re-enable collider *only when closing* (door is fully closed)
        if (!isOpen && targetCollider != null)
            targetCollider.enabled = true;
    }
}