using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Edge : MonoBehaviour, ICollisionPart
{
    public Transform facing;
    [HideInInspector]
    public Vector3 normal;
    bool canCollide = true;

    ICollisionHandler collisionHandler;

    public Vector3 Normal { get { return normal; } }

    void OnEnable()
    {
        normal = new Vector3(facing.position.x - transform.position.x, facing.position.y - transform.position.y, 0f).normalized;
        canCollide = true;
    }

    public void SetCollisionHandler(ICollisionHandler collisionHandler)
    {
        this.collisionHandler = collisionHandler;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Tags.Ball) && canCollide)
        {
            collisionHandler.HandleCollision(collision.gameObject, this);
        }
    }

    public void PermitColliding()
    {
        StopAllCoroutines();
        canCollide = true;
    }

    public void ForbidColliding()
    {
        StopAllCoroutines();
        canCollide = false;
    }

    public void ForbidColliding(float time)
    {
        StopAllCoroutines();
        canCollide = false;
        StartCoroutine(RestoreAbilitityToCollide(time));
    }

    IEnumerator RestoreAbilitityToCollide(float time)
    {
        yield return new WaitForSeconds(time);
        canCollide = true;
    }
}