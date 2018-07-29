using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;

[RequireComponent(typeof(BoxCollider2D))]
public class Bat : NetworkBehaviour, ICollisionHandler
{
    public float maxSpeed = 2f;
    float currentSpeed = 0f;

    // где в начале расположена доска
    Vector3 startPosition;

    // фактические точки крайних положений
    Vector3 leftBound;
    Vector3 rightBound;

    // когда начинаем свайпать, пляшем от этой точки
    Vector3 startMovementPosition;

    // куда едем
    Vector3 targetPosition;

    float timeToForbidColliding = 1f;

    Edge[] edges;
    Player player;

    void OnEnable()
    {
        edges = GetComponentsInChildren<Edge>();
        player = GetComponent<Player>();
        for (int i = 0; i < edges.Length; i++)
        {
            edges[i].SetCollisionHandler(this);
        }
    }

    //[ClientRpc]
    //public void RpcSetPositionAndBounds(Vector3 position, Vector3 levelLeftBound, Vector3 levelRightBound)
    //{
    //    if (!isLocalPlayer)
    //        return;

    //    this.startPosition = position;
    //    transform.position = position;
    //    BoxCollider2D box = GetComponent<BoxCollider2D>();
    //    this.leftBound = levelLeftBound + new Vector3(box.size.x * 0.5f, 0f, 0f);
    //    this.rightBound = levelRightBound - new Vector3(box.size.x * 0.5f, 0f, 0f);

    //    currentSpeed = maxSpeed;
    //}

    public void Set(Vector3 position, Vector3 levelLeftBound, Vector3 levelRightBound)
    {
        if (!isLocalPlayer)
            return;

        this.startPosition = position;
        transform.position = position;
        BoxCollider2D box = GetComponent<BoxCollider2D>();
        this.leftBound = levelLeftBound + new Vector3(box.size.x * 0.5f, 0f, 0f);
        this.rightBound = levelRightBound - new Vector3(box.size.x * 0.5f, 0f, 0f);

        currentSpeed = maxSpeed;
    }

    void Update()
    {
        if (!isLocalPlayer)
            return;

        if (targetPosition == Vector3.zero)
            return;

        Vector3 previousPosition = transform.position;

        float totalDistance = Mathf.Abs(targetPosition.x - transform.position.x);
        if (totalDistance > 0)
        {
            float distanceToCover = Time.deltaTime * maxSpeed;
            float interpolated = Mathf.Lerp(transform.position.x, targetPosition.x, distanceToCover / totalDistance);
            transform.position = new Vector3(interpolated, transform.position.y, transform.position.z);
        }

        currentSpeed = (transform.position.x - previousPosition.x) / Time.deltaTime;
        CmdSetCurrentSpeed(currentSpeed);
    }

    [Command]
    void CmdSetCurrentSpeed(float s)
    {
        currentSpeed = s;
    }

    public void StartNewMovement()
    {
        startMovementPosition = transform.position;
        targetPosition = transform.position;
    }

    public void MoveRelative(float xChangeInWorldUnits)
    {
        targetPosition.x += xChangeInWorldUnits;
        targetPosition.x = Mathf.Clamp(targetPosition.x, leftBound.x, rightBound.x);
    }

    public void HandleCollision(GameObject collider, ICollisionPart part)
    {
        if (!isServer)
        {
            return;
        }

        Ball ball = collider.GetComponent<Ball>();
        if (ball == null)
        {
            Debug.LogError("Нет компонента Ball");
            return;
        }

        Vector3 ballDirection = ball.MoveDirection;

        Vector3 oppositeDirection = new Vector3(-ballDirection.x, -ballDirection.y, 0f);

        float signedAngle = Vector3.SignedAngle(oppositeDirection, part.Normal, new Vector3(0f, 0f, 1f));
        Quaternion rotateQuaternion = Quaternion.Euler(0f, 0f, signedAngle * 2f);
        Vector3 reboundDirection = rotateQuaternion * oppositeDirection;

        if (currentSpeed > 0.01f || currentSpeed < -0.01f)
        {
            Debug.Log(player.PlayerIndex);
            Quaternion speedQuaternion = Quaternion.Euler(0f, 0f, player.PlayerIndex == 0 ? currentSpeed * 4f : -currentSpeed * 4f);
            reboundDirection = speedQuaternion * reboundDirection;
        }

        ball.MoveDirection = reboundDirection;


        for (int i = 0; i < edges.Length; i++)
        {
            edges[i].ForbidColliding(0.2f);
        }
    }
}
