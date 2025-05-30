using UnityEngine;
using UnityEngine.Events;

public class OnBecomeVisibleController : MonoBehaviour
{
    [SerializeField] private UnityEvent BecomeVisibleEvent;
    [SerializeField] private UnityEvent BecomeInvisibleEvent;
    private void OnBecameVisible()
    {
        BecomeVisibleEvent.Invoke();
    }

    private void OnBecameInvisible()
    {
        BecomeInvisibleEvent.Invoke();
    }
}