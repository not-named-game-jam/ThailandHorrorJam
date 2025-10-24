using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System; // Required for Math.Max/Min

public class DragableButton : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform; // Reference to the button's RectTransform
    private RectTransform parentRectTransform; // Reference to the parent's RectTransform (for bounds)
    private Canvas canvas; // Reference to the parent Canvas

    // Stores the offset between the cursor position and the button's pivot when dragging starts.
    private Vector2 dragOffset; 

    // --- NEW SNAPPING FIELDS ---
    [Header("Snapping")]
    [Tooltip("The list of slots (RectTransforms) this button can snap to.")]
    public RectTransform[] snapTargets;
    [Tooltip("The maximum distance (in local space) for snapping to occur.")]
    public float snapDistance = 50f;
    // ---------------------------

    public bool snapped = false;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        // Get the parent's RectTransform
        parentRectTransform = rectTransform.parent.GetComponent<RectTransform>();

        // Find the root Canvas component (crucial for proper UI coordinate conversion)
        canvas = GetComponentInParent<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("DraggableButton requires a Canvas parent to function correctly.");
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(snapped == true) return;

        // 1. Calculate the initial offset. This prevents the button from jumping.
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform, 
            eventData.position, 
            eventData.pressEventCamera, 
            out Vector2 localCursor
        ))
        {
            // localCursor now represents the offset from the pivot.
            dragOffset = localCursor;
        }

        // Optional: If you want the dragged object to appear above others during the drag:
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(snapped == true) return;

        // Variable to hold the calculated position relative to the parent
        Vector2 localPointerPosition;
        
        // Use the pressEventCamera if available, otherwise use worldCamera from Canvas (for overlay mode)
        Camera camera = eventData.pressEventCamera ?? canvas.worldCamera;
        
        // Use the ScreenPointToLocalPointInRectangle method for reliable cursor positioning.
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentRectTransform, // Use the parent's RectTransform for local point conversion
            eventData.position, 
            camera, 
            out localPointerPosition
        ))
        {
            // Calculate the desired position, compensating for the initial drag offset.
            Vector2 desiredPosition = localPointerPosition - dragOffset;

            // --- CLAMPING LOGIC START ---

            // Get the half-size of the button (relative to its pivot).
            Vector2 halfSize = rectTransform.sizeDelta * 0.5f;

            // Determine the maximum bounds of the parent container, minus the button's size.
            // The pivot is usually 0.5, 0.5 (center), so we use halfSize.

            // Get the size of the parent rect.
            Vector2 parentSize = parentRectTransform.sizeDelta;
            
            // Calculate the boundaries (min and max anchored position values).
            // Parent's pivot is assumed to be 0.5, 0.5 (center) in these calculations.
            
            float minX = (-parentSize.x * 0.5f) + halfSize.x;
            float maxX = (parentSize.x * 0.5f) - halfSize.x;
            
            float minY = (-parentSize.y * 0.5f) + halfSize.y;
            float maxY = (parentSize.y * 0.5f) - halfSize.y;

            // Clamp the X and Y positions.
            desiredPosition.x = Math.Max(minX, Math.Min(maxX, desiredPosition.x));
            desiredPosition.y = Math.Max(minY, Math.Min(maxY, desiredPosition.y));

            // --- CLAMPING LOGIC END ---

            // Apply the final, clamped position.
            rectTransform.anchoredPosition = desiredPosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // --- NEW SNAPPING LOGIC START ---
        if (snapTargets != null && snapTargets.Length > 0)
        {
            Vector2 currentPosition = rectTransform.anchoredPosition;
            float closestDistance = float.MaxValue;
            RectTransform closestTarget = null;

            // 1. Find the closest snap target
            foreach (RectTransform target in snapTargets)
            {
                if (target == null) continue; // Skip if a slot is unassigned

                // Calculate the distance between the button's pivot and the slot's pivot
                float distance = Vector2.Distance(currentPosition, target.anchoredPosition);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTarget = target;
                }
            }

            // 2. Snap if the closest target is within the tolerance distance
            if (closestTarget != null && closestDistance <= snapDistance)
            {
                // Snap the button to the slot's position
                rectTransform.anchoredPosition = closestTarget.anchoredPosition;
                Debug.Log($"Snapped {gameObject.name} to {closestTarget.name}.");
                snapped = true;
            }
        }
        // --- NEW SNAPPING LOGIC END ---

        // Reset offset to zero, though it will be recalculated on the next drag start.
        dragOffset = Vector2.zero;
    }
}
