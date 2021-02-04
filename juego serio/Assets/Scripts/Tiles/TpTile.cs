using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TpTile : Tile
{
    public StairTile.DIRECTION tpDirection;
    [SerializeField] private TpTile otherTp;

    public override Tile GetDirectionalTile(int i)
    {
        return directionalTiles[i];
    }

    public override void SetDirectionalTile(Tile tile, int dir)
    {
        throw new System.NotImplementedException();
    }

    public override void SteppedIn()
    {
        Transform pTransform = EventManager.instance.RaiseTransformEvent("GetPlayer");
        pTransform.position = otherTp.transform.position;
        otherTp.GetOutOfTp();

    }

    private void GetOutOfTp()
    {
        EventManager.instance.RaiseEvent("MovementUpdate");
        EventManager.instance.RaiseEvent("PlayerTurn");
        if (tpDirection == StairTile.DIRECTION.LEFT)
            EventManager.instance.RaiseEvent("Input_A");
        else
            EventManager.instance.RaiseEvent("Input_S");
    }

    
}
