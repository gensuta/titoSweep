using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MapBuilder : MonoBehaviour
{

    [SerializeField] private GameObject tilePrefab;

    public static Dictionary<Vector2Int, TileBehavior> tileBehaviors;

    public static int Width => currentMap.width;

    public static int Height => currentMap.height;

    public UnityEvent<Map> OnMapBuilt;

    private static Map currentMap;

    // Use a spreadsheet to create a map and the map tells the map builder how to create it. width, height, Tile info ( passable, high avo, etc.)
    // Like...you take in a csv that goes into a dictionary basically!

    public void BuildMap(Map map)
    {
        tileBehaviors = new Dictionary<Vector2Int, TileBehavior>();

        currentMap = map;

        for(int x = 0; x < map.width; x++) 
        {
            for(int y = 0; y < map.height; y++)
            {
                GameObject tileObj = Instantiate(tilePrefab, new Vector3(x,y,0),Quaternion.identity, transform);
                tileObj.name = $"Tile {x} {y}";

                TileBehavior newTile = tileObj.GetComponent<TileBehavior>();    
                newTile.tile = map.tiles[x,y];

                newTile.location = new Vector2Int(x,y);

                tileBehaviors[new Vector2Int(x, y)] = newTile;
            }
        }

        OnMapBuilt.Invoke(map);

        // spawn heroes so that the heroes spawn next >:/ i dont think i like the idea of it being placed here!
    }

    public static TileBehavior GetTileAtPos(Vector2Int pos)
    {


        if (tileBehaviors.TryGetValue(pos, out TileBehavior tile)) return tile;
        else
        {
            Debug.Log(string.Format("Tile at {0} {1} could not be found", pos.x, pos.y));
            return null;
        }
    }
}
