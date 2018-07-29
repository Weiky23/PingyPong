using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Balls Order", menuName = "Pingy Pong Configs/Balls Order")]
public class BallsOrder : ScriptableObject
{
    public GameObject[] balls;

    // для эдитора
    public const string ballsField = "balls";
}
