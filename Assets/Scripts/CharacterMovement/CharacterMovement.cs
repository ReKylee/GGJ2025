using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class CharacterMovement : MonoBehaviour
{
    [Header("Movement Settings")] [SerializeField]
    private float gridSize = 1f;

    [SerializeField] private float moveDuration = 0.07f;
    [SerializeField] private float jumpPower = 1f;

    [Header("Animation Settings")] [SerializeField]
    private Transform characterModel;

    [SerializeField] private Vector3 punchScale = new(0.2f, -0.2f, 0.2f);
    [SerializeField] private float punchDuration = 0.2f;
    [SerializeField] private int punchVibrato = 10;
    [SerializeField] private float punchElasticity = 1f;

    [Header("Collision Layers")] [SerializeField]
    private LayerMask wallLayer;

    [SerializeField] private LayerMask bubbleLayer;

    [Header("Audio & Camera")] public CameraShaker myCameraShaker;

    public AudioSource flapSfx;
    public UnityEvent playerBlocked;

    // References to the colliders for detecting bubbles
    public BubbleDetector forwardCollider;
    public BubbleDetector leftCollider;
    public BubbleDetector rightCollider;
    public BubbleDetector backCollider;
    public BubbleDetector landCollider;

    // Undo stack to store movement directions and popped bubbles
    private readonly Stack<MovementState> movementHistory = new();


    private bool isMoving;

    private Vector2 movementInput;
    private Vector3 targetPosition;

    private void Update()
    {
        if (isMoving || movementInput == Vector2.zero) return;

        var moveDirection = new Vector3(movementInput.x, 0, movementInput.y).normalized;


        AreAllSurroundingBubblesPopped();

        if (IsMovementBlocked(moveDirection)) return;

        PerformMovement(moveDirection);
    }

    private void OnDestroy()
    {
        DOTween.KillAll();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bubble") && !landCollider.IsPopped()) landCollider.bubble?.Explode();
    }

    private void PerformMovement(Vector3 moveDirection)
    {
        if (isMoving) return;
        isMoving = true;
        targetPosition = transform.position + moveDirection * gridSize;

        // Add squish/stretch effect
        characterModel.DOPunchScale(punchScale, punchDuration, punchVibrato, punchElasticity);

        // Jump and rotate character
        transform.DOJump(targetPosition, jumpPower, 1, moveDuration).SetEase(Ease.InOutQuad)
            .OnComplete(() => { isMoving = false; });
        characterModel.DOLookAt(transform.position + moveDirection, moveDuration).SetEase(Ease.InOutQuad);

        // Trigger camera shake
        myCameraShaker.TriggerShake();

        // Play sound effect with randomized pitch
        if (!flapSfx) return;
        flapSfx.pitch = Random.Range(2f, 2.7f);
        flapSfx.Play();
    }


    private bool IsMovementBlocked(Vector3 direction)
    {
        return IsBlockedByWalls(direction) || IsBlockedByColliders(direction);
    }

    private bool IsBlockedByWalls(Vector3 direction)
    {
        return Physics.Raycast(transform.position, direction, out var hit,
            gridSize * 1f, wallLayer);
    }

    private bool IsBlockedByColliders(Vector3 direction)
    {
        // Convert the direction to local space relative to the player
        var localDirection = transform.InverseTransformDirection(direction);


        // Define a threshold for direction alignment
        const float alignmentThreshold = 0.9f; // Adjust this if needed

        // Check which local direction the movement aligns with
        if (Vector3.Dot(localDirection.normalized, Vector3.forward) > alignmentThreshold &&
            forwardCollider.PoppedOrEmpty())
            return true; // Block movement if no bubble or if it's popped

        if (Vector3.Dot(localDirection.normalized, Vector3.back) > alignmentThreshold &&
            backCollider.PoppedOrEmpty())
            return true; // Block movement if no bubble or if it's popped

        if (Vector3.Dot(localDirection.normalized, Vector3.left) > alignmentThreshold &&
            leftCollider.PoppedOrEmpty()) return true; // Block movement if no bubble or if it's popped

        if (Vector3.Dot(localDirection.normalized, Vector3.right) > alignmentThreshold &&
            rightCollider.PoppedOrEmpty()) return true; // Block movement if no bubble or if it's popped

        // No block detected, allow movement
        return false;
    }


    public void OnMove(InputValue value)
    {
        movementInput = value.Get<Vector2>();

        if (movementInput.x != 0) movementInput.y = 0;
        if (movementInput.y != 0) movementInput.x = 0;
    }

    private void AreAllSurroundingBubblesPopped()
    {
        // Check if any of the directions (forward, left, or right) are blocked by bubbles
        if (forwardCollider.PoppedOrEmpty() && leftCollider.PoppedOrEmpty() && rightCollider.PoppedOrEmpty())
            playerBlocked?.Invoke();
    }

    private class MovementState
    {
        public MovementState(Vector3 direction, Bubble lastBubble)
        {
            Direction = direction;
            LastBubble = lastBubble;
        }

        public Vector3 Direction { get; }
        public Bubble LastBubble { get; }
    }
}