using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class SingleTargetDisabler : MonoBehaviour
{
    private void Awake()
    {
        GetComponentsInChildren<Target>().ToList().OrderBy((x) => Random.value).First().gameObject.SetActive(false);
    }
}
