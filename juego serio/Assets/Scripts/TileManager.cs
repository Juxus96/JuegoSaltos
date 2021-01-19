using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    [SerializeField] private float offsetBetLayers;
    [SerializeField] private Vector2 GridSize;

    private List<Tile> allTiles;
    private List<Tile> tilesToUpdate;
    private List<Tile> oldTiles;

    private void Start()
    {
        allTiles = new List<Tile>();
        tilesToUpdate = new List<Tile>();
        oldTiles = new List<Tile>();

        EventManager.instance.SuscribeToEvent("UpdateLight", UpdateLight);
        EventManager.instance.SuscribeToEvent("LightMoved", PointLightMoved);
        EventManager.instance.SuscribeToBoolEvent("CheckTileMovement", CheckTileToMove);
        EventManager.instance.SuscribeToVect2Event("GetTilePos", GetTIlePos);

        for (int i = 0; i < transform.childCount; i++)
        {
            allTiles.Add(Instantiate(new GameObject("Tile")).GetComponent<Tile>());
        }
    }

    private Vector2 GetTIlePos(Vector2 tilePos)
    {
        return Vector2.zero;
    }

    // Changes the boolean in the movement data
    private bool CheckTileToMove(Vector2 position, Vector2 direction)
    {
        Tile tile = GetTileByPos(position + direction);
        Tile stair = GetTileByPos(position + direction + Vector2.up * offsetBetLayers);

        return tile != null && tile.Walkable(position) || stair != null && stair.GetType() == typeof(StairTile) && stair.Walkable(position);
    }


    private Tile GetTileByPos(Vector2 pos)
    {
        int i = 0;
        for (; i < allTiles.Count && Vector2.Distance(allTiles[i].transform.position, pos) > 0.01f; i++) ;
        return i < allTiles.Count ? allTiles[i] : null;
       
    }

    private void UpdateTiles()
    {
        foreach(Tile ft in oldTiles)
        {
            ft.SetState(Tile.TileState.DARK);
        }

        foreach (Tile ft in tilesToUpdate)
        {
            ft.SetState(Tile.TileState.LIGHT);
        }
    }

    private void PointLightMoved(Vector2 position, int radius)
    {
        tilesToUpdate.Add(GetTileByPos(position));
        for (int i = 0; i < radius; i++)
        {
            for (int j = radius - i; j > 0 ; j--)
            {
                Tile wDirectionTile = GetTileByPos(position + Vector2.up * MovementData.OffsetBetTiles.y * (j + i) - Vector2.right * MovementData.OffsetBetTiles.x * (j - i));
                Tile aDirectionTile = GetTileByPos(position - Vector2.up * MovementData.OffsetBetTiles.y * (j + i) + Vector2.right * MovementData.OffsetBetTiles.x * (j - i));
                Tile sDirectionTile = GetTileByPos(position - Vector2.up * MovementData.OffsetBetTiles.y * (j - i) - Vector2.right * MovementData.OffsetBetTiles.x * (j + i));
                Tile dDirectionTile = GetTileByPos(position + Vector2.up * MovementData.OffsetBetTiles.y * (j - i) + Vector2.right * MovementData.OffsetBetTiles.x * (j + i));

                if (wDirectionTile != null)
                    tilesToUpdate.Add(wDirectionTile);
                if (aDirectionTile != null)
                    tilesToUpdate.Add(aDirectionTile);
                if (sDirectionTile != null)
                    tilesToUpdate.Add(sDirectionTile);
                if (dDirectionTile != null)
                    tilesToUpdate.Add(dDirectionTile);
            }
        }

        UpdateTiles();
    }

    private void UpdateLight()
    {
        oldTiles = new List<Tile>(tilesToUpdate);
        tilesToUpdate.Clear();
        EventManager.instance.RaiseEvent("GetLights");
    }

    


}
