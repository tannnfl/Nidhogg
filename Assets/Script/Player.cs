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
    [SerializeField] KeyCode attack;

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

    //state machine
    enum State
    {
        Idle,FistAttack,
        Duck,DuckAttack,
        SwordDuck,LungeLow,LungeMid,LungeHigh,PrepThrow,
        AttackLow,AttackMid,AttackHigh,
        SwordDuckAttack
    }
    State currentState;
    float tState;

    //sword
    bool isCollideWithSword;
    bool isArmed;

    private void Start()
    {
        //components
        rb = GetComponent<Rigidbody2D>();
        tf = GetComponent<Transform>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        myAnim = GetComponent<Animator>();

        //S-Get player's starting position for now
        startPos = transform.position;

        currentState = State.Idle;
    }

    void Update()
    {
        UpdateState();
        tState -= Time.deltaTime;
    }

    void StartState(State newState)
    {
        EndState(currentState);
        currentState = newState;
        switch (newState)
        {
            case State.FistAttack:
                //tState = fist attack time
                break;
        }
    }
    void UpdateState()
    {
        switch (currentState)
        {
            case State.Idle:
                Move();
                Jump();
                //codes here
                //...

                if (Input.GetKeyDown(attack)) StartState(State.FistAttack);
                if (Input.GetKeyDown(down)) StartState(State.Duck);
                break;
            case State.FistAttack:
                //codes here
                //...

                if (tState <= 0) StartState(State.Idle);
                break;
            case State.Duck:
                Move();
                Jump();
                //codes here
                //...

                if (isCollideWithSword)
                {
                    pickSword();
                    //if I'm still ducking, won't dirrectly enter lunge state
                    if (!Input.GetKeyDown(down))StartState(State.LungeMid);
                }

                if (Input.GetKey(attack)) 
                {
                    //code-attack, 
                }

                if (!Input.GetKey(attack)) StartState(State.Idle);
                break;
            case State.DuckAttack:
                //codes here
                //...

                if (tState <= 0) StartState(State.Duck);
                break;
            case State.SwordDuck:
                //codes here
                //...

                if (Input.GetKey(up)) StartState(State.LungeLow);
                break;
            case State.LungeLow:
                //codes here
                //...

                if (Input.GetKey(up)) StartState(State.LungeMid);
                if (Input.GetKey(down)) StartState(State.SwordDuck);
                if (Input.GetKey(attack)) StartState(State.AttackLow);
                break;
            case State.LungeMid:
                //codes here
                //...

                if (Input.GetKey(up)) StartState(State.LungeHigh);
                if (Input.GetKey(down)) StartState(State.LungeLow);
                if (Input.GetKey(attack)) StartState(State.AttackMid);
                break;
            case State.LungeHigh:
                //codes here
                //...

                if (Input.GetKey(up)) StartState(State.PrepThrow);
                if (Input.GetKey(down)) StartState(State.LungeMid);
                if (Input.GetKey(attack)) StartState(State.AttackHigh);
                break;
            case State.PrepThrow:
                //codes here
                //...

                if (Input.GetKey(down)) StartState(State.LungeHigh);
                if (Input.GetKey(attack)) throwSword ();
                break;
            case State.SwordDuckAttack:
                //codes here
                //...

                break;

        }
    }
    void EndState(State currentState)
    {
        switch (currentState)
        {

        }
    }

    //horizontal move
    private void Move()
    {

        if (Input.GetKey(left))
        {
            direction = -1;
            tf.localScale = new Vector3(-1, 1, 1);
            myAnim.SetBool("isRunning", true);

        }
        else if (Input.GetKey(right))
        {
            direction = 1;
            tf.localScale = new Vector3(1, 1, 1);
            myAnim.SetBool("isRunning", true);
        }
        else
        {
            direction = 0;
            myAnim.SetBool("isRunning", false);
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
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
        }

        if (IsGrounded() && myAnim.GetCurrentAnimatorStateInfo(0).IsName("Jump_Animation"))
        {
            myAnim.SetBool("isJumping", false);
        }
    }

    private void pickSword()
    {
        isArmed = true;
        //code here
        //...

        //StartState(State.LungeMid);
    }

    private void disArmed()
    {
        isArmed = false;
        //create a sword, with initial state drop
        //...

        StartState(State.Idle);
    }

    private void throwSword()
    {
        isArmed = false;
        //create a sword, with initial state rotation
        //...

        StartState(State.Idle);
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
        if (collision.CompareTag("sword"))
        {
            isCollideWithSword = true;
        }
        else
        {
            isCollideWithSword = false;
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
