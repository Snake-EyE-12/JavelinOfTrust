using System;
using TMPro;
using UnityEngine;

public class DebugText : MonoBehaviour
{
    [SerializeField] private TMP_Text textAsset;

    private void Start()
    {
        Target.container.OnUpdate += AlterAccuracyChart;
    }
    private void AlterAccuracyChart(int amount, float accuracy) => textAsset.text = $"{amount} | {accuracy}";
}
