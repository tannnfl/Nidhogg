using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] string playerSide;

    [SerializeField] Color playerColor;

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

    //action flags & timers
    bool hasJumped = false;
    int tempTimer = 0;
    bool isDivekicking = false;
    int swordTempTimer = 0;

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

        isArmed = false;
    }

    void Update()
    {
        //tState -= Time.deltaTime;
        UpdateState();
    

        //Animator setup, if need different colored sword animations for different players
         
          if (playerSide == "Left")
        {
            if (!isArmed)
            {
                spriteRenderer.color = playerColor;             
            }

            if (isArmed)
            {
                spriteRenderer.color = Color.white;
            }
        }
        if (playerSide == "Right")
        {
            if (!isArmed)
            {
                spriteRenderer.color = playerColor;
            }

            if (isArmed)
            {
                spriteRenderer.color = Color.white;
            }
        }
        



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
                    
                }
                break;
            case State.fist_duck:
                
                break;
            case State.sword_stand:
                break;
            case State.sword_jump:
                //animation
                if (Input.GetKey(down))
                {
                    myAnim.SetBool("isSwordDucking", true);
                }
                else if (Input.GetKey(up)) //prepthrow
                {
                    myAnim.SetInteger("swordPos", 2);
                }
                else
                {
                    myAnim.SetBool("isSwordJumping", true);
                }
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
                    FistAttack();
                }

                if (Input.GetKeyUp(attack))
                {
                    myAnim.SetBool("isFistAttacking", false);
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

                //divekick
                if (hasJumped && Input.GetKeyDown(attack) && (!Input.GetKey(down)))
                {
                        Divekick();
                        isDivekicking = true;
                }

                //divekick landing
                if (isDivekicking && hasJumped && IsGrounded() && tempTimer > 50)
                {
                    StartState(State.fist_stand);
                }


                //normal landing, remember to adjust time for animation duration
                if (!isDivekicking && hasJumped && IsGrounded() && tempTimer > 50)
                {
                        if (Input.GetKey(down) && myAnim.GetCurrentAnimatorStateInfo(0).IsName("Fist_Duck_Animation"))
                        {
                            StartState(State.fist_duck);

                        }
                        else if (myAnim.GetCurrentAnimatorStateInfo(0).IsName("Fist_Jump_Animation"))
                        {
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
                    print("yes");
                    Legsweep();
                }

                if (Input.GetKeyUp(attack))
                {
                    myAnim.SetBool("isFistLegsweeping", false);
                }

                if (isCollideWithSword && myAnim.GetCurrentAnimatorStateInfo(0).IsName("Fist_Duck_Animation"))
                {
                    pickSword();
                    //if I'm still ducking, won't dirrectly enter lunge state
                    StartState(State.sword_duck);
                }
                break;

            case State.sword_stand:
                Move();
                SwordAction();
                print(swordTempTimer);

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

                //animation switch
                switch (swordPos)
                {
                    case -1:
                        myAnim.SetInteger("swordPos", -1);

                        swordTempTimer++;

                        if (Input.GetKey(down) && swordTempTimer > 60)
                        {
                            swordTempTimer = 0;
                            StartState(State.sword_duck);
                        }

                        break;

                    case 0:
                        swordTempTimer = 0;
                        myAnim.SetInteger("swordPos", 0);
                        break;

                    case 1:
                        myAnim.SetInteger("swordPos", 1);

                        swordTempTimer++;

                        if(swordTempTimer > 60)
                        {
                            if (Input.GetKey(up))
                            {
                                isPrepThrow = true;
                                myAnim.SetInteger("swordPos", 2);
                                //swordTempTimer = 0;
                            }
                            else
                            {
                                isPrepThrow = false;
                            }
                        }
   
                        break;
                }

                if (Input.GetKeyDown(attack) && !isPrepThrow)
                {
                    //call attack function
                    if (Input.GetKeyDown(attack))
                    {
                        myAnim.SetBool("isSwordAttacking", true);
                    }
                    if (Input.GetKeyUp(attack))
                    {
                        myAnim.SetBool("isSwordAttacking", false);
                    }
                } 

                if (isPrepThrow && Input.GetKeyDown(attack))
                {
                        myAnim.SetBool("isSwordAttacking", true);
                        //call throw sword function
                        disArmed();
                        StartState(State.fist_stand);
       
                    if (Input.GetKeyUp(attack))
                    {
                        myAnim.SetBool("isSwordAttacking", false);
                    }
                }

                break;

            case State.sword_jump:

                tempTimer++;

                Move();

                if (!hasJumped)
                {
                    Jump();
                }

                //throw sword

                if (Input.GetKey(up))
                {
                    isPrepThrow = true;
                }
                else
                {
                    isPrepThrow = false;
                }

                if (isPrepThrow)
                {
                    if (Input.GetKeyDown(attack))
                    {
                        myAnim.SetBool("isSwordAttacking", true);
                        //call throw sword function
                    }

                    if(Input.GetKeyUp(attack))
                    {
                        myAnim.SetBool("isSwordAttacking", false);
                        disArmed();
                        isPrepThrow = false;
                        StartState(State.fist_stand);
                    }  
                }
  


                //divekick
                if (!isPrepThrow && hasJumped && Input.GetKeyDown(attack) && (!Input.GetKey(down)))
                {
                    Divekick();
                    isDivekicking = true;
                }

                //divekick or prepthrow landing
                if ((isDivekicking || isPrepThrow) && hasJumped && IsGrounded() && tempTimer > 50)
                {
                    StartState(State.sword_stand);
                }


                //normal landing, remember to adjust time for animation duration
                if (!isDivekicking && hasJumped && IsGrounded() && tempTimer > 50)
                {
                    if (Input.GetKey(down) && myAnim.GetCurrentAnimatorStateInfo(0).IsName("Sword_Duck_Animation"))
                    {
                        StartState(State.sword_duck);

                    }
                    else if (myAnim.GetCurrentAnimatorStateInfo(0).IsName("Sword_Jump_Animation"))
                    {
                        StartState(State.sword_stand);
                    }

                }


                break;

            case State.sword_duck:
                Move();

                myAnim.SetBool("isSwordDucking", true);

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
                    Legsweep();
                }

                if (Input.GetKeyUp(attack))
                {
                    myAnim.SetBool("isSwordLegsweeping", false);
                }


                if (Input.GetKeyDown(attack))//&& some condition, like having already duck jumped once
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
                hasJumped = false;
                tempTimer = 0;
                isDivekicking = false;
                myAnim.SetBool("isFistDivekicking", false);
                myAnim.SetBool("isFistJumping", false);
                break;
            case State.fist_duck:
                myAnim.SetBool("isFistDucking", false);
                break;
            case State.sword_stand:
                //tState = fist attack time
                break;
            case State.sword_jump:
                hasJumped = false;
                tempTimer = 0;
                isDivekicking = false;
                myAnim.SetBool("isSwordDivekicking", false);
                myAnim.SetBool("isSwordJumping", false);
                swordPos = 0;
                break;
            case State.sword_duck:
                myAnim.SetBool("isSwordDucking", false);

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

    private void FistAttack()
    {
        myAnim.SetBool("isFistAttacking", true);

        //fist attack/punch code here
    }

    private void SwordAction()
    {
        switch (swordPos)
        {
            case -1:
                myAnim.SetInteger("swordPos", -1);

                if (Input.GetKeyDown(attack))
                {
                    myAnim.SetBool("isSwordAttacking", true);
                    //code for sword action and hitboxes at different height
                }
                if (Input.GetKeyUp(attack))
                {
                    myAnim.SetBool("isSwordAttacking", false);
                }

                break;

            case 0:
                myAnim.SetInteger("swordPos", 0);

                if (Input.GetKeyDown(attack))
                {
                    myAnim.SetBool("isSwordAttacking", true);
                    //code for sword action and hitboxes at different height
                }
                if (Input.GetKeyUp(attack))
                {
                    myAnim.SetBool("isSwordAttacking", false);
                }


                break;

            case 1:
                myAnim.SetInteger("swordPos", 1);

                if (Input.GetKeyDown(attack))
                {
                    myAnim.SetBool("isSwordAttacking", true);
                    //code for sword action and hitboxes at different height
                }
                if (Input.GetKeyUp(attack))
                {
                    myAnim.SetBool("isSwordAttacking", false);
                }

                break;
        }

        if (isPrepThrow) //prepping throw sword
        {
            myAnim.SetInteger("swordPos", 2);

            if (Input.GetKeyDown(attack)) //throwing sword
            {
                myAnim.SetBool("isSwordAttacking", true);
                disArmed();
            }
            if (Input.GetKeyUp(attack))
            {
                myAnim.SetBool("isSwordAttacking", false);
            }
        }

    }

    private void Divekick()
    {
        if (!isArmed)
        {
            myAnim.SetBool("isFistDivekicking", true);
        }

        else if (isArmed)
        {
            myAnim.SetBool("isSwordDivekicking", true);
        }

        //divekick attack and movement code here
    }

    private void Legsweep()
    {
        if (!isArmed)
        {
            myAnim.SetBool("isFistLegsweeping", true);
        }

        else if (isArmed)
        {
            myAnim.SetBool("isSwordLegsweeping", true);
        }
    }


    private void pickSword()
    {
        isArmed = true;
        myAnim.SetBool("isArmed", true);
        //code here
        //...

        //StartState(State.LungeMid);
    }

    private void disArmed()
    {
        isArmed = false;
        myAnim.SetBool("isArmed", false);

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
