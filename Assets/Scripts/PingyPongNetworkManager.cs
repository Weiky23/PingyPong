using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

// менеджер сети на хосте. довольно криво, по хорошему надо дольше разбираться и делать без NetworkManager
public class PingyPongNetworkManager : NetworkManager
{
    public LevelConfigurator gameplayScene;
    public GameplayController gameplay;
    public UI ui;
    int playersInGame = 0;

    void Awake()
    {
        GameStates.OnStateExit += OnStateExit;
        GameStates.OnStateEnter += OnStateEnter;

        ui = FindObjectOfType<UI>();
    }

    void OnStateExit(GameState exitState)
    {
        switch (exitState)
        {
            case GameState.OnePlayer:
                StopHost();
                break;
            case GameState.MultiplayerHost:
                StopHost();
                break;
            case GameState.MultiplayerClient:
                StopClient();
                break;
            default:
                break;
        }
    }

    void OnStateEnter(GameState enterState)
    {
        switch (enterState)
        {
            case GameState.OnePlayer:
                StartHost();
                break;
            case GameState.MultiplayerHost:
                StartMatchMaker();
                matchMaker.CreateMatch("PingyPong", 4, true, "", "", "", 0, 0, OnMatchCreate);
                //StartHost();
                break;
            case GameState.MultiplayerClient:
                StartMatchMaker();
                matchMaker.ListMatches(0, 1, "", true, 0, 0, OnMatchList);
                //StartClient();
                break;
            default:
                break;
        }
    }

    public override void OnMatchCreate(bool success, string extendedInfo, MatchInfo matchInfo)
    {
        if (success)
        {
            StartHost(matchInfo);
        }
    }

    public override void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matchList)
    {
        if (success)
        {
            if (matchList != null && matchList.Count > 0)
            {
                matchMaker.JoinMatch(matchList[0].networkId, "", "", "", 0, 0, OnMatchJoined);
            }
            else
            {             
                GameStates.SetState(GameState.Menu);
                ui.ShowMessage(new UIMessage(UIMessageType.Notification, "Nobody plays this wonderful game :(", 4f));
            }
        }
    }

    public override void OnMatchJoined(bool success, string extendedInfo, MatchInfo matchInfo)
    {
        if (success)
        {
            StartClient(matchInfo);
        }
    }

    public override void OnStartHost()
    {
        base.OnStartHost();
        playersInGame = 0;
    }

    public override void OnServerConnect(NetworkConnection conn)
    {
        base.OnServerConnect(conn);
        if (playersInGame == 0)
        {
            // мы хост и присоединилсь к себе, готовим уровень
            gameplayScene.PrepareLevel();
            gameplay.SetTable(gameplayScene.Table);
        }
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        if (GameStates.Current == GameState.OnePlayer)
        {
            // сингл – создаем двух игроков для одного и того же соединения
            Table table = gameplayScene.Table;
            GameObject playerOne = (GameObject)Instantiate(playerPrefab, table.firstPlayerPosition.position, Quaternion.identity);
            NetworkServer.AddPlayerForConnection(conn, playerOne, 0);
            //playerOne.GetComponent<UserInputHandler>().RpcInitialize();
            Player bat = playerOne.GetComponent<Player>();
            bat.RpcInitialize(0, table.firstPlayerPosition.position, table.firstBoundLeft.position, table.firstBoundRight.position);//, new Vector3(0, -1, 0));
            gameplay.SetFirstPlayer(bat);

            GameObject anotherPlayerOne = (GameObject)Instantiate(playerPrefab, table.secondPlayerPosition.position, Quaternion.identity);
            NetworkServer.AddPlayerForConnection(conn, anotherPlayerOne, 1);
            //anotherPlayerOne.GetComponent<UserInputHandler>().RpcInitialize();
            bat = anotherPlayerOne.GetComponent<Player>();
            bat.RpcInitialize(1, table.secondPlayerPosition.position, table.secondBoundLeft.position, table.secondBoundRight.position);//, new Vector3(0, 1, 0));
            gameplay.SetSecondPlayer(bat);

            gameplay.PlayersReady();
        }
        else if (GameStates.Current == GameState.MultiplayerHost)
        {
            Table table = gameplayScene.Table;
            if (playersInGame == 0)
            {
                // мы хост и сами приконнектились первые
                GameObject playerOne = (GameObject)Instantiate(playerPrefab, table.firstPlayerPosition.position, Quaternion.identity);
                NetworkServer.AddPlayerForConnection(conn, playerOne, 0);
                //playerOne.GetComponent<UserInputHandler>().RpcInitialize();
                Player bat = playerOne.GetComponent<Player>();
                bat.RpcInitialize(0, table.firstPlayerPosition.position, table.firstBoundLeft.position, table.firstBoundRight.position);//, new Vector3(0, -1, 0));
                gameplay.SetFirstPlayer(bat);
                playersInGame = 1;

                gameplay.WaitForPlayers();
            }
            else if (playersInGame == 1)
            {
                // мы хост и приконнектился клиент
                GameObject anotherPlayerOne = (GameObject)Instantiate(playerPrefab, table.secondPlayerPosition.position, Quaternion.identity);
                NetworkServer.AddPlayerForConnection(conn, anotherPlayerOne, 1);
                //anotherPlayerOne.GetComponent<UserInputHandler>().RpcInitialize();
                Player bat = anotherPlayerOne.GetComponent<Player>();
                bat.RpcInitialize(1, table.secondPlayerPosition.position, table.secondBoundLeft.position, table.secondBoundRight.position);//, new Vector3(0, 1, 0));
                gameplay.SetSecondPlayer(bat);
                playersInGame = 2;

                gameplay.PlayersReady();
            }
        }
    }


    // чистим левел
    public override void OnStopHost()
    {
        base.OnStopHost();
        gameplayScene.ClearLevel();
    }

    // клиент дисконнектнулся, если переконнектится, заново будет играть
    public override void OnServerDisconnect(NetworkConnection conn)
    {
        base.OnServerDisconnect(conn);
        playersInGame--;
        gameplay.PlayerDisconnected();
    }

    void OnDestroy()
    {
        GameStates.OnStateExit -= OnStateExit;
        GameStates.OnStateEnter -= OnStateEnter;
    }
}