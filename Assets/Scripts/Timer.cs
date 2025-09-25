using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip timerTick;
    [SerializeField] private AudioClip alarm;

    public UnityEvent timeout;
    private int currentTick;
    private int maxTicks;

    private void Awake()
    {
        maxTicks = transform.childCount;
    }

    private void Start()
    {
        Restart();
    }

    private void OnEnable()
    {
        Restart();
    }


    /// <summary>
    ///     Resets the timer and lights, ready for a new countdown.
    /// </summary>
    public void Restart()
    {
        currentTick = 0;
        ResetLights();
    }

    /// <summary>
    ///     Called when the Conductor triggers the OnBeat event.
    /// </summary>
    public void Tick()
    {
        if (currentTick >= maxTicks) return;

        currentTick++;
        UpdateLights();

        if (currentTick < maxTicks)
        {
            audioSource?.PlayOneShot(timerTick,0.3f); // Play ticking sound
        }
        else
        {
            audioSource?.PlayOneShot(alarm,0.25f); // Play alarm sound
            timeout?.Invoke(); // Trigger the Timeout event
        }
    }

    /// <summary>
    ///     Resets all lights to their active state.
    /// </summary>
    private void ResetLights()
    {
        for (var i = 0; i < transform.childCount; i++)
        {
            var lightObject = transform.GetChild(i);
            lightObject.gameObject.SetActive(true);

            var secondaryLight = lightObject.GetChild(1);
            if (secondaryLight != null) secondaryLight.gameObject.SetActive(true);
        }
    }

    /// <summary>
    ///     Disables the appropriate light based on the current tick.
    /// </summary>
    private void UpdateLights()
    {
        if (currentTick <= 0 || currentTick > transform.childCount) return;

        var lightObject = transform.GetChild(transform.childCount - currentTick);

        // Disable the secondary child (if any) for visual feedback
        var secondaryLight = lightObject.GetChild(1);
        if (secondaryLight != null) secondaryLight.gameObject.SetActive(false);
    }
}