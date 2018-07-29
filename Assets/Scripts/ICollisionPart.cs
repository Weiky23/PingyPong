using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICollisionPart
{
    void SetCollisionHandler(ICollisionHandler table);
    Vector3 Normal { get; }
}