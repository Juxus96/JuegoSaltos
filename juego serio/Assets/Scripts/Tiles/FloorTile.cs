using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTile : Tile
{
    public override void SteppedIn()
    {
        if (tileState == TileState.DARK)
            EventManager.instance.RaiseEvent("PlayerInDark");
        else
            EventManager.instance.RaiseEvent("PlayerSafe");

        EventManager.instance.RaiseEvent("MovementUpdate");
    }

    public override bool Walkable(Vector2 from)
    {
        return true;
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
