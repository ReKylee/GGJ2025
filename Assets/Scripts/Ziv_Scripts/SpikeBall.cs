using UnityEngine;

public class EnemyBallRotation : MonoBehaviour
{
    public float rotationSpeed = 100f; // Speed of rotation (degrees per second)

    void Update()
    {
        // Rotate the ball around its local Y-axis
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

        // Optional: Add rotation on other axes for more dynamic spinning
        // Uncomment the following lines if needed:
        // transform.Rotate(Vector3.right, rotationSpeed * 0.5f * Time.deltaTime);
        // transform.Rotate(Vector3.forward, rotationSpeed * 0.2f * Time.deltaTime);
    }
}
