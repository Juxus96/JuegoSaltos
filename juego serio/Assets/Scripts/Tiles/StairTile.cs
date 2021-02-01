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

    private bool recentlyUsed;

    private void Start()
    {
        EventManager.instance.SuscribeToEvent("StairUsed", () => recentlyUsed = false); ;
    }

    public DIRECTION stairDirection;
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

    public override bool Walkable(Vector2 from)
    {
        throw new System.NotImplementedException();
    }

    public new void SetState(TileState newState)
    {
        base.SetState(recentlyUsed ? TileState.LIGHT : newState);
    }

}
