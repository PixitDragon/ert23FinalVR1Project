using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;
using System.Collections.Generic;

/// <summary>
/// Monitors a sequence of XRSocketInteractors and triggers an event
/// if the inserted objects match a required tag order.
/// </summary>
public class SocketOrderChecker : MonoBehaviour
{
    [Header("Configuration")]
    [Tooltip("The ordered list of XRSocketInteractors to check (e.g., NODE0, NODE1, NODE2).")]
    public List<UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor> sockets = new List<UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor>();

    [Tooltip("The REQUIRED TAG order for the objects in the sockets. This list MUST match the size of the Sockets list.")]
    public List<string> requiredTags = new List<string>();

    [Header("Events")]
    [Tooltip("Triggered when ALL sockets are filled with the correct objects in the correct order.")]
    public UnityEvent onOrderCorrect = new UnityEvent();

    [Tooltip("Triggered when an object is placed or removed, and the current configuration is not correct.")]
    public UnityEvent onOrderIncorrect = new UnityEvent();

    private void Start()
    {
        // Perform initial configuration checks and subscribe to socket events.
        SetupSocketListeners();
    }

    private void SetupSocketListeners()
    {
        if (sockets.Count != requiredTags.Count)
        {
            Debug.LogError("SocketOrderChecker: Configuration mismatch! The number of assigned Sockets MUST match the number of Required Tags on object: " + gameObject.name);
            return;
        }

        // Subscribe to both SelectEntered and SelectExited events for each socket
        // so the order is checked whenever an object is placed OR removed.
        foreach (var socket in sockets)
        {
            if (socket != null)
            {
                socket.selectEntered.AddListener(OnSocketInteraction);
                socket.selectExited.AddListener(OnSocketInteraction);
            }
        }
    }

    // Universal listener function for socket select events
    private void OnSocketInteraction(SelectEnterEventArgs args) => CheckOrder();
    private void OnSocketInteraction(SelectExitEventArgs args) => CheckOrder();


    /// <summary>
    /// Iterates through all assigned sockets and checks if the objects placed
    /// within them match the required tag sequence.
    /// </summary>
    [ContextMenu("Manually Check Order")]
    public bool CheckOrder()
    {
        if (sockets.Count != requiredTags.Count)
        {
            return false; // Should be handled in Setup, but as a safeguard
        }

        bool isCorrect = true;

        for (int i = 0; i < sockets.Count; i++)
        {
            UnityEngine.XR.Interaction.Toolkit.Interactors.XRSocketInteractor currentSocket = sockets[i];
            string requiredTag = requiredTags[i];

            // 1. Check if the socket has *any* object selected using the non-obsolete 'hasSelection'
            if (!currentSocket.hasSelection)
            {
                // If a socket is empty, the sequence is incomplete/incorrect
                isCorrect = false;
                break;
            }

            // 2. Get the placed object by accessing the first (and only) item in the selections list.
            // The 'hasSelection' check ensures this list access is safe.
            UnityEngine.XR.Interaction.Toolkit.Interactables.IXRSelectInteractable interactable = currentSocket.interactablesSelected[0];

            // Safety check for null (though rare here, it's good practice)
            if (interactable == null)
            {
                isCorrect = false;
                break;
            }

            GameObject placedObject = interactable.transform.gameObject;

            if (!placedObject.CompareTag(requiredTag))
            {
                // Found a mismatch in the required tag
                isCorrect = false;
                break;
            }
        }

        // 3. Trigger the appropriate event
        if (isCorrect)
        {
            Debug.Log("Socket Sequence Correct! Activating 'onOrderCorrect' event.");
            onOrderCorrect.Invoke();
            ///ActivateDoorOpen();
        }
        else
        {
            Debug.Log("Socket Sequence Incorrect!");
            onOrderIncorrect.Invoke();
        }

        return isCorrect;
    }

    private void OnDestroy()
    {
        // Clean up listeners when the script is destroyed
        foreach (var socket in sockets)
        {
            if (socket != null)
            {
                socket.selectEntered.RemoveListener(OnSocketInteraction);
                socket.selectExited.RemoveListener(OnSocketInteraction);
            }
        }
    }
}