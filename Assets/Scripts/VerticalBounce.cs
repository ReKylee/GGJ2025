using DG.Tweening;
using UnityEngine;

public class VerticalBounce : MonoBehaviour
{
    [SerializeField] private float bounceAmount = 0.2f; // Amount to stretch vertically
    [SerializeField] private float bounceDuration = 0.5f; // Duration of one bounce
    [SerializeField] private float rotationAmount = 10f; // Amount to rotate during each bounce
    [SerializeField] private float rotationDuration = 0.25f; // Duration of each rotation
    private Quaternion originalRotation;

    private Vector3 originalScale;
    private Tween rotateTween;

    private Tween scaleTween;

    private void Start()
    {
        // Store the original scale and rotation
        originalScale = transform.localScale;
        originalRotation = transform.rotation;

        // Initialize the tweens right away in Start() or manage them in Update()
        InitializeTweens();
    }

    private void Update()
    {
        // Ensure that the scale and rotation tweens are active
        if (scaleTween == null || !scaleTween.IsActive())
            // Create or reset the bounce animation for vertical scale
            scaleTween = transform.DOScaleY(originalScale.y + bounceAmount, bounceDuration)
                .SetEase(Ease.InOutSine) // Smooth the bounce
                .SetLoops(-1, LoopType.Yoyo); // Loop indefinitely with a back-and-forth motion

        if (rotateTween == null || !rotateTween.IsActive())
            // Create or reset the rotation animation
            rotateTween = transform
                .DORotate(new Vector3(0, 0, rotationAmount), rotationDuration, RotateMode.LocalAxisAdd)
                .SetEase(Ease.InOutSine) // Smooth rotation
                .SetLoops(-1, LoopType.Yoyo); // Loop the rotation
    }

    private void OnDisable()
    {
        // Kill all DOTween animations when disabled
        scaleTween.Kill();
        rotateTween.Kill();

        // Optionally reset to the original state here as well
        transform.localScale = originalScale;
        transform.rotation = originalRotation;
    }

    private void InitializeTweens()
    {
        // Create the bounce animation for vertical scale
        scaleTween = transform.DOScaleY(originalScale.y + bounceAmount, bounceDuration)
            .SetEase(Ease.InOutSine) // Smooth the bounce
            .SetLoops(-1, LoopType.Yoyo); // Loop indefinitely with a back-and-forth motion

        // Create the rotation animation to make it "dance"
        rotateTween = transform.DORotate(new Vector3(0, 0, rotationAmount), rotationDuration, RotateMode.LocalAxisAdd)
            .SetEase(Ease.InOutSine) // Smooth rotation
            .SetLoops(-1, LoopType.Yoyo); // Loop the rotation
    }
}