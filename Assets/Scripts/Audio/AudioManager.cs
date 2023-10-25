using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // https://github.com/naoisecollins/GD2a-PlayerController/blob/main/Assets/Scripts/AudioManager.cs
    // Instanstiate singleton
    public static AudioManager instance;

    // Player's Sound Effects
    [SerializeField] private AudioSet PlayerSet;
    public Dictionary<string, AudioClip> PlayerDict;

    public AudioClip backgroundClip;

    private AudioSource sfxSource;

    // https://github.com/naoisecollins/GD2a-PlayerController/blob/main/Assets/Scripts/AudioManager.cs
    private void initSingleton()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Awake()
    {
        initSingleton();
        sfxSource = GetComponent<AudioSource>();
        PlayerDict = PlayerSet.PopulateDictionary();
    }

    /// <summary>
    /// Plays a sound OneShot.
    /// </summary>
    /// <param name="clipKey">The key of the player sound in the dictionary.</param>
    public void PlayPlayerSound(string clipKey)
    {
        if(PlayerDict[clipKey] != null) sfxSource.PlayOneShot(PlayerDict[clipKey]);
    }

    /// <summary>
    /// Plays wind sound, called in main menu.
    /// </summary>
    public void PlayBackgroundWind()
    {
        sfxSource.PlayOneShot(backgroundClip);
    }

    /// <summary>
    /// Interrupts and ends all audio
    /// </summary>
    public void StopAudio()
    {
        sfxSource.Stop();
    }
}
