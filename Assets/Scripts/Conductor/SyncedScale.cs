using UnityEngine;

public class SyncedScale : MonoBehaviour
{
    [SerializeField] private float maxScale;
    [SerializeField] private float minScale;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        var speed = 2f; // Adjust speed of scaling
        var scaleFactor = Mathf.Lerp(minScale, maxScale,
            (Mathf.Sin(Conductor.instance.loopPositionInAnalog * Mathf.PI * 2 * speed) + 1) / 2);
        transform.localScale = Vector3.one * scaleFactor;
    }
}