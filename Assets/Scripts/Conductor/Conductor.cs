using UnityEngine;
using UnityEngine.Events;

public class Conductor : MonoBehaviour
{
    // Song-related variables
    public float songBpm = 129; // Song beats per minute
    public float secPerBeat = 2; // Seconds per beat
    public float songPosition; // Current song position in seconds
    public float songPositionInBeats; // Current song position in beats
    public float dspSongTime; // Time since the song started
    public float firstBeatOffset; // Offset for the first beat in seconds

    [SerializeField] private AudioSource musicSource; // AudioSource to play the music

    // Loop-related variables
    public float beatsPerLoop = 8; // Number of beats in one loop
    public int completedLoops; // Total completed loops
    public float loopPositionInBeats; // Current position within the loop in beats

    public float loopPositionInAnalog; // Current relative position in the loop (0 to 1)


    // Unity Events
    public UnityEvent<float> onBeat;
    public UnityEvent onLoop;
    private int lastBeat = -1;

    // Static instance for easy access
    public static Conductor instance { get; private set; }

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this);
        else
            instance = this;
    }

    private void Start()
    {
        songPosition = 0; // Current song position in seconds
        songPositionInBeats = 0; // Current song position in beats
        // Loop-related variables
        completedLoops = 0; // Total completed loops
        loopPositionInBeats = 0; // Current position within the loop in beats
        loopPositionInAnalog = 0; // Current relative position in the loop (0 to 1)


        // Calculate seconds per beat
        secPerBeat = 60f / songBpm;

        // Record the time the music starts
        dspSongTime = (float)AudioSettings.dspTime;

        // Start playing the music
        musicSource?.Play();
    }

    private void Update()
    {
        // Calculate song position in seconds, accounting for the offset
        songPosition = (float)(AudioSettings.dspTime - dspSongTime - firstBeatOffset);

        // Calculate song position in beats
        songPositionInBeats = songPosition / secPerBeat;

        // Handle loop position calculations
        if (songPositionInBeats >= (completedLoops + 1) * beatsPerLoop)
        {
            completedLoops++;
            onLoop?.Invoke();
        }

        loopPositionInBeats = songPositionInBeats - completedLoops * beatsPerLoop;
        loopPositionInAnalog = loopPositionInBeats / beatsPerLoop;

        // Determine the current beat
        var currentBeat = Mathf.FloorToInt(songPositionInBeats);

        // Trigger the OnBeat event if a new beat is detected
        if (currentBeat != lastBeat)
        {
            lastBeat = currentBeat;
            onBeat?.Invoke(loopPositionInBeats);
        }
    }

    public void ResetTimer()
    {
        songPosition = 0; // Current song position in seconds
        songPositionInBeats = 0; // Current song position in beats
        // Loop-related variables
        completedLoops = 0; // Total completed loops
        loopPositionInBeats = 0; // Current position within the loop in beats
        loopPositionInAnalog = 0; // Current relative position in the loop (0 to 1)

        // Calculate seconds per beat
        secPerBeat = 60f / songBpm;

        // Record the time the music starts
        dspSongTime = (float)AudioSettings.dspTime;
    }
}