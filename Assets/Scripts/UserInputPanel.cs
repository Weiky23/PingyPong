using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UserInputPanel : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public static event Action<PointerEventData> OnBeginDrag;
    public static event Action<PointerEventData> OnDrag;
    public static event Action<PointerEventData> OnEndDrag; 
    public static event Action<PointerEventData> OnPointerClick;

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
        if (OnBeginDrag != null)
            OnBeginDrag(eventData);
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {
        if (OnDrag != null)
            OnDrag(eventData);
    }

    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
        if (OnEndDrag != null)
            OnEndDrag(eventData);
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        if (OnPointerClick != null)
            OnPointerClick(eventData);
    }
}