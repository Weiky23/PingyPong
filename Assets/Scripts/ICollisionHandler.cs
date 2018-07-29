using UnityEngine;

public interface ICollisionHandler
{
    void HandleCollision(GameObject collider, ICollisionPart part);
}