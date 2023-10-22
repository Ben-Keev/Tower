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

    public void PlayPlayerSound(string clipKey)
    {
        if(PlayerDict[clipKey] != null) sfxSource.PlayOneShot(PlayerDict[clipKey]);
    }
}
