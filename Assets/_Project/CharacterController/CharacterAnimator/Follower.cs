using System;
using UnityEngine;

public class Follower : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField, Range(0, 1)] private float lerpSpeed;

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, target.position, lerpSpeed);
    }
}