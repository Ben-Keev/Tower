using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu]
public class AudioSet : ScriptableObject
{
    [Tooltip("Place clips here")]
    [SerializeField] private List<AudioClip> clips;

    [Tooltip("Place clip names here")]
    [SerializeField] private List<string> clipNames;

    public Dictionary<string, AudioClip> audioDictionary;
    private void PopulateDictionary()
    {
        for(int i = 0; i < clips.Count; i++)
        {
            audioDictionary.Add(clips[i].name, clips[i]);
        }
    }

    public void Start()
    {
        PopulateDictionary();
    }
}
