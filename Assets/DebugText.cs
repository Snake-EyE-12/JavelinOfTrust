using System;
using TMPro;
using UnityEngine;

public class DebugText : MonoBehaviour
{
    [SerializeField] private TMP_Text textAsset;

    private void Awake()
    {
        Target.container.OnUpdate += AlterAccuracyChart;
    }
    private void OnDisable()
    {
        Target.container.OnUpdate -= AlterAccuracyChart;
    }
    private void AlterAccuracyChart(int amount, float accuracy) => textAsset.text = $"{amount} | {accuracy}";
}
