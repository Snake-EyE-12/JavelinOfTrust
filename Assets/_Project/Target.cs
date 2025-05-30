using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Target : MonoBehaviour
{
    public static TargetContainer container = new();
    [SerializeField] private CapsuleCollider2D collider;
    [SerializeField] private Light2D light;
    [SerializeField] private SpriteRenderer renderer;
    [SerializeField] private GameObject hitParticles;
    public void OnHit(Vector2 point, float powerPercent)
    {
        float accuracy = CalculateAccuracy(point, powerPercent);
        container.AddHit(this, accuracy);
        //Render stuff - Particles, Hit, Sound
        light.gameObject.SetActive(false);
        renderer.sprite = fallenTarget;
        renderer.gameObject.transform.Translate(0, -0.3f, 0);
        Instantiate(hitParticles, transform.position, transform.rotation, transform);
        collider.enabled = false;

    }

    [SerializeField] private Sprite fallenTarget;

    private float CalculateAccuracy(Vector2 point, float powerPercent)
    {
        float distance = Mathf.Abs(point.y - GetCenter().y);
        float accuracy = 1 - distance / collider.size.y;
        
        float weightedResult = (accuracy * 9f + powerPercent * 1f) / 10f;
        return weightedResult;
    }

    public Vector3 GetCenter()
    {
        return collider.bounds.center;
    }
}

public class TargetContainer
{
    private Dictionary<Target, float> targets = new();
    public void AddHit(Target target, float accuracy)
    {
        if (targets.ContainsKey(target))
        {
            float currentAccuracy = targets[target];
            if(accuracy > currentAccuracy) targets[target] = accuracy;
        }
        else
        {
            targets.Add(target, accuracy);
        }
        InvokeUpdate();
    }

    public Action<int, float> OnUpdate { get; set; } = delegate { };

    private void InvokeUpdate()
    {
        OnUpdate.Invoke(Count, GetAccuracy());
    }

    private int Count => targets.Count;

    public float GetAccuracy()
    {
        if (targets.Count == 0) return 0;
        float accuracy = 0;
        foreach (var target in targets)
        {
            accuracy += target.Value;
        }
        return accuracy / targets.Count;
    }
}