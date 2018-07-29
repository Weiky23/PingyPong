using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UIMessage
{
    public UIMessageType messageType;
    public string message;
    public float time;

    // конструктор чтобы летало по сети
    public UIMessage()
    {

    }

    public UIMessage(UIMessageType messageType, string message, float time)
    {
        this.messageType = messageType;
        this.message = message;
        this.time = time;
    }
}