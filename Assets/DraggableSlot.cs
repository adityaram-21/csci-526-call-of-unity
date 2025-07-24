using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public int slotIndex;
    public LetterRack rack;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Canvas canvas;
    private Vector3 originalPosition;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = gameObject.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        canvas = GetComponentInParent<Canvas>();
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        originalPosition = rectTransform.position;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (canvas != null)
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        // Use raycast to check what we dropped on
        if (eventData.pointerEnter != null)
        {
            var other = eventData.pointerEnter.GetComponentInParent<DraggableSlot>();
            if (other != null && other != this)
            {
                rack.Swap(this.slotIndex, other.slotIndex);
            }
        }

        // Return to original position
        rectTransform.position = originalPosition;
    }
}
