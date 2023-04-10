using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehvaior : UnitBehaviour
{
    EnemyMovement enemyMovement;

    public override void Start()
    {
        base.Start();
        enemyMovement = FindObjectOfType<EnemyMovement>();

        enemyMovement.enemies.Add(this);    
    }
}
