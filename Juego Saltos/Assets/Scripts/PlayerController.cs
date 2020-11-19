using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private float speed;
    [SerializeField] private float jumpingSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float playerHeight;
    [SerializeField] private float onWallGravityScale;
    [SerializeField] private LayerMask groundLayerMask;


    private float direction;
    private bool moving;
    private bool grounded;
    private bool jumping;
    private bool canJump;
    private Rigidbody2D rb2D;
    private bool onWall;

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        ResetPlayer();
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
            if(Input.GetMouseButtonDown(0))
                Jump();
        
            if(!onWall)
                transform.Translate(Vector3.right * direction * (jumping? jumpingSpeed : speed) * Time.deltaTime);
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

            rb2D.Sleep();
            rb2D.AddForce(Vector3.up * jumpForce);
            jumping = true;
            canJump = false;
            print(transform.position);
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
                rb2D.gravityScale = onWallGravityScale;
            }
        }

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Wall"))
        {
            onWall = false;
            rb2D.gravityScale = 1;
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
            print(transform.position);
            canJump = true;
            jumping = false;
        }

    }

    


}
