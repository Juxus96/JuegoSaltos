using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
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
        EventManager.instance.SuscribeToVect2Event("CheckTileMovement", CheckTileToMove);
        EventManager.instance.SuscribeToVect2Event("GetTilePos", GetTIlePos);

        for (int i = 0; i < transform.childCount; i++)
        {
            allTiles.Add(transform.GetChild(i).GetComponent<Tile>());
        }
    }

    private Vector2 GetTIlePos(Vector2 tilePos)
    {
        return Vector2.zero;
    }

    // Changes the boolean in the movement data
    private Vector2 CheckTileToMove(Vector2 position, Vector2 direction)
    {
        Tile tile = GetTileByPos(position);
        int i = 0;
        for (; i < Helpers.Directions.Length && !(tile.directionalTiles[i] != null && direction == Helpers.Directions[i]); i++) ;
        return i < Helpers.Directions.Length ? (Vector2)tile.directionalTiles[i].transform.position : Vector2.up * int.MaxValue; 
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
        Tile baseTile = GetTileByPos(position);
        tilesToUpdate.Add(baseTile);
        AddLightTile(baseTile, radius);

        UpdateTiles();
    }

    private void AddLightTile(Tile baseTile, int radius)
    {
        if(radius > 0 && baseTile != null)
        {
            radius--;

            Tile[] directionalTiles = new Tile[Helpers.Directions.Length];
            for (int i = 0; i < directionalTiles.Length; i++)
            {
                directionalTiles[i] = baseTile.directionalTiles[i];
                if (directionalTiles[i] != null && baseTile.layer == directionalTiles[i].layer)
                {
                    if (!tilesToUpdate.Contains(directionalTiles[i]))
                        tilesToUpdate.Add(directionalTiles[i]);
                    AddLightTile(directionalTiles[i], radius);
                }
            }

        }
    }

    private void UpdateLight()
    {
        oldTiles = new List<Tile>(tilesToUpdate);
        tilesToUpdate.Clear();
        EventManager.instance.RaiseEvent("GetLights");
    }

    


}
