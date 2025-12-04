using System.Collections.Generic;
using UnityEngine;

public class ItemSafetyNet : MonoBehaviour
{
    [Header("Safety Parameters")]
    [Tooltip("The Y-level threshold. If an item's Y-position falls below this, it will be reset.")]
    public float safetyYLevel = -5f;

    [Tooltip("The position where the item will be teleported (reset) to.")]
    public Vector3 resetPosition = new Vector3(0f, 1f, 0f);

    [Header("Item List")]
    [Tooltip("Drag and drop the GameObjects you want to monitor here.")]
    public List<GameObject> monitoredItems = new List<GameObject>();

    // Called once per frame
    private void Update()
    {
        // Iterate through all items in the list
        foreach (GameObject item in monitoredItems)
        {
            // Always check if the item is still valid (hasn't been destroyed)
            if (item != null)
            {
                // Check if the item's Y position is below the safety threshold
                if (item.transform.position.y < safetyYLevel)
                {
                    // Call the function to handle the teleport/reset
                    TeleportItem(item);
                }
            }
        }
    }

    /// <summary>
    /// Teleports the fallen item to the defined reset position.
    /// </summary>
    /// <param name="item">The GameObject to be reset.</param>
    private void TeleportItem(GameObject item)
    {
        // Log a message to the Unity console for debugging
        Debug.Log($"Item '{item.name}' fell below Y={safetyYLevel} and is being reset.", item);

        // 1. Reset its position
        item.transform.position = resetPosition;

        // 2. Optional: Reset its rotation to avoid weird physics behavior on landing
        item.transform.rotation = Quaternion.identity;

        // 3. Optional: Stop its movement if it has a Rigidbody (highly recommended for physics objects)
        Rigidbody rb = item.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        // You could add particle effects or sounds here to indicate the reset!
    }
}