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

    // Lists of each coloured block's positions
    private List<Vector3> redPositions = new List<Vector3>();
    private List<Vector3> bluePositions = new List<Vector3>();
    private List<Vector3> greenPositions = new List<Vector3>();
    private List<Vector3> yellowPositions = new List<Vector3>();

    // Link each tile to its data, making it accessible to scripts
    private Dictionary<Colour, List<Vector3>> vectorDictionary;

    [SerializeField] private List<Color> coloursList;
    private Dictionary<Colour, Color> coloursDictionary;

    /// <summary>
    /// Populates dictionary where key is enum Colour, linked to the entire list of coordinates of blocks.
    /// </summary>
    private void PopulateVectorDictionary() 
    {
        vectorDictionary = new Dictionary<Colour, List<Vector3>>();
        vectorDictionary.Add(Colour.Red, MapManager.instance.getBlockWorldPositions(Colour.Red));
        vectorDictionary.Add(Colour.Blue, MapManager.instance.getBlockWorldPositions(Colour.Blue));
        vectorDictionary.Add(Colour.Green, MapManager.instance.getBlockWorldPositions(Colour.Green));
        vectorDictionary.Add(Colour.Yellow, MapManager.instance.getBlockWorldPositions(Colour.Yellow));
    }

    /// <summary>
    /// Populates dictionary where key is Colour, linked to a Unity Color.
    /// </summary>
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
    /// <summary>
    /// Creates particles at the positions of each block.
    /// </summary>
    /// <param name="colour"></param>
    public void SpawnParticlesAtBlocks(Colour colour) 
    {
        // Grab the positions of a specific colour
        List<Vector3> positions = vectorDictionary[colour];
        GameObject spawnedParticle;

        // Grab the colour that the particle will display as linked with the enum Colour
        Color color = coloursDictionary[colour];

        // For every position of a block.
        foreach (Vector3 position in positions)
        {
            // Create a particle at that position
            spawnedParticle = Instantiate(particlePrefab, position, Quaternion.identity);

            // Make the particle a child of the ParticleSpawner (To prevent overflow in the hiearchy)
            // https://forum.unity.com/threads/solved-instantiate-as-child.30654/
            spawnedParticle.transform.parent = transform;

            //Grab the main module of the instance we just created to change its colour.
            //https://stackoverflow.com/questions/42794894/how-to-change-the-start-color-of-the-particle-system
            ParticleSystem.MainModule psMain = spawnedParticle.GetComponent<ParticleSystem>().main;
            psMain.startColor = color;
        }
    }

    /// <summary>
    /// Spawns a particle at any world position.
    /// </summary>
    /// <param name="particlePrefab">The desired particlesystem prefab</param>
    /// <param name="color">The colour of the particle as a built-in unity Color class.</param>
    /// <param name="position">The WorldPosition the particle will be spawned at</param>
    public void SpawnParticleAtWorldPosition(GameObject particlePrefab, Color color, Vector2 position)
    {
        GameObject spawnedParticle;
        // Instantiate the particle.
        spawnedParticle = Instantiate(particlePrefab, position, Quaternion.identity);

        // Grab the main module of instance to change its colour.
        ParticleSystem.MainModule psMain = spawnedParticle.GetComponent<ParticleSystem>().main;
        psMain.startColor = color;
    }

}
