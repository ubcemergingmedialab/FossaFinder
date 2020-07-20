using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Created by Kimberly Burke.
/// Handles all sound effects (UI buttons, feedback sounds, boost, etc.)
/// </summary>
public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioClip[] buttonClicks;
    [SerializeField] AudioClip startClick;

    [SerializeField] AudioClip[] correctSounds;
    [SerializeField] AudioClip errorSound;
    [SerializeField] AudioClip pauseClick;

    [SerializeField] AudioClip[] boostSounds;
    [SerializeField] AudioClip flySound;
    [SerializeField] AudioClip fastFlySound;

    private AudioSource audioSource;
    [SerializeField] private AudioSource backgroundAudio;
    private AudioSource playerAudio;

    // Start is called before the first frame update
    void Start()
    {
        SettingSaveUtility.volumeChanged.AddListener(ChangeVolume);
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.Log("Cannot find audio source for Audio Manager");
        }
        AudioUtility.MusicVolume = SettingSaveUtility.GetMusicVolume() > 0 ? true : false;
        AudioUtility.EffectVolume = SettingSaveUtility.GetEffectsVolume() > 0 ? true : false;
        ChangeVolume();
    }

    public void StartPlayerSounds()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerAudio = player.GetComponent<AudioSource>();
            playerAudio.clip = flySound;
            playerAudio.Play();
            if (ObjectiveUtility.CurrentChapter == ObjectiveUtility.Chapter.Chapter3)
            {
                playerAudio.mute = true;
            }
        }
    }

    /// <summary>
    /// Selects a click sound effect for pressing any UI button on the menus
    /// </summary>
    public virtual void PlayButtonClick()
    {
        PlayClip(buttonClicks[Random.Range(0, buttonClicks.Length - 1)]);
    }

    /// <summary>
    /// Selects click sound effect for clicking the "Start" button
    /// </summary>
    public virtual void PlayStartClick()
    {
        PlayClip(startClick);
    }

    /// <summary>
    /// Selects a sound effect for the positive feedback event
    /// </summary>
    public virtual void PlayCorrectSound()
    {
        PlayClip(correctSounds[Random.Range(0, correctSounds.Length - 1)]);
    }

    /// <summary>
    /// Selects the error sound effect for the negative feedback event
    /// </summary>
    public virtual void PlayWrongSound()
    {
        PlayClip(errorSound);
    }

    /// <summary>
    /// Selects click sound effect for clicking the "Pause" button
    /// </summary>
    public virtual void PlayPauseClick()
    {
        PlayClip(pauseClick);
    }

    /// <summary>
    /// Selects a sound effect for the boost event
    /// </summary>
    public virtual void PlayBoostSound()
    {
        PlayClip(boostSounds[Random.Range(0, boostSounds.Length)]);
    }

    /// <summary>
    /// Plays the looping flying sound effect based on the speed of the player (boosted or not)
    /// TODO: Sudden change instead of gradual pitch adjustment
    /// </summary>
    /// <param name="boost">Boolean for when boost button is being pressed or not and boosting the player</param>
    public virtual void PlayBoostedFly(bool boost)
    {
        if (boost)
        {
            playerAudio.clip = fastFlySound;
        }
        else
        {
            playerAudio.clip = flySound;
        }
        playerAudio.Play();
    }

    /// <summary>
    /// Is passed a clip to play through the audio source
    /// </summary>
    /// <param name="clip">Target audio clip to play</param>
    private void PlayClip(AudioClip clip)
    {
        if (audioSource == null)
            return;
        audioSource.clip = clip;
        audioSource.Play();
    }

    /// <summary>
    /// Called from the volume toggles - sets the volume of the sound effects
    /// </summary>
    /// <param name="volume"></param>
    public virtual void ChangeVolume()
    {
        int fxVolume = AudioUtility.EffectVolume ? 100 : 0;
        int musicVolume = AudioUtility.MusicVolume ? 100 : 0;
        audioSource.volume = fxVolume;
        if (playerAudio != null)
            playerAudio.volume = fxVolume;
        backgroundAudio.volume = musicVolume;
    }
}
