using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Ball : NetworkBehaviour
{
    [SerializeField]
    float speed = 14f;
    public float Speed { get { return speed; } set { speed = value; } }

    Vector3 moveDirection;
    public Vector2 MoveDirection { get { return moveDirection; } set { moveDirection = value; } }

    bool moving = false;
    public bool Moving { get { return moving; } set { moving = value; } }

	void Update ()
    {
        if (!isServer)
            return;

        if (!moving)
            return;

        if (speed > 0f)
        {
            float distance = speed * Time.deltaTime;
            Vector3 change = moveDirection * distance;
            transform.position = transform.position + change;
        }
	}
}