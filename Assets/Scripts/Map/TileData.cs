using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


//https://www.youtube.com/watch?v=XIqtZnqutGg&t=59s
[CreateAssetMenu]
public class TileData : ScriptableObject
{
    [Tooltip("Tile which the data is linked to")]
    public TileBase tile;
    [Tooltip("Used in taking player input and map manager.")]
    public Colour colour;
    public bool active;
    [Tooltip("The position of a reference tile. Determines whether tile colour is active.")]
    public Vector3Int worldPosition;
}
