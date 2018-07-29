using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

[RequireComponent(typeof(Bat))]
public class UserInputHandler : NetworkBehaviour
{
    Bat bat;

    //float distanceToConsiderStaying = 0.05f;
    //float timeToConsiderBeginningAgain = 1f;
    //float timerToBeginAgain = 0f;
    //bool inDrag;
    //Vector2 startDragPosition;
    Vector2 previousDragPosition;

    [ClientRpc]
    public void RpcInitialize()
    {
        if (!isLocalPlayer)
            return;

        bat = GetComponent<Bat>();
        UserInputPanel.OnBeginDrag += OnBeginDrag;
        UserInputPanel.OnDrag += OnDrag;
        UserInputPanel.OnEndDrag += OnEndDrag;
    }

    public void Initialize()
    {
        if (!isLocalPlayer)
            return;

        bat = GetComponent<Bat>();
        UserInputPanel.OnBeginDrag += OnBeginDrag;
        UserInputPanel.OnDrag += OnDrag;
        UserInputPanel.OnEndDrag += OnEndDrag;
    }

    void OnBeginDrag(PointerEventData eventData)
    {
        //inDrag = true;
        //startDragPosition = eventData.position;
        previousDragPosition = eventData.position;

        bat.StartNewMovement();
    }

    void OnEndDrag(PointerEventData eventData)
    {
        //inDrag = false;
        CalculateAndMove(eventData);
    }

    void OnDrag(PointerEventData eventData)
    {
        //float xChangeSinceDragStart = Mathf.Abs(eventData.position.x - startDragPosition.x) / Screen.width;
        //if (xChangeSinceDragStart > distanceToConsiderStaying)
        //{
        //    startDragPosition = eventData.position;
        //    timerToBeginAgain = 0f;
        //}
        //else
        //{
        //    timerToBeginAgain += Time.deltaTime;
        //    if (timerToBeginAgain > timeToConsiderBeginningAgain)
        //    {

        //    }
        //}

        CalculateAndMove(eventData);
    }

    void CalculateAndMove(PointerEventData eventData)
    {
        float xChangeSincePrevious = eventData.position.x - previousDragPosition.x;
        float xChangeInWorldUnits = AppearanceHelper.ToWorldUnits(xChangeSincePrevious);

        bat.MoveRelative(xChangeInWorldUnits);

        previousDragPosition = eventData.position;
    }



    void OnDestroy()
    {
        if (!isLocalPlayer)
            return;

        UserInputPanel.OnBeginDrag -= OnBeginDrag;
        UserInputPanel.OnDrag -= OnDrag;
        UserInputPanel.OnEndDrag -= OnEndDrag;
    }
}
