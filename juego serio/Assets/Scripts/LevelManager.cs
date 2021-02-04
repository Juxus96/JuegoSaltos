using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Vector2 startPos;
    [SerializeField] private Transform player;
    [SerializeField] private Transform lightSource;


    private void Start()
    {
        Invoke("ResetLevel",0.1f);
        EventManager.instance.SuscribeToEvent("PlayerDied", ResetLevel);
        
    }

    private void ResetLevel()
    {
        lightSource.position = startPos;
        player.position = startPos;
        EventManager.instance.RaiseEvent("UpdateLight");
        EventManager.instance.RaiseEvent("MovementUpdate");
        EventManager.instance.RaiseEvent("PlayerTurn");

    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.X))
        //{
        //    enemyTurn = false;
        //    playerMoving = true;
        //    EventManager.instance.RaiseEvent("PlayerTurn");
        //}

        //if(Input.GetKeyDown(KeyCode.M) && !enemyTurn)
        //{
        //    if (playerMoving)
        //    {
        //        EventManager.instance.RaiseEvent("LightCanMove");
        //        playerMoving = false;
        //    }
        //    else
        //    {
        //        EventManager.instance.RaiseEvent("PlayerTurn");
        //        playerMoving = true;
        //    }
        //}
    }
}
