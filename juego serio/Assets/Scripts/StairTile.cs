using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairTile : Tile
{
    [SerializeField] private Vector2[] directions;

    public override void SteppedIn()
    {

    }

    public override bool Walkable(Vector2 from)
    {
        int i = 0;
        for (; i < directions.Length && directions[i] != from; i++) ;
        return i < directions.Length; 
    }

}
