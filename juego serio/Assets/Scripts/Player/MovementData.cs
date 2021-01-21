using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PlayerData")]
public class MovementData : ScriptableObject
{

    [HideInInspector] public bool[] canMove;
    private Transform movingTransform;

    public void Init(Transform mTransform)
    {
        canMove = new bool[Helpers.Directions.Length];
        movingTransform = mTransform;
        EventManager.instance.SuscribeToEvent("MovementUpdate", MovementUpdate);
    }

    private void MovementUpdate()
    {
        for (int i = 0; i < Helpers.Directions.Length; i++)
        {
            canMove[i] = EventManager.instance.RaiseVect2Event("CheckTileMovement", movingTransform.position, Helpers.Directions[i]) != Vector2.up * int.MaxValue;
        }
    }

}
