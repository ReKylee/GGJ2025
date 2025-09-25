using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioController : MonoBehaviour
{
    // Slider references for volume control
    public Slider musicSlider; // Slider for background music
    public Slider sfxSlider; // Slider for sound effects

    // Reference to the AudioMixer
    public AudioMixer audioMixer;

    private const string MusicVolumeKey = "MusicVolume";
    private const string SFXVolumeKey = "SFXVolume";

    void Start()
    {
        // Load saved volume preferences and apply to sliders
        float savedMusicVolume = PlayerPrefs.GetFloat(MusicVolumeKey, 1f); // Default to 1 if not saved
        float savedSFXVolume = PlayerPrefs.GetFloat(SFXVolumeKey, 1f); // Default to 1 if not saved

        // Set slider values based on saved preferences
        musicSlider.value = savedMusicVolume;
        sfxSlider.value = savedSFXVolume;

        // Apply saved volume to AudioMixer
        SetMusicVolume(savedMusicVolume);
        SetSFXVolume(savedSFXVolume);

        // Add listeners to sliders
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    // Method to set music volume
    public void SetMusicVolume(float volume)
    {
        // Convert linear volume (0-1) to decibels for AudioMixer
        audioMixer.SetFloat("MUSIC", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat(MusicVolumeKey, volume); // Save volume preference
    }

    // Method to set sound effects volume
    public void SetSFXVolume(float volume)
    {
        // Convert linear volume (0-1) to decibels for AudioMixer
        audioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat(SFXVolumeKey, volume); // Save volume preference
    }
}
