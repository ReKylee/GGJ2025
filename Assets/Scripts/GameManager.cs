using System.Collections;
using System.Collections.Generic;
using EasyTransition;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("UI and Transitions")] [SerializeField]
    private TransitionSettings backToMainMenuTransition;

    [SerializeField] private GameObject transitionScreen;
    [SerializeField] private GameObject gameScreen;
    [SerializeField] private Image resultImage;
    [SerializeField] private Sprite winSprite;
    [SerializeField] private Sprite loseSprite;
    [SerializeField] private TextMeshProUGUI instructionText;
    [SerializeField] private UnityEvent levelLoaded;
    [SerializeField] private UnityEvent transitionLoaded;

    [SerializeField] private GameObject instructionImage;
    [SerializeField] private GameObject birdAnimImage;

    [Header("Level Settings")] [SerializeField]
    private Transform levelSpawnPoint;

    [SerializeField] private List<GameObject> levels;

    [Header("Game Settings")] [SerializeField]
    private int maxHealth = 3;


    [Header("Audio and Effects")] //
    [SerializeField]
    private AudioClip successSfx;

    [SerializeField] private AudioClip failureSfx;

    [SerializeField] private AudioClip winMusic;

    [SerializeField] private AudioSource sfxSource;

    [SerializeField] private ParticleSystem confettiParticles;

    [Header("Events")] public UnityEvent<int> onHealthChanged;

    [SerializeField] private GameObject gameRoom;

    private GameObject currentLevel;

    private int currentLevelIndex;


    private int health;

    private bool levelEnded;


    private TransitionManager transitionManager;


    private void Awake()
    {
        health = maxHealth;
        currentLevelIndex = 0;
    }

    private void Start()
    {
        transitionManager = TransitionManager.Instance();
        ShowTransition();
    }


    // Shows transition screen
    private void ShowTransition()
    {
        transitionScreen.SetActive(true);
        gameScreen.SetActive(false);
        instructionText.text = "";
        transitionLoaded?.Invoke();
    }

    private void ShowGameScreen()
    {
        gameScreen.SetActive(true);
        transitionScreen.SetActive(false);
    }

    //Shows game screen
    public void HandleLoadLevel()
    {
        if (currentLevel) Destroy(currentLevel);

        gameRoom.SetActive(currentLevelIndex is not (3 or 7));
        gameScreen.SetActive(currentLevelIndex is not (3 or 7));

        currentLevel = Instantiate(levels[currentLevelIndex], levelSpawnPoint);
        instructionText.text = GetLevelInstruction(currentLevelIndex);

        var levelScript = currentLevel.GetComponent<Level>();
        levelScript.onLevelWon.AddListener(LevelWon);
        levelScript.onLevelLost.AddListener(LevelLost);
        levelEnded = false;
        levelLoaded?.Invoke();
        ShowGameScreen();
    }


    private void HandleEndLevel()
    {
        if (health <= 0)
        {
            EndGame(false); // End the game if out of health
        }
        else if (currentLevelIndex >= levels.Count)
        {
            EndGame(true); // End the game if the player has won
        }
        else
        {
            if (currentLevel) Destroy(currentLevel);
            ShowTransition();
        }
    }

    private void LevelWon()
    {
        levelEnded = true;
        instructionImage.SetActive(false);
        birdAnimImage.SetActive(true);
        StartCoroutine(PlayWinEffects());
    }

    private IEnumerator PlayWinEffects()
    {
        currentLevelIndex++;
        sfxSource.PlayOneShot(successSfx);
        confettiParticles?.Play();
        yield return new WaitForSeconds(3f);
        HandleEndLevel();
    }

    public void LevelLost()
    {
        if (levelEnded) return;
        levelEnded = true;
        health--;
        onHealthChanged?.Invoke(health);
        instructionImage.SetActive(true);
        birdAnimImage.SetActive(false);
        sfxSource.PlayOneShot(failureSfx);
        HandleEndLevel();
    }

    private void EndGame(bool isWin)
    {
        resultImage.sprite = isWin ? winSprite : loseSprite;
        resultImage.gameObject.SetActive(true);

        sfxSource.PlayOneShot(isWin ? winMusic : failureSfx);

        transitionManager.Transition("MainMenu", backToMainMenuTransition, 5f);
    }


    private string GetLevelInstruction(int index)
    {
        return index switch
        {
            0 => "POP!",
            1 => "POP MORE!",
            2 => "POPPIN'!",
            3 => "FOLLOW ARROW!",
            4 => "POP X!",
            5 => "X POPIN'",
            6 => "X POPPER",
            7 => "FLY HOME!",
            _ => ""
        };
    }

    public void HandlePlayerBlocked()
    {
        // Add behavior here, such as ending the level or showing a message
        LevelLost();
    }
}