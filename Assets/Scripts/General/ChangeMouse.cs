using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChangeMouse : MonoBehaviour , IPointerEnterHandler , IPointerExitHandler
{
    [SerializeField] private Texture2D clickableCursor;

    [SerializeField] private Vector2 hotSpot = Vector2.zero;


    public void OnPointerEnter(PointerEventData eventData)
    {
        if (clickableCursor != null)
        {
            Cursor.SetCursor(clickableCursor, hotSpot, CursorMode.Auto);
        }
    }


    public void OnPointerExit(PointerEventData eventData)
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    private void OnDisable()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}
