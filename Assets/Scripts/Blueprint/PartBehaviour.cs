using UnityEngine;

public class PartDragging : MonoBehaviour
{
    private bool isDragging = false;
    private bool isLocked = false; // Flag to track if the part is locked
    private Vector3 offset;
    private Transform lockTarget = null;
    private SpriteRenderer spriteRenderer;
    private string originalSortingLayer;

    void Start()
    {
        // Cache the SpriteRenderer component
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            originalSortingLayer = spriteRenderer.sortingLayerName; // Store the original sorting layer
        }
    }

    void Update()
    {
        
    }

    void OnMouseDown()
    {
        // Prevent dragging if the part is locked
        if (isLocked) return;

        // Enable dragging and calculate the offset from the initial starting point
        isDragging = true;
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        offset = transform.position - new Vector3(mousePosition.x, mousePosition.y, transform.position.z);

        // Change the sorting layer of the dragged sprite to "Dragged"
        if (spriteRenderer != null)
        {
            spriteRenderer.sortingLayerName = "Dragged";
        }
    }

    void OnMouseDrag()
    {
        // Prevent dragging if the part is locked
        if (isLocked) return;

        // Continuously update the position of the object while dragging
        if (isDragging)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(mousePosition.x, mousePosition.y, transform.position.z) + offset;
        }
    }

    void OnMouseUp()
    {
        // Prevent further execution if the part is locked
        if (isLocked) return;

        isDragging = false;

        // Restore the original sorting layer
        if (spriteRenderer != null)
        {
            spriteRenderer.sortingLayerName = originalSortingLayer;
        }

        // If the object is above the highlighted template, lock into position
        if (lockTarget != null)
        {
            transform.position = lockTarget.position;

            // Lock the part to prevent further dragging
            isLocked = true;

            // Change the sorting layer to "Locked"
            if (spriteRenderer != null)
            {
                spriteRenderer.sortingLayerName = "Locked";
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object is above the highlighted template
        if (other.CompareTag(gameObject.tag))
        {
            lockTarget = other.transform;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // Prevent the object from being dragged out of the highlighted template
        if (other.CompareTag(gameObject.tag) && !isLocked)
        {
            lockTarget = null;
        }
    }
}
