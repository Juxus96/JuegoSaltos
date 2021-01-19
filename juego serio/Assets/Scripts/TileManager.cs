using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    private List<FloorTile> allTiles;
    private List<FloorTile> tilesToUpdate;
    private List<FloorTile> oldTiles;

    [SerializeField] private Vector2 offesetBetTiles;
    private void Start()
    {
        allTiles = new List<FloorTile>();
        tilesToUpdate = new List<FloorTile>();
        oldTiles = new List<FloorTile>();
        EventManager.instance.SuscribeToEvent("PlayerMoved", CheckTile);
        EventManager.instance.SuscribeToEvent("UpdateLight", UpdateLight);
        EventManager.instance.SuscribeToEvent("LightMoved", PointLightMoved);
        EventManager.instance.SuscribeToFuncEvent("CheckMove", CheckMove);
        EventManager.instance.SuscribeToFuncEvent("CheckStairs", CheckStairs);

        for (int i = 0; i < transform.childCount; i++)
        {
            allTiles.Add(transform.GetChild(i).GetComponent<FloorTile>());
        }
    }


    private void CheckTile(Vector2 position)
    {
        GetTileByPos(position).DoAction();
    }


    private FloorTile GetTileByPos(Vector2 pos)
    {
        int i = 0;
        for (; i < allTiles.Count && Vector2.Distance(allTiles[i].transform.position, pos) > 0.01f; i++) ;
        return i < allTiles.Count ? allTiles[i] : null;
       
    }

    private void UpdateTiles()
    {
        foreach(FloorTile ft in oldTiles)
        {
            ft.SetState(FloorTile.TileState.DARK);
        }

        foreach (FloorTile ft in tilesToUpdate)
        {
            ft.SetState(FloorTile.TileState.LIGHT);
        }
    }

    private void PointLightMoved(Vector2 position, int radius)
    {
        tilesToUpdate.Add(GetTileByPos(position));
        for (int i = 0; i < radius; i++)
        {
            for (int j = radius - i; j > 0 ; j--)
            {
                FloorTile wDirectionTile = GetTileByPos(position + Vector2.up * offesetBetTiles * (j + i) - Vector2.right * offesetBetTiles * (j - i));
                FloorTile aDirectionTile = GetTileByPos(position - Vector2.up * offesetBetTiles * (j + i) + Vector2.right * offesetBetTiles * (j - i));
                FloorTile sDirectionTile = GetTileByPos(position - Vector2.up * offesetBetTiles * (j - i) - Vector2.right * offesetBetTiles * (j + i));
                FloorTile dDirectionTile = GetTileByPos(position + Vector2.up * offesetBetTiles * (j - i) + Vector2.right * offesetBetTiles * (j + i));

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
        oldTiles = new List<FloorTile>(tilesToUpdate);
        tilesToUpdate.Clear();
        EventManager.instance.RaiseEvent("GetLights");
    }

    private bool CheckMove(Vector2 position)
    {
        return GetTileByPos(position) != null;
    }
    
    private bool CheckStairs(Vector2 position)
    {
        FloorTile ft = GetTileByPos(position);
        return ft != null && ft.stairs;
    }


}
