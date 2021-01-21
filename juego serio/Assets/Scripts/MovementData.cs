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

    private Transform movingTransform;

    public void Init(Transform mTransform)
    {
        movingTransform = mTransform;
        EventManager.instance.SuscribeToEvent("MovementUpdate", MovementUpdate);
    }

    private void MovementUpdate()
    {
        canMoveW = EventManager.instance.RaiseVect2Event("CheckTileMovement", movingTransform.position, Helpers.WDirection) != Vector2.up * int.MaxValue;
        canMoveA = EventManager.instance.RaiseVect2Event("CheckTileMovement", movingTransform.position, Helpers.ADirection) != Vector2.up * int.MaxValue;
        canMoveS = EventManager.instance.RaiseVect2Event("CheckTileMovement", movingTransform.position, Helpers.SDirection) != Vector2.up * int.MaxValue;
        canMoveD = EventManager.instance.RaiseVect2Event("CheckTileMovement", movingTransform.position, Helpers.DDirection) != Vector2.up * int.MaxValue;
    }

}
