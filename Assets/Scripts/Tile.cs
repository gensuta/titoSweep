using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile 
{
    public int G, H;
    public int F => G + H; // costs for pathfinding

    public Vector2Int location;

    public TileBehavior prevTile;

    public bool isPassable;

    public GroundType tileType;

    public int moveCost;

    public Tile(GroundType tileType, bool isPassable, int moveCost = 1)
    {
        this.tileType = tileType;
        this.isPassable = isPassable;
        this.moveCost = moveCost;
    }
}

public enum GroundType
{
    Grass =  0, //G
    Wall = 1, //W
    Water = 2, //Wa
    Mountains = 3 //M
}