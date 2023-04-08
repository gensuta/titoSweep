using System.Collections;
using System.Collections.Generic;
using System.Linq; // TODO: research this
using UnityEngine;
public class PathFinder 
{
    public List<TileBehavior> FindPath(TileBehavior startTile,TileBehavior endTile)
    {
        List<TileBehavior> openList = new List<TileBehavior>();
        List<TileBehavior> closedList = new List<TileBehavior>(); // tiles added to the list we're actually sending back

        openList.Add(startTile);

        while(openList.Count > 0)
        {
            TileBehavior currentTile = openList.OrderBy(x =>x.F).First();

            openList.Remove(currentTile);
            closedList.Add(currentTile);

            if(currentTile == endTile)
            {
                //finalize our path! we don't need to keep looping
                return GetFinishedPath(startTile, endTile);
            }

            var neighborTiles = GetNeighborTiles(currentTile);

            foreach(var neighbor in neighborTiles)
            {
                if(closedList.Contains(neighbor) || !neighbor.walkable) 
                {
                    continue; // not a valid tile
                }

                neighbor.G = GetBlockDist(startTile, neighbor);
                neighbor.H = GetBlockDist(endTile, neighbor);

                neighbor.prevTile= currentTile;

                // idk there's something messy about adding to a while loop inside a while loop...
                if(!openList.Contains(neighbor)) { openList.Add(neighbor); }
            }
        }
        return null;
    }

    public List<TileBehavior> GetFinishedPath(TileBehavior startTile, TileBehavior endTile)
    {
        List<TileBehavior> path = new List<TileBehavior>();

        TileBehavior currentTile = endTile;

        while(currentTile != startTile) 
        {
            path.Add(currentTile);
            currentTile = currentTile.prevTile;
        }

        path.Reverse(); // we built it backwards and have to flip it to move forward

        return path;
    }

    private int GetBlockDist(TileBehavior start, TileBehavior neighbor)
    {
        return Mathf.Abs(start.location.x - neighbor.location.x) + Mathf.Abs(start.location.y - neighbor.location.y);
    }

    public List<TileBehavior> GetNeighborTiles(TileBehavior tile)
    {
        var map = MapBuilder.tileBehaviors;

        List<TileBehavior> neighbors = new List<TileBehavior>();

        //TODO: please clean this up i dont like having shit repeated like this

        //up
        Vector2Int locationToCheck = new Vector2Int(tile.location.x,tile.location.y+1);
        if(map.ContainsKey(locationToCheck)) { neighbors.Add(map[locationToCheck]); }


        //down
         locationToCheck = new Vector2Int(tile.location.x, tile.location.y - 1);
        if (map.ContainsKey(locationToCheck)) { neighbors.Add(map[locationToCheck]); }


        //left
         locationToCheck = new Vector2Int(tile.location.x-1, tile.location.y);
        if (map.ContainsKey(locationToCheck)) { neighbors.Add(map[locationToCheck]); }


        //right
         locationToCheck = new Vector2Int(tile.location.x+1, tile.location.y );
        if (map.ContainsKey(locationToCheck)) { neighbors.Add(map[locationToCheck]); }

        return neighbors;
    }
}
