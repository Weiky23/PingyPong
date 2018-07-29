using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Bat))]
[RequireComponent(typeof(UserInputHandler))]
public class Player : NetworkBehaviour
{
    Bat bat;
    UserInputHandler userInputHandler;
    UI ui;
    [SyncVar]
    int playerIndex;
    public int PlayerIndex { get { return playerIndex; } }

    [ClientRpc]
    public void RpcInitialize(int playerIndex, Vector3 position, Vector3 levelLeftBound, Vector3 levelRightBound)
    {
        if (!isLocalPlayer)
            return;

        this.playerIndex = playerIndex;
        CmdSetPlayerIndex(playerIndex);

        bat = GetComponent<Bat>();
        bat.Set(position, levelLeftBound, levelRightBound);

        userInputHandler = GetComponent<UserInputHandler>();
        userInputHandler.Initialize();

        ui = FindObjectOfType<UI>();
    }

    [Command]
    void CmdSetPlayerIndex(int index)
    {
        playerIndex = index;
    }

    [ClientRpc]
    public void RpcSendMessage(UIMessage message)
    {
        if (!isLocalPlayer)
            return;

        ui.ShowMessage(message);
    }
}
