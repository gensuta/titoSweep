using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TileBehavior : MonoBehaviour
{
    [SerializeField] Sprite[] tileSprites;

    [SerializeField] private Color baseColor, highlightColor;
    [SerializeField] private SpriteRenderer sr;
    public UnitBehaviour OccupiedUnit;

    public Tile tile;

    // quick explanation of the pathfinding method A*

    // There are three costs. 
    // G Cost = dist from the starting tile
    // H Cost = dist from the end tile
    // F Cost = G Cost + H Cost

    // the unit will find the tile with the lowest f cost and add it to the list of tiles to hit
    // it will repeat the above until the player hits the target

    // there's some shit called a manhattan dist that needs to be calculated. The distance using blocks. Using ups and lefts 
    // supposedly manhattan uses the block system or some shit wuddahell

    public int G { get { return tile.G;} set { tile.G = value;} }
    public int H { get { return tile.H; } set { tile.H = value; } }
    public int F => G + H; // costs for pathfinding

    public Vector2Int location { get { return tile.location; } set { tile.location = value; } }

    public TileBehavior prevTile;
    

    public bool walkable => OccupiedUnit == null && tile.isPassable; // it's more complicated than this. you cant pass thru enemy tiles unless ur a thief.

    public static event Action<TileBehavior> OnTileSelected, OnTileHovered, OnTileExit;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = tileSprites[(int)tile.tileType];
    }

    private void OnMouseEnter()
    {
        Highlight();
    }

    private void OnMouseExit()
    {
        sr.color = baseColor;

    }

    public void Highlight()
    {
        sr.color = highlightColor;
    }

    private void OnMouseDown()
    {
        OnTileSelected?.Invoke(this);
    }

    public void SetUnit(UnitBehaviour unit) // setting a unit to the tile
    {
        if(unit.currentTile!=null) { unit.currentTile.OccupiedUnit = null; }

        OccupiedUnit= unit;
        unit.currentTile = this;
    }
}
