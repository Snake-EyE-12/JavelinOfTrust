using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class BannerController : MonoBehaviour
{
    [SerializeField] private List<RollOutBanner> banners;
    [SerializeField] private float delayBeforeShow;
    [SerializeField] private float midDelay;


    private int currentValue;
    private int updatedTargetHitCount;

    private void OnValueUpdated(int targetsHit, float accuracy)
    {
        Debug.Log("Updated Banner Counter");
        updatedTargetHitCount = targetsHit;
    }

    private bool inFrame = false;
    public void EnterFrame()
    {
        Invoke(nameof(SetEnterFrame), delayBeforeShow);
    }

    private void SetEnterFrame()
    {
        inFrame = true;
    }

    public void ExitFrame()
    {
        inFrame = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)) updatedTargetHitCount = 10;
        if (inFrame)
        {
            UpdateBanners();
        }
    }

    [Button]
    public void UpdateBanners()
    {
        for (int i = currentValue; i < updatedTargetHitCount; i++)
        {
            if (i >= banners.Count) return;
            banners[i].StartRollOut(midDelay * (i - currentValue));
            if(BirdTargetCounter.IsBirdBanner(i)) banners[i].SetAsBirdBanner();
        }

        currentValue = updatedTargetHitCount;
    }

    private void Awake()
    {
        Target.container.OnUpdate += OnValueUpdated;
    }
    private void OnDisable()
    {
        Target.container.OnUpdate -= OnValueUpdated;
    }
}