using UnityEngine;
using UnityEngine.Events;

public class Level : MonoBehaviour
{
    public UnityEvent onLevelLost;
    public UnityEvent onLevelWon;

    public void WinLevel()
    {
        onLevelWon?.Invoke();
    }

    public void LoseLevel()
    {
        onLevelLost?.Invoke();
    }
}