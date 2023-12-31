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

    /// <summary>
    /// Generate a dictionary where clip names are keys and the clips themselves are parameters
    /// </summary>
    /// <returns>A dictionary of clip names associated with corresponding clips.</returns>
    public Dictionary<string, AudioClip> PopulateDictionary()
    {
        Dictionary<string, AudioClip> audioDictionary = new Dictionary<string, AudioClip>();

        for(int i = 0; i < clips.Count; i++)
        {
            audioDictionary.Add(clipNames[i], clips[i]);
        }

        return audioDictionary;
    }
}
