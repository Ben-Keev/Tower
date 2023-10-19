using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


//https://www.youtube.com/watch?v=XIqtZnqutGg&t=59s
[CreateAssetMenu]
public class TileData : ScriptableObject
{
    public TileBase tile;
    public Colour colour;
    public bool active;
}
