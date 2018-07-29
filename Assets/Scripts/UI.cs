using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject gameplayMenu;
    public MessagePanel messagePanel;
    public MessagePanel pointsPanel;

    void Awake()
    {
        GameStates.OnStateExit += OnStateExit;
        GameStates.OnStateEnter += OnStateEnter;
    }

    void OnStateExit(GameState exitState)
    {
        //if (exitState == GameState.Menu)
        //{
        //    mainMenuPanel.SetActive(false);
        //}
        //else if (exitState == GameState.OnePlayer || exitState == GameState.TwoPlayers)
        //{
        //    gameplayMenu.SetActive(false);
        //}
    }

    void OnStateEnter(GameState enterState)
    {
        if (enterState == GameState.Menu)
        {
            gameplayMenu.SetActive(false);
            mainMenuPanel.SetActive(true);
            messagePanel.Hide();
            pointsPanel.Hide();      
        }
        else
        {
            mainMenuPanel.SetActive(false);
            gameplayMenu.SetActive(true);
        }
    }

    public void SingleplayerButtonClick()
    {
        GameStates.SetState(GameState.OnePlayer);
    }

    public void CreateMatchButtonClick()
    {
        GameStates.SetState(GameState.MultiplayerHost);
    }

    public void JoinMatchButtonClick()
    {
        GameStates.SetState(GameState.MultiplayerClient);
    }

    public void BackToMenuButtonClick()
    {
        GameStates.SetState(GameState.Menu);
    }

    public void ShowMessage(UIMessage message)
    {
        switch (message.messageType)
        {
            case UIMessageType.Points:
                pointsPanel.Show(message.message, message.time);
                break;
            case UIMessageType.Notification:
                messagePanel.Show(message.message, message.time);
                break;
            default:
                break;
        }
    }

    void OnDestroy()
    {
        GameStates.OnStateExit -= OnStateExit;
        GameStates.OnStateEnter -= OnStateEnter;
    }
}
