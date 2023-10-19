using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

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

    public Tilemap temp;

    public BoundsInt seek;

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

        SwitchTileState(Colour.Red, false);
    }

    // Switch tiles based on colour specified and what state.
    // Use not operator for inverse
    void SwitchTileState(Colour colour, bool active)
    {
        // TODO if statement that selects tilemap based on colour.
        Tilemap map = temp;
        TileBase tileNew = null;
        TileBase tileChange = null;

        // TODO check what blocks are out there. Check whether it's currently active or not.
        // TODO instead keep track of these variables in game manager
        // Set up a dummy block below the map and use its coordinates to keep track.
        // https://docs.unity3d.com/ScriptReference/Tilemaps.Tilemap.GetTile.html

        //Vector3 worldPosition = new Vector3(-3, -3, 0); // Create a world position vector
        //Vector3Int cellPosition = temp.WorldToCell(worldPosition); // Convert world position to cell position

        // TileBase tileChange = temp.GetTile(cellPosition);
        //TileBase tileChange = tileArray[0];

        // Checks the current state of whether a tile is active
        //bool active = dataFromTiles[tileChange].active;

        foreach (var tile in tiles)
        {
            if (dataFromTiles[tile].colour == colour && dataFromTiles[tile].active == active)
            {
                tileChange = tile;
            }
            // If the Colour matches and the active state is the opposite from the one specified
            if (dataFromTiles[tile].colour == colour && dataFromTiles[tile].active == !active)
            {
                tileNew = tile;
            }
        }

        // It should never equal null. This is to prevent errors.
        if (tileNew != null && tileChange != null)
        {
            Debug.Log("Done");
            map.SwapTile(tileChange, tileNew);
        }
    }
}
