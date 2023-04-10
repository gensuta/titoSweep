using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour, UnitMovement
{

    [SerializeField] private float moveSpeed;
    public UnitBehaviour SelectedUnit { get; set; }

    public PathFinder pathFinder { get; set; }
    public List<TileBehavior> path { get; set; }
    public float speed { get; set; }

    public List<EnemyBehvaior> enemies = new List<EnemyBehvaior>();

    private void Start()
    {
        pathFinder = new PathFinder();
        path = new List<TileBehavior>();
        speed = 6.5f;
    }

    public UnitBehaviour[] GetUnits() { return FindObjectsOfType<UnitBehaviour>(); }

    public void MoveAlongPath()
    {
        var step = speed * Time.deltaTime;

        Vector2 startPos = SelectedUnit.transform.position;
        Vector2 nextpos = path[0].transform.position;

        SelectedUnit.transform.position = Vector2.MoveTowards(startPos, nextpos, step);

        if (Vector2.Distance(startPos, nextpos) < 0.0001f)
        {
            path.RemoveAt(0);
        }
    }

    public void MoveEnemies()
    {
        StartCoroutine(MovingEnemies());
    }

    private IEnumerator MovingEnemies() // enemy doesn't move unless a player/ally is within range
    {
        int i = 0;
        yield return new WaitForSeconds(0.6f);

        while (i < enemies.Count)
        {
            SelectUnit(enemies[i]);

            UnitBehaviour closestUnit = null;
            float maxDist = enemies[i].unit.MOV;
            Vector2Int enemyPos = enemies[i].currentTile.location;


            foreach (UnitBehaviour u in GetUnits())
            {
                if(u.faction == Faction.Hero ||  u.faction == Faction.Ally || u.faction == Faction.Recruitable)
                {
                    Vector2Int uPos = new Vector2Int((int)u.transform.position.z,(int)u.transform.position.y);

                    if(Vector2Int.Distance(enemyPos, uPos) < maxDist) { closestUnit = u; }
                }
            }

            int xPos = (enemyPos.x > 0 ) ? -Random.Range(1,3) : enemyPos.x + (int)maxDist -1;
            int yPos = (enemyPos.y > 0) ? -Random.Range(0, 2) : enemyPos.y + (int)maxDist - 2;

            xPos += (int)maxDist; yPos += (int)maxDist;

            if (xPos > MapBuilder.Width) { xPos = MapBuilder.Width -1; }
            if(yPos > MapBuilder.Height) { yPos = MapBuilder.Height - 1; }

            if(xPos <0) { xPos = 0; }
            if(yPos <0) {  yPos = 0; }


            Vector2Int closePos = new Vector2Int(xPos, yPos);
            TileBehavior targetTile = MapBuilder.GetTileAtPos(closePos);

            if(!targetTile.walkable) { targetTile = MapBuilder.GetTileAtPos(new Vector2Int(closePos.x - 1, closePos.y)); }
            if (!targetTile.walkable) { targetTile = MapBuilder.GetTileAtPos(new Vector2Int(closePos.x, closePos.y+1)); }

            if (closestUnit != null)
            {
                closePos = new Vector2Int((int)closestUnit.transform.position.x, (int)closestUnit.transform.position.y);

                TileBehavior aboveTile = MapBuilder.GetTileAtPos(new Vector2Int(closePos.x, closePos.y + 1));
                TileBehavior belowTile = MapBuilder.GetTileAtPos(new Vector2Int(closePos.x, closePos.y - 1));
                TileBehavior leftTile = MapBuilder.GetTileAtPos(new Vector2Int(closePos.x - 1, closePos.y));
                TileBehavior rightTile = MapBuilder.GetTileAtPos(new Vector2Int(closePos.x + 1, closePos.y ));

                if (aboveTile != null && aboveTile.walkable) { targetTile = aboveTile; }
                else if (belowTile != null && belowTile.walkable) { targetTile = belowTile; }
                else if (leftTile != null && leftTile.walkable) { targetTile = leftTile; }
                else if (rightTile != null && rightTile.walkable) { targetTile = rightTile; }
            }

            SetPath(targetTile);

            while (!enemies[i].turnTaken)
            {
                if (path.Count > 0)
                {
                    MoveAlongPath();
                    yield return new WaitForSeconds(moveSpeed);
                }
                else
                {
                    targetTile.OccupiedUnit = SelectedUnit;
                    enemies[i].turnTaken = true;
                }
            }
            yield return new WaitForSeconds(.5f);
            i++;
        }

        FinishEnemyState();

    }

    public void FinishEnemyState()
    {
        SelectUnit(null);
        foreach (var enemy in enemies) {  enemy.turnTaken = false; }
        StateManager.Instance.ChangeState(GameState.HeroesTurn);
    }

    public void SelectUnit(UnitBehaviour unit)
    {
        SelectedUnit = unit;
    }

    public void SetPath(TileBehavior end)
    {
        path = pathFinder.FindPath(SelectedUnit.currentTile, end);
    }

    public List<TileBehavior> GetTilesWithinRange()
    {
        List<TileBehavior> tiles = new List<TileBehavior>(); // the tiles in range

        return tiles;
    }
}
