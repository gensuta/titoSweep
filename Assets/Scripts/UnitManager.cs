using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    private List<Unit> units= new List<Unit>();

    public GameObject unitPrefab, enemyPrefab;

    private void Awake()
    {
        units = Resources.LoadAll<Unit>("Units").ToList();
    }

    public void SpawnUnits(Map map)
    {
        // FOR TESTING PURPSOES ONLY!!

        //TODO: Ensure that map labels have ways of labeling unit spawn types along with factions plz

        UnitBehaviour newUnit;

        Vector3 heroPos = new Vector3(map.tiles[0, 0].location.x, map.tiles[0, 0].location.y, 0f);
        Vector3 enemy = new Vector3(map.tiles[4, 4].location.x, map.tiles[4, 4].location.y, 0f);


        GameObject g = Instantiate(unitPrefab, heroPos, Quaternion.identity);
        newUnit = g.GetComponent<UnitBehaviour>();

        TileBehavior heroTile = MapBuilder.GetTileAtPos(new Vector2Int(0, 0));

        newUnit.currentTile = heroTile;

        newUnit.unit = units[0];

        g.name = newUnit.unit.Name;

        heroTile.SetUnit(newUnit);


        g = Instantiate(enemyPrefab, enemy, Quaternion.identity);
        newUnit = g.GetComponent<UnitBehaviour>();

        TileBehavior enemyTile = MapBuilder.GetTileAtPos(new Vector2Int(4, 4));

        newUnit.currentTile = enemyTile;

        newUnit.unit = units[1];

        g.name = newUnit.unit.Name;


        enemyTile.SetUnit(newUnit);
    }

   
    
}
