using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// either interface are an inheritable parent. For players, parents, etc
// Goal is to get the character moving
//CHARACTER DETAILS (stats, names, etc) SHOULD BE SCRIPTABLE
// oh...maybe we dont need an interface bc we can use a scriptable object.

// need a gameObject that's called "baseUnit" or "characterPrefab" for spawning


/// <summary>
/// UnitBehaviour is the base script for all Units. It can be inherited 
/// </summary>
public class UnitBehaviour : MonoBehaviour
{
    public TileBehavior currentTile; // tile unit is standing on

    public Unit unit; // has all the info the unit needs! Like whether we're an enemy/ally etc.

    public Faction faction => unit.faction;

    protected SpriteRenderer sr;

    public bool turnTaken;


    public virtual void Start()
    {
        sr = GetComponent<SpriteRenderer>();    
        if (unit.level <= 1)
        {
            unit.level = 1;
            unit.SetBaseStats();
        }

        sr.sprite = unit.unitIcon;
    }
}

