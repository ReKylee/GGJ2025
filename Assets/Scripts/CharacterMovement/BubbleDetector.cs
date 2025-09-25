using UnityEngine;

public class BubbleDetector : MonoBehaviour
{
    [Header("Detection Settings")] [SerializeField]
    private Vector3 boxSize = Vector3.one * 0.5f; // Size of the detection box

    [SerializeField] private LayerMask bubbleLayer; // Layer to filter bubbles
    public Bubble bubble { get; private set; }

    private void Start()
    {
        DetectBubble();
    }

    private void Update()
    {
        DetectBubble();
    }

    private void OnDrawGizmos()
    {
        // Optional: Visualize the overlap box in the editor
        Gizmos.color = bubble ? Color.green : Color.red;
        Gizmos.matrix = Matrix4x4.TRS(transform.position, Quaternion.identity, boxSize);
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
    }

    private void DetectBubble()
    {
        var colliders = new Collider[3];
        // Perform an OverlapBox at the detector's position
        var size = Physics.OverlapBoxNonAlloc(transform.position, boxSize / 2, colliders, Quaternion.identity,
            bubbleLayer);

        // Check for a bubble in the overlapping colliders
        bubble = null; // Reset bubble reference
        foreach (var col in colliders)
        {
            if (!col) return;
            var detectedBubble = col.GetComponent<Bubble>();
            if (!detectedBubble) continue;
            bubble = detectedBubble;
            return;
        }
    }

    public bool IsPopped()
    {
        return bubble && bubble.isExploded;
    }

    public bool IsEmpty()
    {
        return !bubble;
    }

    public bool PoppedOrEmpty()
    {
        return IsEmpty() || IsPopped();
    }
}