using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private Vector2 distanceBetCubes;

    private Vector2 direction;
    private Vector3 targetDir;
    private Vector3 targetPos;

    /// <summary>
    /// Suscribe to movement events
    /// </summary>
    private void Start()
    {
        EventManager.instance.SuscribeToEvent("Input_W", () => { if (!CanMove()) SetDirectionW(); });
        EventManager.instance.SuscribeToEvent("Input_A", () => { if (!CanMove()) SetDirectionA(); });
        EventManager.instance.SuscribeToEvent("Input_S", () => { if (!CanMove()) SetDirectionS(); });
        EventManager.instance.SuscribeToEvent("Input_D", () => { if (!CanMove()) SetDirectionD(); });
    }

    private void Update()
    {
        if (CanMove())
        {
            Move();
        }
    }

    private void Move()
    {
        transform.Translate(targetDir * Time.deltaTime);

        //Reset on reaching the target pos
        if (Vector3.Distance(transform.position, targetPos) < 0.01f)
        {
            transform.position = targetPos;
            targetDir = Vector3.zero;
            targetPos = Vector3.zero;
            direction = Vector2.zero;
            EventManager.instance.RaiseEvent("PlayerCanWalk");
        }
    }

    // TO-DO ADD RAYCASTS FOR WALLS
    private bool CanMove()
    {
        return targetDir != Vector3.zero;
    }
    
    public void SetDirectionW()
    {
        direction = Vector2.left + Vector2.up;
        SetTargetDir();
    }
    public void SetDirectionA()
    {
        direction = Vector2.left + Vector2.down;
        SetTargetDir();
    }
    public void SetDirectionS()
    {
        direction = Vector2.right + Vector2.down;
        SetTargetDir();
    }
    public void SetDirectionD()
    {
        direction = Vector2.right + Vector2.up;
        SetTargetDir();
    }

    private void SetTargetDir()
    {
        targetDir.x = direction.x * distanceBetCubes.x;
        targetDir.y = direction.y * distanceBetCubes.y;

        targetPos = transform.position + targetDir;
        targetDir.Normalize();

        EventManager.instance.RaiseEvent("PlayerCantWalk");

    }
}
