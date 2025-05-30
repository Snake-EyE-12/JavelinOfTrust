using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightFader : MonoBehaviour
{
    [SerializeField] private Light2D light;
    [SerializeField] private float colorPingPongDuration;
    private float colorInitialTime;
    [SerializeField] private Color color1;
    [SerializeField] private Color color2;

    [SerializeField, MinMaxSlider(0, 30)] private Vector2 intensityRange;
    [SerializeField] private float intensityPingPongDuration;
    private float intensityInitialTime;

    private void Awake()
    {
        colorInitialTime = UnityEngine.Random.Range(0, colorPingPongDuration);
        intensityInitialTime = UnityEngine.Random.Range(0, intensityPingPongDuration);
    }

    private void Update()
    {
        light.color = Color.Lerp(color1, color2, Mathf.PingPong(Time.time + colorInitialTime, colorPingPongDuration));
        light.intensity = Mathf.Lerp(intensityRange.x, intensityRange.y, Mathf.PingPong(Time.time + intensityInitialTime, intensityPingPongDuration));
    }
}