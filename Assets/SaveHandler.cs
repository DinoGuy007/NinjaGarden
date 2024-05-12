using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class SaveHandler : MonoBehaviour
{
    public SaveHandler saveInstance;

    Dictionary<string, Tilemap> tilemaps = new Dictionary<string, Tilemap>();
    [SerializeField] BoundsInt bounds;
    [SerializeField] string filename = "tilemapData.json";

    private void Start()
    {
        initTilemaps();
    }

    private void initTilemaps()
    {
        //get all tilemaps
        //write to dictionary
        Tilemap[] maps = FindObjectsOfType<Tilemap>();

        foreach(var map in maps)
        {
            tilemaps.Add(map.name, map);
        }

    }

    public void OnSave()
    {
        Debug.Log("OnSave has been called");

        List<TilemapData> data = new List<TilemapData>();

        //foreach existing tilemap
        foreach (var mapObj in tilemaps)
        {
            TilemapData mapData = new TilemapData();
            mapData.key = mapObj.Key;

            for(int x = bounds.xMin; x <bounds.xMax; x++)
            {
                for (int y = bounds.yMin; y < bounds.yMax; y++)
                {
                    Vector3Int pos = new Vector3Int(x, y, 0);
                    TileBase tile = mapObj.Value.GetTile(pos);

                    if (tile != null) {
                        TileInfo ti = new TileInfo(tile, pos);
                        mapData.tiles.Add(ti);
                    }
                }
            }
            data.Add(mapData);
        }
    }

    public void OnLoad()
    {
        Debug.Log("OnLoad has been called");
    }

}

[Serializable]
public class TilemapData
{
    public string key; //the key of the dictionary
    public List<TileInfo> tiles = new List<TileInfo>();
}

[Serializable]
public class TileInfo
{
    public TileBase tile;
    public Vector3Int position;

    public TileInfo(TileBase tile, Vector3Int pos)
    {
        this.tile = tile;
        position = pos;
    }
}