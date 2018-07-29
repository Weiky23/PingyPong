using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Table : NetworkBehaviour, ICollisionHandler
{
    // информация о положении игроков и кружка на старте
    public Transform firstPlayerPosition;
    public Transform firstBoundLeft;
    public Transform firstBoundRight;

    public Transform secondPlayerPosition;
    public Transform secondBoundLeft;
    public Transform secondBoundRight;

    public Transform ballPosition;

    GameplayController gameplay;

    void OnEnable()
    {
        // части стола, которые шлют сообщения о столкновениях сюда
        ICollisionPart[] tableParts = GetComponentsInChildren<ICollisionPart>();
        for (int i = 0; i < tableParts.Length; i++)
        {
            tableParts[i].SetCollisionHandler(this);
        }
    }

    public void SetGameplay(GameplayController gameplay)
    {
        this.gameplay = gameplay;
    }

    public void BallFlewAway(int player)
    {
        if (!isServer)
        {
            return;
        }
        gameplay.BallFlewAway(player);
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
        // просто рикошетим
        Vector3 ballDirection = ball.MoveDirection;

        Vector3 oppositeDirection = new Vector3(-ballDirection.x, -ballDirection.y, 0f);

        float signedAngle = Vector3.SignedAngle(oppositeDirection, part.Normal, new Vector3(0f, 0f, 1f));
        Quaternion rotateQuaternion = Quaternion.Euler(0f, 0f, signedAngle * 2f);
        Vector3 reboundDirection = rotateQuaternion * oppositeDirection;

        ball.MoveDirection = reboundDirection;

        Edge wall = part as Edge;
        if (wall != null)
        {
            wall.ForbidColliding(0.2f);
        }
    }
}
