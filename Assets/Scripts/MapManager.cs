using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{    
    //https://www.youtube.com/watch?v=XIqtZnqutGg&t=59s
    [SerializeField] private Tilemap[] maps;

    // List of all tile types
    [SerializeField] private TileBase[] tiles;

    // List of all tile data scriptableObjects created
    [SerializeField] List<TileData> tileDatas;

    // Link each tile to its data, making it accessible to scripts
    private Dictionary<TileBase, TileData> dataFromTiles;

    private void PopulateDictionary()
    {
        dataFromTiles = new Dictionary<TileBase, TileData>();

        foreach (var tileData in tileDatas)
        {
            // First the tile is added, then the tiles data.
            dataFromTiles.Add(tileData.tile, tileData);
        }
    }

    // Fill in the dictionary
    private void Awake()
    {
        PopulateDictionary();
        foreach(var tile in tiles)
        {
            Debug.Log("This is a " + dataFromTiles[tile].colour + " and it is" + dataFromTiles[tile].active);
        }
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
