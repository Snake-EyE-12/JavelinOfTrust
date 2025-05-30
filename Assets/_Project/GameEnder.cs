using UnityEngine;

public class GameEnder : MonoBehaviour
{
    [SerializeField] private GameTimer timer;
    private int startingPoints = 100;
    private int birdBanners;
    private int targets = 10;
    private float accuracy;
    private float totalTime;

    private void Awake()
    {
        timer.OnEndTimer += OnGameEnd;
    }

    private void OnDisable()
    {
        timer.OnEndTimer -= OnGameEnd;
    }

    public void OnGameEnd()
    {
        //get birds
        for (int i = 0; i < targets; i++)
        {
            if(BirdTargetCounter.IsBirdBanner(i)) birdBanners++;
        }
        //get targets
        //get accuracy
        accuracy = Target.container.GetAccuracy();
        //get time
        totalTime = timer.GetTime();

        // formula = (points) * (targets) * (accuracy -> grade -> multiplier) * (birds) / (time ^ 2)
        // anim                add quickly         smack down -> squash        hammer    grow -> cut
        //order        1           2                     3                     5 (hidden)     4

        Debug.Log("Final Score: " + GetScore());
    }
    private char GetGrade(float accuracy) => accuracy > 0.9f ? 'A' : accuracy > 0.8f ? 'B' : accuracy > 0.7f ? 'C' : accuracy > 0.6f ? 'D' : 'F';
    private float GetMultiplier(char grade) => grade == 'A' ? 3f : grade == 'B' ? 2f : grade == 'C' ? 1.5f : grade == 'D' ? 1.25f : 1f;

    private float GetScore()
    {
        return (((startingPoints * targets * GetMultiplier(GetGrade(accuracy))) / (totalTime * totalTime)) * birdBanners);
    }
}