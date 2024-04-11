using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    TileBase empty;
    [SerializeField] Tilemap sandTilemap;
    [SerializeField] TileBase sand;
    [SerializeField] Camera cam;
    [SerializeField] GameObject rock;
    [SerializeField] LayerMask tileContents;
    public ItemManager itemManager;

    [SerializeField] float overlapSize;
    [SerializeField] int dropChance;

    [SerializeField] GameObject enemy;

    public void Unearth(Vector3Int position)
    {
        sandTilemap.SetTile(position, empty);
        GetTileDrop(position);
    }

    private void GetTileDrop(Vector3Int position)
    {
        int dropNumber = Random.Range(1, 101);
        if (dropNumber > 0 && dropNumber < dropChance)
        {
            Instantiate(itemManager.TileDrop(), new Vector3(position.x, position.y, -5), Quaternion.identity);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Instantiate(enemy, new Vector3(0, 0, -5), Quaternion.identity);
        }
    }

    public string CheckTile(Vector3Int position)
    {
        TileBase tile = sandTilemap.GetTile(position);
        Collider2D[] allCollidersInTile = Physics2D.OverlapCircleAll((Vector2Int)position, overlapSize, tileContents);

        if (tile == sand)
        {
            return "sand";
        }
        else 
        {
            foreach (Collider2D collider in allCollidersInTile)
            {
                if (collider.gameObject.tag == "Rock")
                {
                    return "rock";
                }
            }

            foreach (Collider2D collider in allCollidersInTile)
            {
                if (collider.gameObject.tag == "Trap")
                {
                    return "trap";
                }
            }

            return "null";
        }
    }
}