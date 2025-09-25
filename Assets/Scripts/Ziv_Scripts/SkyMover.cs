using UnityEngine;

public class SkyTransformMover : MonoBehaviour
{
    public Vector3 moveDirection = new Vector3(1, 0, 0); // Movement direction
    public float speed = 5f; // Movement speed

    void Update()
    {
        // Move the plane in the specified direction
        transform.Translate(moveDirection * speed * Time.deltaTime);

        // Optional: Reset position if it goes too far
        if (transform.position.x > 50f)
        {
            transform.position = new Vector3(-50f, transform.position.y, transform.position.z);
        }
    }
}
