using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairTile : Tile
{
    public enum DIRECTION
    {
        LEFT,
        RIGHT
    }
    public DIRECTION stairDirection;

    private bool recentlyUsed;

    private void Start()
    {
        EventManager.instance.SuscribeToEvent("StairUsed", () => recentlyUsed = false); ;
    }

    public override Tile GetDirectionalTile(int i)
    {
        return directionalTiles[i];
    }

    public override void SetDirectionalTile(Tile tile, int dir)
    {
        directionalTiles[dir] = tile;
    }

    public override void SteppedIn()
    {
        EventManager.instance.RaiseEvent("StairUsed");
        recentlyUsed = true;
    }

    public new void SetState(TileState newState)
    {
        base.SetState(recentlyUsed ? TileState.LIGHT : newState);
    }

}
