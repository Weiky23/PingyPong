using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCatcher : MonoBehaviour, ICollisionPart
{
    Table table;
    public int winPlayer;

    public Vector3 Normal { get { return Vector3.up; } }

    public void SetCollisionHandler(ICollisionHandler table)
    {
        this.table = (Table)table;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Tags.Ball))
        {
            table.BallFlewAway(winPlayer);
        }
    }
}
