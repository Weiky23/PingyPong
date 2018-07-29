using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class PointsCounter
{
    Player firstPlayer;
    Player secondPlayer;
    int firstPlayerPoints;
    int secondPlayerPoints;

    int firstPlayerStreak;
    int secondPlayerStreak;

    public PointsCounter(Player first, Player second)
    {
        this.firstPlayer = first;
        this.secondPlayer = second;
        firstPlayerPoints = 0;
        secondPlayerPoints = 0;
    }

    public void PlayerScored(int playerWin)
    {
        if (playerWin == 0)
        {
            firstPlayerPoints++;
            firstPlayerStreak++;
            secondPlayerStreak = 0;
        }
        else
        {
            secondPlayerPoints++;
            secondPlayerStreak++;
            firstPlayerStreak = 0;
        }

        if (firstPlayerPoints == 1 && secondPlayerPoints == 0)
            SendToFirst(new UIMessage(UIMessageType.Notification, "First blood!", 2f));
        else if (secondPlayerPoints == 1 && firstPlayerPoints == 0)
            SendToSecond(new UIMessage(UIMessageType.Notification, "First blood!", 2f));
        else if (firstPlayerStreak > 1 || secondPlayerStreak > 1)
            SendStreakMessage(firstPlayerStreak > 1 ? 0 : 1);


        string points = PointsMessage();

        SendToFirst(new UIMessage(UIMessageType.Points, points, 2f));
        SendToSecond(new UIMessage(UIMessageType.Points, points, 2f));
    }

    void SendStreakMessage(int playerIndex)
    {
        int streak = playerIndex == 0 ? firstPlayerStreak : secondPlayerStreak;
        string streakMessage = GetStreakMessage(streak);
        if (!string.IsNullOrEmpty(streakMessage))
        {
            UIMessage message = new UIMessage(UIMessageType.Notification, streakMessage, 2f);
            if (playerIndex == 0)
                SendToFirst(message);
            else
                SendToSecond(message);
        }

        if (streak > 2 && GameStates.Current != GameState.OnePlayer)
        {
            UIMessage message = new UIMessage(UIMessageType.Notification, "Loser!", 2f);
            if (playerIndex == 0)
                SendToSecond(message);
            else
                SendToFirst(message);
        }

    }

    string GetStreakMessage(int streak)
    {
        switch (streak)
        {
            case 2:
                return "Double kill";
            case 3:
                return "Triple kill";
            case 4:
                return "Mega kill!!";
            case 5:
                return "Ultra kill!!";
            case 6:
                return "M-m-m-monster kill!";
            case 7:
                return "Cheater";
        }
        return string.Empty;
    }

    void SendToFirst(UIMessage message)
    {
        if (firstPlayer != null)
        {
            firstPlayer.RpcSendMessage(message);
        }
    }

    void SendToSecond(UIMessage message)
    {
        if (secondPlayer != null)
        {
            secondPlayer.RpcSendMessage(message);
        }
    }

    string PointsMessage()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(firstPlayerPoints);
        sb.Append(" : ");
        sb.Append(secondPlayerPoints);
        return sb.ToString();
    }
}
