using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private Vector3 distanceBetCubes;
    [SerializeField] private float speed;

    private Vector2 direction;
    private Vector3 targetDir;
    private Vector3 targetPos;
    private bool moving;



    private void Update()
    {
        if (!moving)
        {
            direction = Vector2.zero;

            if(Input.GetKeyDown(KeyCode.W))
            {
                SetDirectionW();
            }
            else if(Input.GetKeyDown(KeyCode.A))
            {
                SetDirectionA();
            }
            else if(Input.GetKeyDown(KeyCode.S))
            {
                SetDirectionS();
            }
            else if(Input.GetKeyDown(KeyCode.D))
            {
                SetDirectionD();
            }

            if (direction != Vector2.zero)
            {
                moving = true;
                targetDir.x = direction.x * distanceBetCubes.x; 
                targetDir.y = direction.y * distanceBetCubes.y; 
                targetDir.z = direction.y * distanceBetCubes.z;
                targetPos = transform.position + targetDir;
                targetDir.Normalize();
            }
        }
        if (targetDir != Vector3.zero)
            Move();
}

    private void Move()
    {
        transform.Translate( targetDir * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPos) < 0.01f)
        {
            transform.position = targetPos;
            targetDir = Vector3.zero;
            moving = false;
        }
    }
    
    public void SetDirectionW()
    {
        direction = Vector2.left + Vector2.up;
    }
    public void SetDirectionA()
    {
        direction = Vector2.left + Vector2.down;
    }
    public void SetDirectionS()
    {
        direction = Vector2.right + Vector2.down;
    }
    public void SetDirectionD()
    {
        direction = Vector2.right + Vector2.up; 
    }
}
