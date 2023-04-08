using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehvaior : UnitBehaviour
{
    public bool turnTaken = false;
    EnemyMovement enemyMovement;

    public void Start()
    {
        base.Start();
        enemyMovement = FindObjectOfType<EnemyMovement>();

        enemyMovement.enemies.Add(this);    
    }
}
