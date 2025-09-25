using DG.Tweening;
using UnityEngine;

public class BouncyText : MonoBehaviour
{
    [SerializeField] private float bounceScale = 1.2f; // Max scale for bouncing
    [SerializeField] private float moveOffset = 10f; // Offset for vertical movement
    [SerializeField] private float rotationAmount = 15f; // Rotation angle
    [SerializeField] private int bpm = 107; // Music BPM for syncing
    [SerializeField] private RectTransform textTransform; // RectTransform of the text

    private void Start()
    {
        if (!textTransform) textTransform = GetComponent<RectTransform>();

        // Calculate beat duration based on BPM
        var beatDuration = 60f / bpm;

        // Chain the animations for a dancing effect
        textTransform.DOScale(bounceScale, beatDuration / 2f)
            .SetEase(Ease.OutQuad)
            .SetLoops(-1, LoopType.Yoyo)
            .SetUpdate(true); // Ignore time scale for this animation

        textTransform.DOLocalMoveY(textTransform.localPosition.y + moveOffset, beatDuration / 2f)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo)
            .SetUpdate(true); // Ignore time scale for this animation

        textTransform.DORotate(new Vector3(0, 0, rotationAmount), beatDuration / 2f, RotateMode.LocalAxisAdd)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo)
            .SetUpdate(true); // Ignore time scale for this animation
    }

    public void ResetAnimation()
    {
        Start();
    }
}