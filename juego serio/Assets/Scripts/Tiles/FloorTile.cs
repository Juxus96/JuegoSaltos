using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTile : Tile
{
    public override void SteppedIn()
    {
        if(assetData == null)
        {
            EventManager.instance.RaiseEvent("MovementUpdate");
            EventManager.instance.RaiseEvent("PlayerTurn");
        }
    }

    public override Tile GetDirectionalTile(int i)
    {
        return directionalTiles[i];
    }

    public override void SetDirectionalTile(Tile tile, int dir)
    {
        directionalTiles[dir] = tile;
    }
}
