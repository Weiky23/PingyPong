using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStates : MonoBehaviour
{
    static GameState current = GameState.None;
    public static GameState Current { get { return current; } }

    public static event Action<GameState> OnStateExit = delegate { };
    public static event Action<GameState> OnStateEnter = delegate { };

    void Start()
    {
        SetState(GameState.Menu);
    }

    public static void SetState(GameState state)
    {
        if (current == state)
            return;

        OnStateExit(current);

        current = state;
        OnStateEnter(state);
    }
}
