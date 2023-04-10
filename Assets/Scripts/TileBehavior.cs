using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TileBehavior : MonoBehaviour
{
    [SerializeField] Sprite[] tileSprites;

    [SerializeField] private Color baseColor, highlightColor, canMoveColor, moveHighlightColor;
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

    private bool isInPath;

    public bool walkable => OccupiedUnit == null && tile.isPassable; // it's more complicated than this. you cant pass thru enemy tiles unless ur a thief.

    public static event Action<TileBehavior> OnTileSelected, OnTileHovered, OnTileExit;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = tileSprites[(int)tile.tileType];
    }

    private void OnMouseEnter()
    {
        if (StateManager.currentState != GameState.HeroesTurn) return;
        Highlight();
    }

    private void OnMouseExit()
    {
        if (StateManager.currentState != GameState.HeroesTurn) return;

        if (!isInPath) UnHighlight();
        else sr.color = canMoveColor;
    }

    public void Highlight()
    {
        if (!isInPath) sr.color = highlightColor;
        else sr.color = moveHighlightColor;
    }

    public void MovementHighlight()
    {
        sr.color = canMoveColor;
        isInPath = true;
    }

    public void UnHighlight()
    {
        sr.color = baseColor;
        isInPath = false;
    }

    private void OnMouseDown()
    {
        if (StateManager.currentState != GameState.HeroesTurn) return;
        OnTileSelected?.Invoke(this);
    }

    public void SetUnit(UnitBehaviour unit) // setting a unit to the tile
    {
        if(unit.currentTile!=null) { unit.currentTile.OccupiedUnit = null; }

        OccupiedUnit= unit;
        unit.currentTile = this;
    }
}
