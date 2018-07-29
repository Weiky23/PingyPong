using UnityEngine.EventSystems;

public interface IClickDragHandler
{
    void PointerClick(PointerEventData data);
    void BeginDrag(PointerEventData data);
    void Drag(PointerEventData data);
    void EndDrag(PointerEventData data);
}