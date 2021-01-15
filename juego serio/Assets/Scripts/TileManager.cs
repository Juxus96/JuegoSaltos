using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    private List<FloorTile> tiles;

    [SerializeField] private Vector2 distanceBetTiles;
    private void Start()
    {
        tiles = new List<FloorTile>();
        EventManager.instance.SuscribeToEvent("PlayerMoved", CheckTile);
        EventManager.instance.SuscribeToEvent("LightMoved", UpdateTiles);

        for (int i = 0; i < transform.childCount; i++)
        {
            tiles.Add(transform.GetChild(i).GetComponent<FloorTile>());
        }
        
    }


    private void CheckTile(Vector2 position)
    {
        GetTileByPos(position).DoAction();
    }

    private FloorTile GetTileByPos(Vector2 pos)
    {
        int i = 0;
        for (; i < tiles.Count && Vector2.Distance(tiles[i].transform.position, pos) > 0.01f; i++) ;
        return tiles[i];
       
    }

    private void UpdateTiles(Vector2 position, int radius)
    {
        for (int i = 0; i < tiles.Count; i++)
        {
            if(Mathf.Abs(position.x - tiles[i].transform.position.x) - distanceBetTiles.x * radius <= 0.1f
                && Mathf.Abs(position.y -tiles[i].transform.position.y) - distanceBetTiles.y * radius <= 0.1f)
            {
                tiles[i].SetState(FloorTile.TileState.LIGHT);

            }
            else
            {
                tiles[i].SetState(FloorTile.TileState.DARK);
            }

        }
    }


}
