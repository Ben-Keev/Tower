using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using Color = UnityEngine.Color;

// https://github.com/naoisecollins/GD2a-PlayerController/blob/main/Assets/Scripts/EnemySpawner.cs
public class ParticleSpawner : MonoBehaviour
{
    public static ParticleSpawner instance;

    [SerializeField] private GameObject particlePrefab;

    private List<Vector3> redPositions = new List<Vector3>();
    private List<Vector3> bluePositions = new List<Vector3>();
    private List<Vector3> greenPositions = new List<Vector3>();
    private List<Vector3> yellowPositions = new List<Vector3>();

    // Link each tile to its data, making it accessible to scripts
    private Dictionary<Colour, List<Vector3>> vectorDictionary;

    [SerializeField] private List<Color> coloursList;
    private Dictionary<Colour, Color> coloursDictionary;

    // Use dictionaries so we can sort each list by colour.
    private void PopulateVectorDictionary() 
    {
        vectorDictionary = new Dictionary<Colour, List<Vector3>>();
        vectorDictionary.Add(Colour.Red, MapManager.instance.getBlockWorldPositions(Colour.Red));
        vectorDictionary.Add(Colour.Blue, MapManager.instance.getBlockWorldPositions(Colour.Blue));
        vectorDictionary.Add(Colour.Green, MapManager.instance.getBlockWorldPositions(Colour.Green));
        vectorDictionary.Add(Colour.Yellow, MapManager.instance.getBlockWorldPositions(Colour.Yellow));
    }

    // Sort each unity color by the Colour class
    private void PopulateColoursDictionary()
    {
        coloursDictionary = new Dictionary<Colour, Color>();
        coloursDictionary.Add(Colour.Red, Color.red);
        coloursDictionary.Add(Colour.Blue, Color.blue);
        coloursDictionary.Add(Colour.Green, Color.green);
        coloursDictionary.Add(Colour.Yellow, Color.yellow);
    }

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

    private void Start() 
    {
        initSingleton();
        PopulateVectorDictionary();
        PopulateColoursDictionary();
    }

    // https://github.com/naoisecollins/GD2a-PlayerController/blob/main/Assets/Scripts/EnemySpawner.cs
    public void SpawnParticlesAtBlocks(Colour colour) 
    {
        List<Vector3> positions = vectorDictionary[colour];
        GameObject spawnedParticle;

        Color color = coloursDictionary[colour];

        foreach (Vector3 position in positions)
        {
            spawnedParticle = Instantiate(particlePrefab, position, Quaternion.identity);

            // https://forum.unity.com/threads/solved-instantiate-as-child.30654/
            spawnedParticle.transform.parent = transform;

            //https://stackoverflow.com/questions/42794894/how-to-change-the-start-color-of-the-particle-system
            ParticleSystem.MainModule psMain = spawnedParticle.GetComponent<ParticleSystem>().main;
            psMain.startColor = color;
        }
    }

    // This method is not tied to blocks, so it uses Color.
    public void SpawnParticleAtWorldPosition(GameObject particlePrefab, Color color, Vector2 position)
    {
        GameObject spawnedParticle;
        spawnedParticle = Instantiate(particlePrefab, position, Quaternion.identity);
        ParticleSystem.MainModule psMain = spawnedParticle.GetComponent<ParticleSystem>().main;
        psMain.startColor = color;
    }

}
