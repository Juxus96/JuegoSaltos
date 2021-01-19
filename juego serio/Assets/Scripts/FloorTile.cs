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
}
