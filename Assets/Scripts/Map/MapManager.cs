using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class MapManager : MonoBehaviour
{    
    // https://github.com/naoisecollins/GD2a-PlayerController/blob/main/Assets/Scripts/AudioManager.cs
    // Instansiate singleton
    public static MapManager instance;

    //https://www.youtube.com/watch?v=XIqtZnqutGg&t=59s
    [SerializeField] private Tilemap[] maps;

    // List of all tile types
    [SerializeField] private TileBase[] tiles;

    // List of all tile data scriptableObjects created
    [SerializeField] private List<TileData>  tileDatas;

    // Link each tile to its data, making it accessible to scripts
    private Dictionary<TileBase, TileData> dataFromTiles;

    /// <summary>
    /// Populate dictionary of tiles as linked to their tile's data.
    /// </summary>
    private void PopulateDictionary()
    {
        dataFromTiles = new Dictionary<TileBase, TileData>();

        foreach (var tileData in tileDatas)
        {
            // First the tile is added, then the tiles data.
            dataFromTiles.Add(tileData.tile, tileData);
        }
    }

    // https://github.com/naoisecollins/GD2a-PlayerController/blob/main/Assets/Scripts/AudioManager.cs
    private void initSingleton() {
        if(instance == null)
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

    /// <summary>
    /// Turn two sets of blocks on by default. Necessary to create swapping effect.
    /// </summary>
    /// <param name="colourOne"></param>
    /// <param name="colourTwo"></param>
    private void initBlocks(Colour colourOne, Colour colourTwo) {
        SwitchTileState(colourOne);
        SwitchTileState(colourTwo);
    }

    // Fill in the dictionary
    private void Awake()
    {
        initSingleton();
        PopulateDictionary();
        initBlocks(Colour.Red, Colour.Green);
    }

    // https://docs.unity3d.com/ScriptReference/Tilemaps.Tilemap.SwapTile.html
    /// <summary>
    /// Switch tiles based on colour specified. Checks current state automatically.
    /// </summary>
    /// <param name="colour"></param>
    public void SwitchTileState(Colour colour)
    {
        // Tilemap we modify
        Tilemap map = getTilemapOnColour(colour);
        // Tilemap's collision
        TilemapCollider2D mapCollision = map.GetComponent<TilemapCollider2D>();
        // The tile currently in use
        TileBase tileChange = getReferenceTile(map, colour);
        // The new tile we wish to use
        TileBase tileNew = getInverseTile(tileChange);

        // It should never equal null. This is to prevent compilation errors.
        if (tileNew != null && tileChange != null)
        {
            map.SwapTile(tileChange, tileNew);

            // Enable/Disable collision based on tiles active state https://learn.unity.com/tutorial/enabling-and-disabling-components#5c8a4388edbc2a001f47cdd0
            mapCollision.enabled = dataFromTiles[tileNew].active;
        }
    }

    /// <summary>
    /// Returns Tilemap based on the enum Colour
    /// </summary>
    /// <param name="colour">enum Colour of Tilemap's blocks</param>
    /// <returns></returns>
    public Tilemap getTilemapOnColour(Colour colour) {

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
    /// <summary>
    /// Gets reference tile from world
    /// </summary>
    /// <param name="map">Tilemap of colour</param>
    /// <param name="colour">Colour of blocks being sought</param>
    /// <returns></returns>
    private TileBase getReferenceTile(Tilemap map, Colour colour) {
        foreach (var tile in tiles) { 
            // If the tile's data is equal to the colour.
            if (dataFromTiles[tile].colour == colour)
            {
                // Get active state of tile placed in using a reference tile.
                Vector3Int cellPosition = map.WorldToCell(dataFromTiles[tile].worldPosition);
                // The tile that must be changed is the one currently placed in the world.
                // iterating is redundant and possibly harmful
                return map.GetTile(cellPosition);
            }
        }

        // This should never happen. Added to prevent compilation errors.
        return null;
    }

    /// <summary>
    /// Gets a tile of inverse active state
    /// </summary>
    /// <param name="tileChange">Tile whose state you wish to invert</param>
    /// <returns>A new tile of inverted action state</returns>
    private TileBase getInverseTile(TileBase tileChange) {

        // Prevents error if function is called in a scene without a set of coloured blocks
        if (tileChange != null) {
            foreach (var tile in tiles)
            {
                // Active state must be opposite from tile we wish to change.
                if (dataFromTiles[tile].colour == dataFromTiles[tileChange].colour && dataFromTiles[tile].active == !dataFromTiles[tileChange].active)
                {
                    // iterating is redundant
                    return tile;
                }
            }
        }

        // This should never happen. Added to prevent errors.
        return null;
    }

    // https://docs.unity3d.com/2017.2/Documentation/ScriptReference/Tilemaps.Tilemap.GetTilesBlock.html
    // https://docs.unity3d.com/2017.2/Documentation/ScriptReference/Tilemaps.Tilemap-cellBounds.html,  https://docs.unity3d.com/ScriptReference/BoundsInt-allPositionsWithin.html
    // https://www.c-sharpcorner.com/article/what-is-the-difference-between-c-sharp-array-and-c-sharp-list/

    /// <summary>
    /// Gets cellpositions of all blocks of a given enum Colour.
    /// </summary>
    /// <param name="colour">Enum colour of blocks</param>
    /// <returns>List of cellpositions (as opposed to worldpositions)</returns>
    public List<Vector3Int> getBlockCellPositions(Colour colour) {
        
        List<Vector3Int> positions = new List<Vector3Int>();
        Tilemap map = getTilemapOnColour(colour);
        
        // Iterate every position that exists within the tilemap's bounds
        foreach (var cellPosition in map.cellBounds.allPositionsWithin) {
            // Check whether or not there is a tile there
            if (map.GetTile(cellPosition) != null) {
                // Add its position
                positions.Add(cellPosition);
            }
        }

        return positions;
    }

    // Converts Cellpositions to worldpositions
    // https://docs.unity3d.com/ScriptReference/Tilemaps.Tilemap.GetCellCenterWorld.html
    /// <summary>
    /// Converts cellpositions of blocks to worldpositions. Necessary for calling particles to a block's location.
    /// </summary>
    /// <param name="colour">Enum Colour of blocks</param>
    /// <returns>A list containing worldpositions of each block of enum Colour</returns>
    public List<Vector3> getBlockWorldPositions(Colour colour)
    {
        // Get tilemap of colour
        Tilemap map = getTilemapOnColour(colour);
        // Get cell positions of blocks
        List<Vector3Int> listInt = getBlockCellPositions(colour);

        List<Vector3> listConvert = new List<Vector3>();
        Vector3 worldposition;

        foreach (Vector3Int cellposition in listInt)
        {
            // Calls method that returns the cell center's position as a world position.
            worldposition = map.GetCellCenterWorld(cellposition);
            listConvert.Add(worldposition);
        }

        return listConvert;
    }
}
