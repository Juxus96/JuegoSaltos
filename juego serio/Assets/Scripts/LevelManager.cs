using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Vector2 startPos;
    [SerializeField] private Transform player;
    [SerializeField] private Transform lightSource;

    private bool playerMoving;
    private bool enemyTurn;

    private void Start()
    {
        playerMoving = true;
        enemyTurn = false;
        Invoke("ResetLevel",0.1f);
        EventManager.instance.SuscribeToEvent("PlayerDied", ResetLevel);
        EventManager.instance.SuscribeToEvent("PlayerAction", () => { enemyTurn = true; });
        
    }

    private void ResetLevel()
    {
        lightSource.position = startPos;
        player.position = startPos;
        EventManager.instance.RaiseEvent("UpdateLight");

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            enemyTurn = false;
            playerMoving = true;
            EventManager.instance.RaiseEvent("PlayerTurn");
        }

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
