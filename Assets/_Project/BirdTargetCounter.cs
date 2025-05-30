using System;
using System.Collections.Generic;
using UnityEngine;

public class BirdTargetCounter : MonoBehaviour
{
    private static bool birdHitBetweenTargets = false;
    private static List<bool> hits = new();

    private void Awake()
    {
        hits.Clear();
        birdHitBetweenTargets = false;
        BirdFly.OnBirdHit += OnBirdHit;
        Target.container.OnUpdate += OnTargetHit;
    }

    private void OnDisable()
    {
        BirdFly.OnBirdHit -= OnBirdHit;
        Target.container.OnUpdate -= OnTargetHit;
    }

    public static bool IsBirdBanner(int num)
    {
        if(num < hits.Count)
            return hits[num];
        return false;
    }

    public void OnTargetHit(int num, float acc)
    {
        if(num > hits.Count)
        {
            hits.Add(birdHitBetweenTargets);
            ResetBirdHitFlag();
        }
    }

    private static void ResetBirdHitFlag() => birdHitBetweenTargets = false;

    public void OnBirdHit()
    {
        birdHitBetweenTargets = true;
    }
}