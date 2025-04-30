using System;
using UnityEngine;

namespace ProceduralCharacter
{
    public abstract class Link : MonoBehaviour
    {
        [SerializeField] protected Transform anchor;

        protected virtual void Update()
        {
        }
    }

    public abstract class DistanceLink : Link
    {
        [SerializeField] protected float distance;
    }
}