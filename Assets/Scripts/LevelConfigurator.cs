using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

// сидит на сервере и конфигурирует уровень перед началом игры 
public class LevelConfigurator : NetworkBehaviour
{
    public GameObject tablePrefab;
    public BallsOrder ballsOrder;
    int currentBallIndex = -1;

    Table currentTable;
    public Table Table { get { return currentTable; } }
    Ball currentBall;
    public Ball Ball { get { return currentBall; } }

    public void PrepareLevel()
    {
        GameObject tableGameObject = (GameObject)Instantiate(tablePrefab);
        currentTable = tableGameObject.GetComponent<Table>();
        NetworkServer.Spawn(tableGameObject);
    }

    public Ball GetNextBall()
    {
        DestroyBall();

        GameObject ballPrefab = NextBallPrefab();
        if (ballPrefab == null)
            return null;

        GameObject ball = (GameObject)Instantiate(ballPrefab, currentTable.ballPosition.position, Quaternion.identity);
        currentBall = ball.GetComponent<Ball>();
        NetworkServer.Spawn(ball);
        return currentBall;
    }

    public void DestroyBall()
    {
        if (currentBall != null)
        {
            NetworkServer.Destroy(currentBall.gameObject);
        }
    }

    GameObject NextBallPrefab()
    {
        currentBallIndex++;
        GameObject[] balls = ballsOrder.balls;
        if (balls == null || balls.Length == 0)
        {
            Debug.LogError("Нет ни одного мяча в конфиге");
            return null;
        }
        if (balls.Length > currentBallIndex)
        {
            return balls[currentBallIndex];
        }
        currentBallIndex = 0;
        return balls[0];
    }

    public void ClearLevel()
    {
        Debug.Log("Clear scene");
        if (currentTable != null)
        {
            NetworkServer.Destroy(currentTable.gameObject);
        }
        if (currentBall != null)
        {
            NetworkServer.Destroy(currentBall.gameObject);
        }
    }
}
