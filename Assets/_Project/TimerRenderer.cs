using TMPro;
using UnityEngine;

public class TimerRenderer : MonoBehaviour
{
    [SerializeField] private GameTimer timer;
    [SerializeField] private TMP_Text textbox;
    private void Update()
    {
        float time = timer.GetTime(); // e.g. 65.23325324 seconds

        int totalSeconds = Mathf.FloorToInt(time);
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;

        // Get milliseconds from the fractional part of time
        int milliseconds = Mathf.FloorToInt((time - totalSeconds) * 1000);

        // Format with milliseconds included (3 digits)
        string formatted = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);

        textbox.text = formatted;
    }

}