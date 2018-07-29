using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameplayController : NetworkBehaviour
{
    public LevelConfigurator level;

    Table table;
    Ball ball;
    Player firstPlayer;
    Player secondPlayer;

    int firstPlayerPoints;
    int secondPlayerPoints;

    PointsCounter pointsCounter;

    public void SetFirstPlayer(Player first)
    {
        if (!isServer)
            return;

        this.firstPlayer = first;
    }

    public void SetSecondPlayer(Player second)
    {
        if (!isServer)
            return;

        this.secondPlayer = second;
    }

    public void SetTable(Table table)
    {
        if (!isServer)
            return;

        this.table = table;
        table.SetGameplay(this);
    }

    public void WaitForPlayers()
    {
        if (!isServer)
            return;

        level.DestroyBall();

        if (firstPlayer != null)
        {
            firstPlayer.RpcSendMessage(new UIMessage(UIMessageType.Notification, "Waiting for second player", 1000f));
        }
    }
    public void PlayersReady()
    {
        if (!isServer)
            return;

        pointsCounter = new PointsCounter(firstPlayer, secondPlayer);

        firstPlayer.RpcSendMessage(new UIMessage(UIMessageType.Notification, "Let's get ready to rumble!!", 2f));
        secondPlayer.RpcSendMessage(new UIMessage(UIMessageType.Notification, "Let's get ready to rumble!!", 2f));

        firstPlayer.RpcSendMessage(new UIMessage(UIMessageType.Points, "0 : 0", 2f));
        secondPlayer.RpcSendMessage(new UIMessage(UIMessageType.Points, "0 : 0", 2f));

        ball = level.GetNextBall();

        LaunchBallInRandomDirection();
    }

    Vector2 GetStartBallDirection()
    {
        for (;;)
        {
            Vector2 direction = UnityEngine.Random.insideUnitCircle.normalized;
            Vector2 horizontalLine = new Vector3(1f, 0f);//, 0f);
            float angle = Vector2.Angle(direction, horizontalLine);
            if (angle > 20f && angle < 160f)
            {
                return direction;
            }
        }
    }

    public void BallFlewAway(int playerWin)
    {
        if (!isServer)
        {
            return;
        }

        pointsCounter.PlayerScored(playerWin);
        ball = level.GetNextBall();
        
        LaunchBallInRandomDirection();
    }

    void LaunchBallInRandomDirection()
    {
        Vector2 direction = GetStartBallDirection();
        ball.MoveDirection = direction;
        ball.Moving = true;
    }

    public void PlayerDisconnected()
    {
        if (!isServer)
        {
            return;
        }

        level.DestroyBall();
        if (firstPlayer != null)
        {
            firstPlayer.RpcSendMessage(new UIMessage(UIMessageType.Notification, "Your opponent has fled away! Ha-ha!", 5f));
        }
    }
}
