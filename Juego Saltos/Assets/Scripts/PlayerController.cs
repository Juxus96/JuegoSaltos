using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private float speed;
    [SerializeField] private float jumpLenght;
    [SerializeField] private float jumpHeight;
    [SerializeField] private float onWallGravityMul;

    [SerializeField] private float playerHeight;
    [SerializeField] private LayerMask groundLayerMask;


    private float direction;
    private bool moving;
    private bool grounded;
    private bool jumping;
    private bool canJump;
    private bool onWall;
    private float jumpForce;
    private float currentVerticalForce;
    private float normalGravity;

    private void Start()
    {
        ResetPlayer();
        normalGravity = jumpForce = jumpHeight * 4;
        
    }

    private void ResetPlayer()
    {
        moving = false;
        direction = 1;
    }

    void Update()
    {
        GroundCheck();
        if(moving)
        {
            Vector3 movDirection = Vector3.zero;
            if(Input.GetMouseButtonDown(0))
                Jump();

            if (onWall)
            {
                movDirection += Vector3.down * onWallGravityMul * normalGravity * Time.deltaTime;
            }
            else
            {
                movDirection += Vector3.right * direction * (jumping ? jumpLenght : speed) * Time.deltaTime;
                if (!grounded || jumping)
                {
                    movDirection += Vector3.up * currentVerticalForce * Time.deltaTime;
                    if (!grounded)
                        currentVerticalForce -= 2 * normalGravity * Time.deltaTime;

                    //if (!grounded && currentVerticalForce <= 0)
                    //    print(4.2 + transform.position.y);


                }

            }
            transform.Translate(movDirection);

        }
        else
        {
            if (Input.GetMouseButtonDown(0))
                moving = true;
        }


    }

    private void Jump()
    {
        if (canJump)
        {
            if (onWall)
                onWall = false;

            print(transform.position.x);

            currentVerticalForce = jumpForce;
            jumping = true;
            canJump = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Wall"))
        {
            direction = -direction;
            if(!grounded)
            {
                onWall = true;
                canJump = true;
            }
        }

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Wall"))
        {
            onWall = false;
        }
    }

    private void GroundCheck()
    {
        bool prevGrounded = grounded;
        if(Physics2D.Linecast(transform.position, transform.position - Vector3.up * playerHeight, groundLayerMask))
        {
            grounded = true;
            onWall = false;
        }
        else
        {
            grounded = false;
        }

        if(!prevGrounded && grounded)
        {
            canJump = true;
            jumping = false;
            currentVerticalForce = 0;
            print(transform.position.x);
        }

    }

    


}
