using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroBehavior : UnitBehaviour
{
    PlayerMovement playerMovement;

    public override void Start()
    {
        base.Start();
        playerMovement = FindObjectOfType<PlayerMovement>();

        playerMovement.players.Add(this);
    }
}
