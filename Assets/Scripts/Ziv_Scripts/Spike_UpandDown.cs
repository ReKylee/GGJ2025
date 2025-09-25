using UnityEngine;

public class SpikeBallMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("Speed of the spike ball movement.")]
    public float speed = 2f;

    [Tooltip("Range of the movement along the Z-axis.")]
    public float range = 3f;

    private float initialZ;
    private float direction = 1f;

    void Start()
    {
        // Store the initial Z position
        initialZ = transform.position.z;
    }

    void Update()
    {
        // Calculate the new Z position
        float newZ = transform.position.z + direction * speed * Time.deltaTime;

        // Check if the spike ball has reached the movement range limits
        if (Mathf.Abs(newZ - initialZ) > range)
        {
            // Reverse direction
            direction *= -1f;
            newZ = Mathf.Clamp(newZ, initialZ - range, initialZ + range);
        }

        // Apply the new position
        transform.position = new Vector3(transform.position.x, transform.position.y, newZ);
    }
}
