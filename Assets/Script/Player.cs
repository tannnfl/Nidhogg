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

    //movement tools
    int direction;

    //state machine
    enum State
    {
        fist_stand, fist_jump, fist_duck,
        sword_stand, sword_jump, sword_duck,

        /*
        Idle,FistAttack,
        Duck,DuckAttack,
        SwordDuck,LungeLow,LungeMid,LungeHigh,PrepThrow,
        AttackLow,AttackMid,AttackHigh,
        SwordDuckAttack
        */
        
    }
    State currentState;
    float tState;

    //flags
    bool hasJumped = false;
    int tempTimer = 0;

    //sword
    bool isCollideWithSword;
    bool isArmed;
    bool isFence;
    bool isPrepThrow;
    int swordPos = 0;

    private void Start()
    {
        //components
        rb = GetComponent<Rigidbody2D>();
        tf = GetComponent<Transform>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        myAnim = GetComponent<Animator>();

        //S-Get player's starting position for now
        startPos = transform.position;

        currentState = State.fist_stand;
    }

    void Update()
    {
        UpdateState();
        //tState -= Time.deltaTime;
    }

    void StartState(State newState)
    {
        EndState(currentState);
        currentState = newState;
        switch (newState)
        {
            case State.fist_stand:

                break;
            case State.fist_jump:
                //animation
                if (Input.GetKey(down))
                {
                    myAnim.SetBool("isFistDucking", true);
                }
                else
                {
                    myAnim.SetBool("isFistJumping", true);
                    //this works
                }
                break;
            case State.fist_duck:
                //tState = fist attack time
                break;
            case State.sword_stand:
                //tState = fist attack time
                break;
            case State.sword_jump:
                //tState = fist attack time
                break;
            case State.sword_duck:
                //tState = fist attack time
                break;

        }
    }
    void UpdateState()
    {
        switch (currentState)
        {
            case State.fist_stand:
                Move();

                //print("fist stand state");

                if (Input.GetKeyDown(jump) && IsGrounded())
                {
                    StartState(State.fist_jump);
                }

                if (Input.GetKeyDown(attack)) 
                {
                    //call up attack function here 
                }

                if (Input.GetKey(down)) 
                {
                    StartState(State.fist_duck);
                }
                break;


            case State.fist_jump:

                tempTimer++;

                Move();

                if (!hasJumped)
                {
                    Jump();
                }

                //adjust time for animation duration
                if (hasJumped && IsGrounded() && tempTimer > 50)
                {
                        if (Input.GetKey(down) && myAnim.GetCurrentAnimatorStateInfo(0).IsName("Fist_Duck_Animation"))
                        {
                            StartState(State.fist_duck);

                        }
                        else if (myAnim.GetCurrentAnimatorStateInfo(0).IsName("Fist_Jump_Animation"))
                        {
                            myAnim.SetBool("isFistJumping", false);
                            print("change animation?");
                            StartState(State.fist_stand);
                        }
 
                }

                if (Input.GetKeyDown(attack)&&(!Input.GetKey(down)))
                {
                    //call up dive kick function
                }


                break;

            case State.fist_duck:
                Move();
                //change the sprite and collider for the player here
                myAnim.SetBool("isFistDucking", true);

                if (!Input.GetKey(down)) 
                {
                    StartState(State.fist_stand);
                }

                if (Input.GetKeyDown(jump) && IsGrounded())
                {
                    StartState(State.fist_jump);
                }

                if (Input.GetKeyDown(attack))
                {
                    //call up sweep leg function here 
                }

                if (isCollideWithSword)
                {
                    pickSword();
                    //if I'm still ducking, won't dirrectly enter lunge state
                    StartState(State.sword_duck);
                }
                break;

            case State.sword_stand:
                Move();

                if (Input.GetKeyDown(jump) && IsGrounded())
                {
                    StartState(State.sword_jump);
                }

                if (Input.GetKeyDown(up) && swordPos < 1) 
                {
                    swordPos += 1;
                }

                if (Input.GetKeyDown(down) && swordPos > -1)
                {
                    swordPos -= 1;
                }

                if (Input.GetKey(up) && swordPos == 1)
                {
                    isPrepThrow = true;
                }
                else 
                {
                    isPrepThrow = false;
                }

                if (Input.GetKeyDown(attack) && (!isPrepThrow))
                {
                    //call up sword attack function here 
                }
                else if(Input.GetKeyDown(attack) && (isPrepThrow))
                {
                    //call up sword throw function here
                }


                if (Input.GetKey(down )&& swordPos == -1)
                {
                    StartState(State.sword_duck);
                }
                break;

            case State.sword_jump:
                Move();
                Jump();

                //animation
                if (Input.GetKey(down))
                {
                    myAnim.SetBool("isSwordDucking", true);
                }
                else
                {
                    myAnim.SetBool("isSwordJumping", true);
                }

                if (Input.GetKey(up))
                {
                    isPrepThrow = true;
                }
                else 
                {
                    isPrepThrow = false;
                }

                if (Input.GetKeyDown(attack) && isPrepThrow)
                {
                    //call up sword throw function
                }

                if (IsGrounded())
                {
                    if (Input.GetKey(down))
                    {
                        StartState(State.sword_duck);
                    }
                    else
                    {
                        StartState(State.sword_stand);
                    }
                }

                break;

            case State.sword_duck:
                Move();
                if (!Input.GetKey(down))
                {
                    StartState(State.sword_stand);
                }

                if (Input.GetKeyDown(jump) && IsGrounded())
                {
                    StartState(State.sword_jump);
                }

                if (Input.GetKeyDown(attack))
                {
                    //new element-spinning sword function call up here
                }
                break;
        }
    }
    void EndState(State currentState)
    {
        switch (currentState)
        {
            case State.fist_stand:
                //tState = fist attack time
                break;
            case State.fist_jump:
                print("end:" + tempTimer);
                print("end:" + hasJumped);
                hasJumped = false;
                tempTimer = 0;
                print("ever here in the endstate?");
                break;
            case State.fist_duck:
                myAnim.SetBool("isFistDucking", false);

                break;
            case State.sword_stand:
                //tState = fist attack time
                break;
            case State.sword_jump:
                //tState = fist attack time
                break;
            case State.sword_duck:
                //tState = fist attack time
                break;

        }
    }

    //horizontal move
    private void Move()
    {

        if (Input.GetKey(left))
        {
            direction = -1;
            tf.localScale = new Vector3(-1, 1, 1);
            myAnim.SetBool("isFistRunning", true);

        }
        else if (Input.GetKey(right))
        {
            direction = 1;
            tf.localScale = new Vector3(1, 1, 1);
            myAnim.SetBool("isFistRunning", true);
        }
        else
        {
            direction = 0;
            myAnim.SetBool("isFistRunning", false);
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

        rb.velocity = new Vector2(rb.velocity.x, jumpPower);

        hasJumped = true;

        /* if (IsGrounded()&& myAnim.GetCurrentAnimatorStateInfo(0).IsName("Fist_Jump_Animation"))
          {
              myAnim.SetBool("isFistJumping", false);
          } */
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

       // StartState(State.Idle);
    }

    private void throwSword()
    {
        isArmed = false;
        //create a sword, with initial state rotation
        //...
        //StartState(State.Idle);
    }

    public bool IsGrounded()
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
