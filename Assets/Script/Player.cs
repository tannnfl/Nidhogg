using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] string playerSide;

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
    
    //S-Variables for checkpoint spawning and death
    Vector2 startPos;

    //components
    Rigidbody2D rb;
    Transform tf;
    SpriteRenderer spriteRenderer;
    Animator myAnim;

    //movement bools
    int direction;


    private void Start()
    {
        //components
        rb = GetComponent<Rigidbody2D>();
        tf = GetComponent<Transform>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        myAnim = GetComponent<Animator>();

        //S-Get player's starting position for now
        startPos = transform.position;


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
            tf.localScale = new Vector3(-1, 1, 1);

        }
        else if (Input.GetKey(right))
        {
            direction = 1;
            tf.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            direction = 0;

            if (playerSide == "Left")
            {
                tf.localScale = new Vector3(1, 1, 1);
            }
            else if (playerSide == "Right")
            {
                tf.localScale = new Vector3(-1, 1, 1);
            }

        }
        rb.velocity = new Vector2(direction * moveSpeed, rb.velocity.y);
    }

    private void Jump()
    {
        if (Input.GetKeyDown(jump) && IsGrounded())
        {
            myAnim.SetBool("isJumping", true);
            print("j");
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
        }

        if (IsGrounded() && myAnim.GetCurrentAnimatorStateInfo(0).IsName("Jump_Animation"))
        {
            print("stop jump");
            myAnim.SetBool("isJumping", false);
        }
    }
    
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Fallen")) 
        {
            Die();
        }
    }

    void Die() 
    {
        Respawn();
    }

    void Respawn() 
    {
        transform.position = startPos;
    }

    private void ChangeAnimation(string AnimName)
    {
        myAnim.Play(AnimName);
    }

}
