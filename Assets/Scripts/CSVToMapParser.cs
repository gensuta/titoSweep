using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CSVToMapParser : MonoBehaviour
{
    [SerializeField] private TextAsset csv;
    public Map parsedMap = new Map();
    public UnityEvent<Map> MapParsed;

    private void Start()
    {
        ParseCSV();
    }

    public void ParseCSV()
    {
        string[] rows = csv.text.Split('\n');

        parsedMap.height = rows.Length;

        for (int x = 0; x < rows.Length; x++)
        {
            string[] columns = rows[x].Split(',');

            if (parsedMap.width == 0) { parsedMap.width = columns.Length; parsedMap.tiles = new Tile[rows.Length, columns.Length]; }

            for (int y = 0; y < columns.Length; y++)
            {
                //TODO: Come back to this! ^0^

                if (columns[y].Contains("G")) { parsedMap.tiles[x, y] = new Tile(GroundType.Grass, true); }

                if (columns[y].Equals("W")) { parsedMap.tiles[x, y] = new Tile(GroundType.Wall, false); }

                if (columns[y].Equals("Wa")) { parsedMap.tiles[x, y] = new Tile(GroundType.Water, true, 2); }

                if (columns[y].Equals("M")) { parsedMap.tiles[x, y] = new Tile(GroundType.Mountains, false,2); }
            }
        }

        //fire this off when map parsing complete
        MapParsed.Invoke(parsedMap);
    }
}
