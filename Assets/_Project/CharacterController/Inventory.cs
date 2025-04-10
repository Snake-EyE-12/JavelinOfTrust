using System;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public bool IsHoldingObject()
    {
        return true;
    }

    public void UseObject(float timeCharged, Vector3 ownerVelocity, Transform owner, Vector2 direction)
    {
        javelin.Throw(timeCharged, ownerVelocity, owner, direction);
    }

    public Javelin javelin;
}