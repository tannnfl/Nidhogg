using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Control")]
    [SerializeField] KeyCode left;
    [SerializeField] KeyCode right;
    [SerializeField] KeyCode up;
    [SerializeField] KeyCode down;
    [SerializeField] KeyCode jump;
    [SerializeField] KeyCode lunge;

    [Header("Movement Tuning")]
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpPower;

    [Header("ground check components")]
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;

    //components
    Rigidbody2D rb;
    Transform tf;

    //movement bools
    int direction;

    private void Start()
    {
        //components
        rb = GetComponent<Rigidbody2D>();
        tf = GetComponent<Transform>();
    }

    void Update()
    {

        //control
        Move();
        Jump();

    }

    //horizontal move
    private void Move()
    {
        if (Input.GetKey(left))
        {
            direction = -1;

        }
        else if (Input.GetKey(right))
        {
            direction = 1;
        }
        else
        {
            direction = 0;
        }
        rb.velocity = new Vector2(direction * moveSpeed, rb.velocity.y);
    }
    private void Jump()
    {
        if (Input.GetKeyDown(up) && IsGrounded())
        {
            print("j");
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
        }
    }
    
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }
}
