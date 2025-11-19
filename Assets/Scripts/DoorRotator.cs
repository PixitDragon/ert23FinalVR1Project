using System.Collections;
using UnityEngine;

/// <summary>
/// Controls the rotation of a door object around an explicit hinge point.
/// To use this script correctly with a child pivot:
/// 1. Attach this script directly to your main door Mesh/Model GameObject.
/// 2. Create an empty GameObject (e.g., "HingePoint") as a CHILD of the door Mesh/Model.
/// 3. Position the "HingePoint" exactly where you want the door to rotate from.
/// 4. Drag the "HingePoint" child Transform into the 'Hinge Pivot' field in the Inspector.
/// 5. Call the public OpenDoor() method from another script when your external task is completed.
/// </summary>
public class DoorRotator : MonoBehaviour
{
    [Header("Setup")]
    [Tooltip("The Transform representing the hinge point. Must be a child of this door object.")]
    public Transform hingePivot;

    [Header("Rotation Settings")]
    [Tooltip("The total angle (in degrees) the door will rotate when opened.")]
    public float rotationAngle = 90f;

    [Tooltip("The local axis around which the door rotates (e.g., Vector3.up or Vector3.forward).")]
    public Vector3 rotationAxis = Vector3.up;

    [Tooltip("The time (in seconds) it takes for the door to fully open or close.")]
    public float openDuration = 1.0f;

    // Internal state tracking
    private bool isOpen = false;
    private bool isAnimating = false;

    // Store the initial rotation of the door mesh
    private Quaternion startRotation;

    // Store the calculated target rotation
    private Quaternion openRotation;

    void Start()
    {
        if (hingePivot == null)
        {
            Debug.LogError("DoorRotator requires a Hinge Pivot to be assigned in the Inspector.", this);
            enabled = false;
            return;
        }

        // Set the initial rotation
        startRotation = transform.localRotation;

        // Calculate the target rotation based on the initial state and the desired angle
        openRotation = startRotation * Quaternion.Euler(rotationAxis.normalized * rotationAngle);
    }

    /// <summary>
    /// Public method to initiate the door opening or closing.
    /// </summary>
    /// <param name="open">True to open the door, False to close it.</param>
    public void OpenDoor(bool open)
    {
        if (isAnimating || isOpen == open)
        {
            // Ignore new requests if animating or already in the desired state
            return;
        }

        isOpen = open;

        // Start the rotation coroutine
        StartCoroutine(RotateDoor(isOpen));
    }

    /// <summary>
    /// Coroutine to handle smooth, time-based rotation of the door around the hingePivot.
    /// </summary>
    /// <param name="open">True to open, False to close.</param>
    private IEnumerator RotateDoor(bool open)
    {
        isAnimating = true;

        // Determine the start and end rotation for the current action
        Quaternion startRot = transform.localRotation;
        Quaternion endRot = open ? openRotation : startRotation;

        float timeElapsed = 0f;

        while (timeElapsed < openDuration)
        {
            // Calculate the fraction of the total duration completed
            float t = timeElapsed / openDuration;

            // Use Lerp to find the target rotation for this specific frame
            Quaternion targetFrameRot = Quaternion.Lerp(startRot, endRot, t);

            // Calculate the rotation difference needed this frame
            Quaternion deltaRotation = targetFrameRot * Quaternion.Inverse(transform.localRotation);

            // Convert delta Quaternion to Euler angles to get the axis-angle components
            Vector3 deltaAngles = deltaRotation.eulerAngles;

            // Normalize angles between -180 and 180 for accurate rotation application
            // Only consider the rotation around the main axis (e.g., Y-axis for Vector3.up)
            float angleToRotate = Vector3.Dot(deltaAngles, rotationAxis.normalized);

            // This is the key: Rotate the door transform around the world position of the hinge pivot.
            transform.RotateAround(hingePivot.position, transform.TransformDirection(rotationAxis), angleToRotate);

            timeElapsed += Time.deltaTime;
            yield return null; // Wait until the next frame
        }

        // Ensure the rotation is precisely at the target when finished
        transform.localRotation = endRot;
        isAnimating = false;
    }
}