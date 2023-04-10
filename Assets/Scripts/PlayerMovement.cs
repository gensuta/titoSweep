using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, UnitMovement
{
    public UnitBehaviour SelectedUnit { get; set; }

    public PathFinder pathFinder { get; set; }
    public List<TileBehavior> path { get; set; }
    public float speed { get; set; }

    public List<HeroBehavior> players = new List<HeroBehavior>();

    private void OnEnable()
    {
        TileBehavior.OnTileSelected += OnTileChosen;
    }

    private void OnDisable()
    {
        TileBehavior.OnTileSelected -= OnTileChosen;
    }


    private void Start()
    {
        pathFinder = new PathFinder();
        path = new List<TileBehavior>();
        speed = 8f;
    }

    public void SelectUnit(UnitBehaviour unit)
    {
        SelectedUnit = unit;
    }

    public void OnTileChosen(TileBehavior tile)
    {

        UnitBehaviour unit = tile.OccupiedUnit;

        if (unit != null)
        {
            if (unit.faction == Faction.Hero)
            {
                SelectUnit(unit);
                GetTilesWithinRange();
            }
            else
            {
                // attempts to kill enemy ( bring up ui for what attack to use and confirm ur attack )
                if (SelectedUnit != null && unit.faction == Faction.Enemy)
                {
                    Destroy(unit.gameObject);
                    SelectUnit(null);
                }
                else
                {
                    //display info for ally, recruitable, or enemy. Need to show their attack distance too.
                    // maybe fire off an event to show info!
                }
            }
        }
        // move our unit if there's nobody on the tile / if it's passable
        else
        {
            if (SelectedUnit != null && tile.walkable)
            {
                List<TileBehavior> tilesInRange = GetTilesWithinRange();
                if (tilesInRange.Contains(tile)) // must be within the player's movement range or w/e
                {
                    SetPath(tile);
                    tile.SetUnit(SelectedUnit);
                    SelectedUnit.turnTaken = true;
                    if (isHeroTurnFinished()) { FinishPlayerTurn(); }
                    ClearPath(tilesInRange);
                }
            }
        }
    }

    public void FinishPlayerTurn()
    {
        foreach (var p in players) { p.turnTaken = false; }
        StateManager.Instance.ChangeState(GameState.EnemiesTurn);
    }

    bool isHeroTurnFinished()
    {
        int amtFinished = 0;
        foreach(HeroBehavior hero in players)
        {
            if(hero.turnTaken) amtFinished++;
        }

        if(amtFinished == players.Count) return true;
         else return false;
    }

    private void ClearPath(List<TileBehavior> path)
    {
        foreach (TileBehavior tile in path)
        {
            tile.UnHighlight();
        }
    }

    public void SetPath(TileBehavior end)
    {
        path = pathFinder.FindPath(SelectedUnit.currentTile, end);
    }

    private void LateUpdate()
    {
        if (path.Count > 0)
        {
            MoveAlongPath();
        }
    }

    public void MoveAlongPath()
    {
        var step = speed * Time.deltaTime;

        Vector2 startPos = SelectedUnit.transform.position;
        Vector2 nextpos = path[0].transform.position;

        SelectedUnit.transform.position = Vector2.MoveTowards(startPos, nextpos, step);

        if (Vector2.Distance(startPos, nextpos) < 0.0001f)
        {
            if (path.Count == 1)
            {
                SelectUnit(null);
            }
            path.RemoveAt(0);
        }
    }

    public List<TileBehavior> GetTilesWithinRange()
    {
        List<TileBehavior> tiles = new List<TileBehavior>(); // the tiles in range
        Vector2 playerPos = SelectedUnit.transform.position;
        int MOV = SelectedUnit.unit.MOV;
        //add the ones that are above, below, to the right/left and then add on from there decreasing the amt of moves left

        foreach (TileBehavior t in MapBuilder.tileBehaviors.Values)
        {
            if (t.walkable)
            {
                float xCost = t.location.x - playerPos.x;
                float yCost = t.location.y - playerPos.y;

                if (xCost == 0 && yCost == 0) { tiles.Add(t); t.MovementHighlight(); continue; }

                if(Mathf.Abs(xCost) > 0 && Mathf.Abs(yCost) > 0) // diagonals
                {
                    int moveCost = 1;

                    for (int i = 0; i < MOV; i++)
                    {
                        if (!t.walkable) break;

                        if (xCost == i && yCost == i || xCost == -i && yCost == -i)
                        {
                            if (t == SelectedUnit.currentTile) continue;

                            moveCost += (t.tile.moveCost +1);

                            int totalCost = MOV - moveCost;

                            if (totalCost >= 0 && !tiles.Contains(t))
                            {
                                tiles.Add(t);
                                t.MovementHighlight();
                            }
                        }
                    }
                }

                if (Mathf.Abs(yCost) > 0) // verticals
                {
                    int moveCost = 0;

                    if (playerPos.y < MapBuilder.Height)
                    {
                        for (int i = (int)playerPos.y; i < Mathf.Abs(yCost) + MOV && i < MapBuilder.Height; i++) // if it's a positive number
                        {
                            TileBehavior vertTile = MapBuilder.GetTileAtPos(new Vector2Int((int)playerPos.x, i));
                            if (vertTile == SelectedUnit.currentTile) continue;

                            if (!vertTile.walkable) break;

                            if (vertTile != null)
                            {
                                moveCost += vertTile.tile.moveCost;
                            }

                            int totalCost = MOV - moveCost;

                            if (totalCost >= 0 && !tiles.Contains(vertTile)) { tiles.Add(vertTile); vertTile.MovementHighlight(); }

                        }
                    }
                    if (playerPos.y > 0)
                    {
                        for (int i = (int)playerPos.y; i > yCost - MOV && i > -1; i--) // if it's a negative number ( going downwards )
                        {
                            TileBehavior vertTile = MapBuilder.GetTileAtPos(new Vector2Int((int)playerPos.x, i));
                            if (vertTile == SelectedUnit.currentTile) continue;


                            if (!vertTile.walkable) break;

                            if (vertTile != null)
                            {
                                moveCost += vertTile.tile.moveCost;
                            }

                            int totalCost = MOV - moveCost;

                            if (totalCost >= 0 && !tiles.Contains(vertTile)) { tiles.Add(vertTile); vertTile.MovementHighlight(); }

                        }
                    }

                }

                if (Mathf.Abs(xCost) > 0) // horizontals
                {

                    int moveCost = 0;

                    if (playerPos.x < MapBuilder.Width)
                    {
                        for (int i = (int)playerPos.x; i < Mathf.Abs(xCost) + MOV && i < MapBuilder.Width; i++) // if it's a positive number
                        {

                            if(playerPos.x == i) continue;

                            TileBehavior horzTile = MapBuilder.GetTileAtPos(new Vector2Int(i,(int)playerPos.y));


                            if (!horzTile.walkable) break;

                            if (horzTile != null)
                            {
                                moveCost += horzTile.tile.moveCost;
                            }

                            int totalCost = MOV - moveCost;

                            if (totalCost >= 0 && !tiles.Contains(horzTile)) { tiles.Add(horzTile); horzTile.MovementHighlight(); }

                        }
                    }
                    if (playerPos.x > 0)
                    {
                        for (int i = (int)playerPos.x; i > xCost - MOV && i > -1; i--) // if it's a negative number ( going left )
                        {
                            TileBehavior horzTile = MapBuilder.GetTileAtPos(new Vector2Int(i, (int)playerPos.y));
                            if (horzTile == SelectedUnit.currentTile) continue;

                            if (!horzTile.walkable) break;

                            if (horzTile != null)
                            {
                                moveCost += horzTile.tile.moveCost;
                            }

                            int totalCost = MOV - moveCost;

                            if (totalCost >= 0 && !tiles.Contains(horzTile)) { tiles.Add(horzTile); horzTile.MovementHighlight(); }

                        }
                    }
                }

            }

        }
        return tiles;


    }
}
