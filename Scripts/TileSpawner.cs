using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileSpawner : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Tilemap baseTilemap;
    [SerializeField] TileBase baseTile;
    [SerializeField] Tilemap sandTilemap;
    [SerializeField] TileBase sandTile;
    [SerializeField] int tileSpawnRangeX;
    [SerializeField] int tileSpawnRangeY;

    private void Start()
    {
        SpawnTiles();
    }

    public void SpawnTiles()
    {
        for (int x = -tileSpawnRangeX; x < tileSpawnRangeX; x++)
        {
            for (int y = -tileSpawnRangeY; y < tileSpawnRangeY; y++)
            {
                Vector3Int targetTilePosition = Vector3Int.FloorToInt(new Vector3(player.position.x + x, player.position.y + y, 0));
                if (baseTilemap.GetTile(targetTilePosition) != baseTile)
                {
                    baseTilemap.SetTile(targetTilePosition, baseTile);
                    sandTilemap.SetTile(targetTilePosition, sandTile);
                }
            }
        }
    }
}