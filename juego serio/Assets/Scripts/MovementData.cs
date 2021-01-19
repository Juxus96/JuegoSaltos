using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerData")]
public class MovementData : ScriptableObject
{
    //public bool canMoveW;
    //public bool canMoveA;
    //public bool canMoveS;
    //public bool canMoveD;


    [HideInInspector] public bool canMoveW;
    [HideInInspector] public bool canMoveA;
    public bool canMoveS;
    [HideInInspector] public bool canMoveD;

    public readonly static Vector2 OffsetBetTiles = new Vector2(0.6f, 0.3f);
    public readonly static Vector2 WDirection = new Vector2(-1,  1);
    public readonly static Vector2 ADirection = new Vector2(-1, -1);
    public readonly static Vector2 SDirection = new Vector2( 1, -1);
    public readonly static Vector2 DDirection = new Vector2( 1,  1);
    private Transform movingTransform;

    public void Init(Transform mTransform)
    {
        movingTransform = mTransform;
        EventManager.instance.SuscribeToEvent("MovementUpdate", MovementUpdate);
    }

    private void MovementUpdate()
    {
        canMoveW = EventManager.instance.RaiseBoolEvent("CheckTileMovement", movingTransform.position, WDirection * OffsetBetTiles);
        canMoveA = EventManager.instance.RaiseBoolEvent("CheckTileMovement", movingTransform.position, ADirection * OffsetBetTiles);
        canMoveS = EventManager.instance.RaiseBoolEvent("CheckTileMovement", movingTransform.position, SDirection * OffsetBetTiles);
        canMoveD = EventManager.instance.RaiseBoolEvent("CheckTileMovement", movingTransform.position, DDirection * OffsetBetTiles);
    }

}
