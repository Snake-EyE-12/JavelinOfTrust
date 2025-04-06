using UnityEngine;

public class LagMachineForTesting : MonoBehaviour
{
    private void Update()
    {
        for (int i = 0; i < 1000; i++)
        {
            Debug.Log("LAG");
        }
    }
}