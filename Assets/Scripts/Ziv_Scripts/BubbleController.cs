using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class BubbleController : MonoBehaviour
{
    private static readonly int IsMoving = Animator.StringToHash("isMoving");
    private static readonly int Hurt = Animator.StringToHash("Hurt");
    private static readonly int Fly = Animator.StringToHash("Fly");
    public float moveSpeed = 20f;
    public float floatiness = 0.5f;
    public float drag = 0.99f;
    public float verticalBounce = 0.5f;
    public float bounceAmplitude = 0.1f;
    public float bounceFactor = 1.0f;
    public float maxSpeed = 10f;
    public float minSpeed = 2f;

    public Animator chirpAnimator;

    // Audio variables
    public AudioClip hurtSound;
    public AudioClip winSound;
    public AudioClip flapSound;
    public AudioClip hawkSound;
    public AudioClip bgMusic;
    public AudioClip loseSound;
    public UnityEvent gameLost;
    public UnityEvent gameWon;
    private AudioSource bgMusicAudioSource;
    private AudioSource effectsAudioSource;
    private AudioSource hawkAudioSource;
    private bool hawkChasing;
    private GameObject[] hawks;
    private bool isGameOver;

    private AudioSource movementAudioSource;

    private Rigidbody rb;
    private Vector3 velocity;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; // Assuming you don't want gravity for controlled movement
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        // Set collision detection to Continuous or ContinuousDynamic for smoother collision handling
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        velocity = Vector3.zero;
        hawks = GameObject.FindGameObjectsWithTag("Hawk");

        // Set up separate AudioSources
        movementAudioSource = gameObject.AddComponent<AudioSource>();
        hawkAudioSource = gameObject.AddComponent<AudioSource>();
        effectsAudioSource = gameObject.AddComponent<AudioSource>();
        bgMusicAudioSource = gameObject.AddComponent<AudioSource>();

        // Configure background music AudioSource
        bgMusicAudioSource.clip = bgMusic;
        bgMusicAudioSource.loop = true;
        bgMusicAudioSource.volume = 0.5f;
        bgMusicAudioSource.playOnAwake = true;
        bgMusicAudioSource.Play();
    }

    private void Update()
    {
        if (isGameOver) return;

        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
        var inputDirection = new Vector3(horizontal, 0, vertical);

        if (inputDirection.magnitude > 0.1f)
        {
            var targetVelocity = inputDirection.normalized * moveSpeed;

            // Smoothly interpolate the velocity (like velocity-based movement)
            velocity = Vector3.Lerp(velocity, targetVelocity, Time.deltaTime * floatiness);

            // Trigger movement animation
            chirpAnimator.SetBool(IsMoving, true);

            // Play flap (fly) sound if not already playing
            if (!movementAudioSource.isPlaying)
            {
                movementAudioSource.clip = flapSound;
                movementAudioSource.loop = true;
                movementAudioSource.Play();
            }
        }
        else
        {
            velocity *= drag; // Apply drag to gradually slow down
            chirpAnimator.SetBool(IsMoving, false);

            // Stop flap sound when not moving
            if (movementAudioSource.isPlaying) movementAudioSource.Stop();
        }

        velocity = Vector3.ClampMagnitude(velocity, maxSpeed); // Limit max speed

        // Calculate the bounce effect based on time (sinusoidal movement)
        var bobbing = Mathf.Sin(Time.time * verticalBounce) * bounceAmplitude;
        velocity.y = bobbing;

        // Apply velocity to Rigidbody
        rb.linearVelocity = velocity;

        // Handle hawk chasing logic
        if (hawkChasing && hawks.Length > 0)
            foreach (var hawk in hawks)
                if (hawk)
                {
                    var direction = (transform.position - hawk.transform.position).normalized;
                    const float hawkSpeed = 7f;
                    hawk.transform.position += direction * (hawkSpeed * Time.deltaTime);
                    hawk.transform.LookAt(transform.position);
                }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Hawk"))
        {
            Debug.Log("u died");

            // Trigger Hurt animation
            chirpAnimator.SetTrigger(Hurt);

            // Play hurt sound
            effectsAudioSource.PlayOneShot(hurtSound);

            // Disable the MeshRenderer of the player game object
            GetComponent<MeshRenderer>().enabled = false;

            // Start coroutine to delay setting Time.timeScale to 0
            StartCoroutine(SlowDownTime());
            isGameOver = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HawkTrigger"))
        {
            Debug.Log("Hawk triggered by trigger! It's chasing the bubble.");
            hawkChasing = true;

            // Trigger Fly animation
            chirpAnimator.SetTrigger(Fly);

            // Play hawk sound
            hawkAudioSource.PlayOneShot(hawkSound);
        }

        // Handle win condition
        if (other.CompareTag("Win") && !isGameOver)
        {
            Debug.Log("You win!");

            // Play win sound
            effectsAudioSource.PlayOneShot(winSound);

            // Stop player movement and freeze the game
            isGameOver = true;

            // Optionally delay and load a new scene
            gameWon?.Invoke();
        }
    }

    private IEnumerator SlowDownTime()
    {
        yield return new WaitForSeconds(0.2f);

        // Stop all currently playing sounds
        movementAudioSource.Stop();
        hawkAudioSource.Stop();
        effectsAudioSource.Stop();
        bgMusicAudioSource.Stop();

        // Play lose sound
        effectsAudioSource.PlayOneShot(hurtSound); // Replace with loseSound if desired

        gameLost?.Invoke();
    }
}