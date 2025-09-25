using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject target; // The object the camera follows
    public Vector3 offset = new(0f, 10f, 0f); // Camera's position relative to the target
    public float smoothTime = 20f; // Time for the smoothing to complete

    private void Update()
    {
        // Calculate the desired position with the offset
        var desiredPosition = target.transform.position + offset;

        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothTime * Time.smoothDeltaTime);
        // Ensure the camera looks directly downward (top-down view)
        transform.rotation = Quaternion.Euler(90f, 0f, 0f); // Fixed top-down perspective
    }
}