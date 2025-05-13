using System;
using UnityEngine;

public class Decay : MonoBehaviour
{
    [SerializeField] private float lifeTime = 6f;

    private void Start()
    {
        Invoke("TryDelete", lifeTime);
    }

    private void TryDelete()
    {
        if(enabled) Destroy(this.gameObject, lifeTime);
    }
}
