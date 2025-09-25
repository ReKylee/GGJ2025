using UnityEngine;
using UnityEngine.Events;

public class Grid : MonoBehaviour
{
    public UnityEvent allExploded;
    private int bubbleNumber;

    private void Start()
    {
        bubbleNumber = transform.childCount - 1;
    }


    public void OnExplode()
    {
        bubbleNumber--;
        if (bubbleNumber <= 0)
            allExploded?.Invoke();
    }

    public void OnInflate()
    {
        // bubbleNumber++;
    }
}