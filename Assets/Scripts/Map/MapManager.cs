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

        //SwitchTileState(Colour.Red);
    }

    // Switch tiles based on colour specified and what state.
    // https://docs.unity3d.com/ScriptReference/Tilemaps.Tilemap.SwapTile.html
    void SwitchTileState(Colour colour)
    {
        Tilemap map = getTilemapOnColour(colour);
        TilemapCollider2D mapCollision = map.GetComponent<TilemapCollider2D>();
        TileBase tileChange = getReferenceTile(map, colour);
        TileBase tileNew = getInverseTile(tileChange);
        tileNew = getInverseTile(tileChange);

        // It should never equal null. This is to prevent errors.
        if (tileNew != null && tileChange != null)
        {
            map.SwapTile(tileChange, tileNew);

            // Enable/Disable collision based on tiles active state https://learn.unity.com/tutorial/enabling-and-disabling-components#5c8a4388edbc2a001f47cdd0
            mapCollision.enabled = dataFromTiles[tileNew].active;
        }
    }

    //Returns a tilemap based on a colour
    private Tilemap getTilemapOnColour(Colour colour) {

        // Using MapData class
        MapData mapData;

        foreach (var map in maps) {
            mapData = map.GetComponent<MapData>();
            if (mapData.colour == colour) {
                return map;
            }
        }

        // This should never happen. Added to prevent errors.
        return null;
    }

    // Gets reference tile given map and colour
    // https://docs.unity3d.com/ScriptReference/Tilemaps.Tilemap.GetTile.html
    private TileBase getReferenceTile(Tilemap map, Colour colour) {
        foreach (var tile in tiles) { 
            if (dataFromTiles[tile].colour == colour)
            {
                // Get active state of tile placed in using a reference tile.
                Vector3Int cellPosition = map.WorldToCell(dataFromTiles[tile].worldPosition);
                // The tile that must be changed is the one currently placed in the world.
                // iterating is redundant and possibly harmful
                return map.GetTile(cellPosition);
            }
        }

        // This should never happen. Added to prevent errors.
        return null;
    }

    // Gets a tile of inverse active state.
    private TileBase getInverseTile(TileBase tileChange) {
        foreach (var tile in tiles)
        {
            // Active state must be opposite from tile we wish to change.
            if (dataFromTiles[tile].colour == dataFromTiles[tileChange].colour && dataFromTiles[tile].active == !dataFromTiles[tileChange].active)
            {
                // iterating is redundant
                return tile;
            }
        }

        // This should never happen. Added to prevent errors.
        return null;
    }
}
