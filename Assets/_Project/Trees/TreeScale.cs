using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;


public class TreeScale : MonoBehaviour
{
    [SerializeField, MinMaxSlider(0.1f, 5f)] private Vector2 scaleRange;
    [SerializeField] private List<Sprite> spriteOptions;
    [Button]
    public void Preset()
    {
        GetComponentInChildren<SpriteRenderer>().sprite = spriteOptions[Random.Range(0, spriteOptions.Count)];
        transform.localScale = new Vector3(Random.Range(scaleRange.x, scaleRange.y) * (Random.Range(0, 2) == 0 ? 1 : -1), Random.Range(scaleRange.x, scaleRange.y), 1);
    }
}