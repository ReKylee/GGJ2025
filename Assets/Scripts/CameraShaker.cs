using DG.Tweening;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    [SerializeField] private float duration = 0.5f;   // Duration of the shake
    [SerializeField] private float strength = 1f;    // Strength of the shake
    [SerializeField] private int vibrato = 10;       // How many shakes will occur
    [SerializeField] private float randomness = 90f; // Randomness of the shake

    private Transform cameraTransform;

    private void Start()
    {
        cameraTransform = Camera.main.transform; // Get the main camera's transform
    }

    public void TriggerShake()
    {
        // Apply the shake
        cameraTransform.DOShakePosition(duration, strength, vibrato, randomness)
            .SetEase(Ease.InOutQuad); // Optional easing
    }
}
