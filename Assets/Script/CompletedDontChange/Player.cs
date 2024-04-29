using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{

    [SerializeField] string playerSide;
    public static event Action<int> OnSwordPosChanged;

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
    [SerializeField] float moveSpeedDuck;
    [SerializeField] float jumpPower;
    [SerializeField] float jumpPowerDuck;

    [Header("ground check components")]
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;

    //S-Variables for checkpoint spawning and death
    Vector2 startPos;
    public bool hasDied;
    public bool canRespawn;
    public bool notInMap;

    //components
    Rigidbody2D rb;
    Transform tf;
    SpriteRenderer spriteRenderer;
    Animator myAnim;
    Camera cam;

    //movement tools
    int direction;

    //state machine
    enum State
    {

        fist_stand, 
        fist_jump, 
        fist_duck, fist_legsweep,
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
    bool isDivekicking = false;
    int swordTempTimer = 0;

    //sword
    bool isCollideWithSword;
    bool isArmed;
    bool isFence;
    bool isPrepThrow;
    int swordPos = 0;

    string transitionToMap;

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
        hasDied = false;
        isArmed = false;
        canRespawn = true;
    }

    void Update()
    {
        if (!canRespawn)
        print(canRespawn);
        //if (notInMap) transitionTo(transitionToMap);


        //tState -= Time.deltaTime;
        UpdateState();
        if (Input.GetKeyDown(attack))
        {
            if(!(
                (
                    myAnim.GetCurrentAnimatorStateInfo(0).IsName("Fist_Duck_Animation")
                    ||
                    myAnim.GetCurrentAnimatorStateInfo(0).IsName("Sword_Duck_Animation")
                )// ultimate roll attack
                &&
                !IsGrounded()
                ))
            myAnim.SetBool("isAttacking", true);
        }
        else myAnim.SetBool("isAttacking", false);

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
                    myAnim.SetBool("isDucking", true);
                }
                else
                {
                    myAnim.SetBool("isJumping", true);  
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
                    myAnim.SetBool("isDucking", true);
                }
                else if (Input.GetKey(up)) //prepthrow
                {
                    myAnim.SetInteger("swordPos", 2);
                }
                else
                {
                    myAnim.SetBool("isJumping", true);
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
                Move(moveSpeed);
                Jump(jumpPower);
                if (Input.GetKeyDown(jump) && IsGrounded())
                {
                    StartState(State.fist_jump);
                }

                if (Input.GetKeyDown(attack)) 
                {
                    //FistAttack();
                }

                if (Input.GetKeyUp(attack))
                {
                    myAnim.SetBool("isAttacking", false);
                }

                if (Input.GetKey(down)) 
                {
                    StartState(State.fist_duck);
                }
                break;


            case State.fist_jump:
                Move(moveSpeed);

                if (IsGrounded())
                {
                    Jump(jumpPower);
                }

                /*
                //divekick
                if (hasJumped && Input.GetKeyDown(attack) && (!Input.GetKey(down)))
                {
                    print(212);
                        Divekick();
                        isDivekicking = true;
                }
                */

                //divekick landing
                if (IsGrounded())
                {
                    StartState(State.fist_stand);
                }

                /*
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
                */

                /*
                if (Input.GetKeyDown(attack)&&(!Input.GetKey(down)))
                {
                    //call up dive kick function
                    print(242);
                    Divekick();
                }
                */

                break;

            case State.fist_duck:
                Move(moveSpeedDuck);
                Jump(jumpPowerDuck);

                //change the sprite and collider for the player here
                myAnim.SetBool("isDucking", true);

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
                    //Legsweep();
                }

                if (Input.GetKeyUp(attack))
                {
                    myAnim.SetBool("isAttacking", false);
                }

                if (isCollideWithSword && myAnim.GetCurrentAnimatorStateInfo(0).IsName("Fist_Duck_Animation"))
                {
                    pickSword();
                    //if I'm still ducking, won't dirrectly enter lunge state
                    StartState(State.sword_duck);
                }
                break;

            case State.sword_stand:
                Move(moveSpeed);
                Jump(jumpPower);
                SwordAction();

                /*
                if (Input.GetKeyDown(jump) && IsGrounded())
                {
                    StartState(State.sword_jump);
                }
                */

                if (Input.GetKeyDown(up) && swordPos < 1)
                {
                    swordPos += 1;
                    OnSwordPosChanged?.Invoke(swordPos);
                }

                if (Input.GetKeyDown(down) && swordPos > -1)
                {
                    swordPos -= 1;
                    OnSwordPosChanged?.Invoke(swordPos);
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
                        myAnim.SetBool("isAttacking", true);
                    }
                } 

                if (isPrepThrow && Input.GetKeyDown(attack))
                {
                        myAnim.SetBool("isAttacking", true);
                        //call throw sword function
                        disArmed();
                        StartState(State.fist_stand);
       
                    if (Input.GetKeyUp(attack))
                    {
                        myAnim.SetBool("isAttacking", false);
                    }
                }

                break;

            case State.sword_jump:

                //tempTimer++;

                Move(moveSpeed);
                Jump(jumpPower);

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
                        myAnim.SetBool("isAttacking", true);
                        //call throw sword function
                    }

                    if(Input.GetKeyUp(attack))
                    {
                        myAnim.SetBool("isAttacking", false);
                        disArmed();
                        isPrepThrow = false;
                        StartState(State.fist_stand);
                    }  
                }

                //divekick
                /*
                if (!isPrepThrow && hasJumped && Input.GetKeyDown(attack) && (!Input.GetKey(down)))
                {
                    print(415);
                    Divekick();
                    isDivekicking = true;
                }
                */

                //divekick or prepthrow landing
                //if ((isDivekicking || isPrepThrow) && hasJumped && IsGrounded() && tempTimer > 50)
                /*
                if (IsGrounded())
                {
                    print(422);
                    StartState(State.sword_stand);
                }
                */

                //normal landing, remember to adjust time for animation duration
                //if (!isDivekicking && hasJumped && IsGrounded() && tempTimer > 50)
                if (IsGrounded())
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
                Move(moveSpeedDuck);
                Jump(jumpPowerDuck);

                myAnim.SetBool("isDucking", true);

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
                    //Legsweep();
                }

                if (Input.GetKeyUp(attack))
                {
                    myAnim.SetBool("isAttacking", false);
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
                isDivekicking = false;
                myAnim.SetBool("isAttacking", false);
                myAnim.SetBool("isJumping", false);
                break;
            case State.fist_duck:
                myAnim.SetBool("isDucking", false);
                break;
            case State.sword_stand:
                //tState = fist attack time
                break;
            case State.sword_jump:
                hasJumped = false;
                isDivekicking = false;
                myAnim.SetBool("isAttacking", false);
                myAnim.SetBool("isJumping", false);
                swordPos = 0;
                break;
            case State.sword_duck:
                myAnim.SetBool("isDucking", false);
                break;

        }
    }

    //horizontal move
    private void Move(float _moveSpeed)
    {
        if (
            myAnim.GetCurrentAnimatorStateInfo(0).IsName("Pos-1_Attack_Animation")
            || myAnim.GetCurrentAnimatorStateInfo(0).IsName("Pos0_Attack_Animation")
            || myAnim.GetCurrentAnimatorStateInfo(0).IsName("Pos1_Attack_Animation")
            )
        {
            direction = 1;
            myAnim.SetBool("isRunning", false);
        }
        else if (Input.GetKey(left))
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

        switch (GameManager.currentGOState)
        {
            case GameManager.GOState.GORight:
                if(playerSide == "Left")
                {
                    if(!(isOutOfLeftCameraEdge(gameObject) && direction == -1))
                    {
                        rb.velocity = new Vector2(direction * _moveSpeed, rb.velocity.y);
                    }
                    else
                    {
                        rb.velocity = new Vector2(0, 0);
                    }
                }
                else if (playerSide == "Right")
                {
                    if (!(isOutOfLeftCameraEdge(gameObject) && direction == -1)
                    &&
                     !(isOutOfRightCameraEdge(gameObject) && direction == 1))
                    {
                        rb.velocity = new Vector2(direction * _moveSpeed, rb.velocity.y);
                    }
                    else
                    {
                        rb.velocity = new Vector2(0, 0);
                    }
                }
                break;

            case GameManager.GOState.GOLeft:
                if (playerSide == "Left")
                {
                    if (!(isOutOfLeftCameraEdge(gameObject) && direction == -1)
                    &&
                     !(isOutOfRightCameraEdge(gameObject) && direction == 1))
                    {
                        rb.velocity = new Vector2(direction * _moveSpeed, rb.velocity.y);
                    }
                    else
                    {
                        rb.velocity = new Vector2(0, 0);
                    }
                }
                else if (playerSide == "Right")
                {
                    if (!(isOutOfRightCameraEdge(gameObject) && direction == 1))
                    {
                        rb.velocity = new Vector2(direction * _moveSpeed, rb.velocity.y);
                    }
                    else
                    {
                        rb.velocity = new Vector2(0, 0);
                    }
                }
                break;

            case GameManager.GOState.NoGO:
                if (!(isOutOfLeftCameraEdge(gameObject) && direction == -1)
                    &&
                     !(isOutOfRightCameraEdge(gameObject) && direction == 1))
                {
                    rb.velocity = new Vector2(direction * _moveSpeed, rb.velocity.y);
                }
                else
                {
                    rb.velocity = new Vector2(0,0);
                }

                break;
        }




  
        
    }

    private void Jump(float _jumpPower)
    {
        if (IsGrounded() && Input.GetKeyDown(jump))
        {
            myAnim.SetBool("isJumping", true);
            rb.velocity = new Vector2(rb.velocity.x, _jumpPower);
            hasJumped = true;
        }
        
    }
    /*
    private void FistAttack()
    {
        if (myAnim.GetBool("isAttacking")) myAnim.SetBool("isAttacking", false);
        else myAnim.SetBool("isAttacking", true);

        //fist attack/punch code here
    }
    */

    private void SwordAction()
    {
        if (Input.GetKeyDown(attack))
        {

            //if(myAnim.GetBool("isAttacking")) myAnim.SetBool("isAttacking", false);
            //else myAnim.SetBool("isAttacking", true);
            switch (swordPos)
            {
                case -1:
                    myAnim.SetInteger("swordPos", -1);
                    break;

                case 0:
                    myAnim.SetInteger("swordPos", 0);
                    break;

                case 1:
                    myAnim.SetInteger("swordPos", 1);
                    break;
            }
        }

        if (isPrepThrow) //prepping throw sword
        {
            myAnim.SetInteger("swordPos", 2);

            if (Input.GetKeyDown(attack)) //throwing sword
            {
                myAnim.SetBool("isAttacking", true);
                disArmed();
            }
        }
    }

    /*
    private void Divekick()
    {
            myAnim.SetBool("isAttacking", true);

            //divekick attack and movement code here
    }
    */
    /*
    private void Legsweep()
    {
        if (myAnim.GetBool("isAttacking")) myAnim.SetBool("isAttacking", false);
        else myAnim.SetBool("isAttacking", true);
    }
    

    */
    private void pickSword()
    {
        print(631);
        isArmed = true;
        myAnim.SetBool("isArmed", true);
        //code here
        //...

        //StartState(State.LungeMid);
    }
     

    public void disArmed()
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
        disArmed();
        //create a sword, with initial state rotation
        //...
        //StartState(State.Idle);
    }

    public bool IsGrounded()
    {
        bool temp = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
        if (temp)
        {
            myAnim.SetBool("isOnGround", true);
            myAnim.SetBool("isJumping", false);
        }
        else
        {
            myAnim.SetBool("isOnGround", false);
        }
        return temp;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("sword"))
        {
            isCollideWithSword = true;
        }
        else
        {
            isCollideWithSword = false;
        }

        if (collision.CompareTag("Fallen"))
        {
            //remember to implement new respawn pos based on the other player and isgrounded

            DieStartPos();
            hasDied = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((collision.CompareTag("map0")))
        {
            if ((GameManager.currentGOState == GameManager.GOState.GORight)&& (playerSide == "Left") && isOutOfRightCameraEdge(gameObject))
            {
                Camera.main.GetComponent<GameManager>().changeScene("mapR1");
            }
        }

        if ((collision.CompareTag("mapR1")))
        {
            print("MapR1 Exit being called");
            if ((GameManager.currentGOState == GameManager.GOState.GORight) && (playerSide == "Left") && isOutOfRightCameraEdge(gameObject))
            {
                Camera.main.GetComponent<GameManager>().changeScene("mapR2");
            }
        }

        if ((collision.CompareTag("mapR2")))
        {
            if ((GameManager.currentGOState == GameManager.GOState.GORight) && (playerSide == "Left") && isOutOfRightCameraEdge(gameObject))
            {
                Camera.main.GetComponent<GameManager>().changeScene("mapR3");  
            }
        }

    }

    /*
    private void transitionTo(string map)
    {
            Camera.main.GetComponent<GameManager>().changeScene(map);
    }*/

        public void Die(Vector3 _respawnPos) 
        {
            //write a timer here
            //...
            //if timer <= 0
            //respawn
            if (canRespawn)
            {
            transform.position = _respawnPos;
            }     
            //death behaviors here, clear all accumulated values?
        }

         public void DieStartPos() 
        {
        if (gameObject.CompareTag("RightPlayer"))
        {
            transform.position = GameManager.RightPlayerRespawnPos;
        }
        if (gameObject.CompareTag("LeftPlayer"))
        {
            transform.position = GameManager.LeftPlayerRespawnPos;
        }
    }
   

    public static bool isOutOfLeftCameraEdge(GameObject player)
    {
        float camLeftEdge = Camera.main.ViewportToWorldPoint(new Vector3(0.01f, 0, 0)).x;

        if (player.transform.position.x < camLeftEdge)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool isOutOfRightCameraEdge(GameObject player)
    {
        float camRightEdge = Camera.main.ViewportToWorldPoint(new Vector3(0.99f, 0, 0)).x;

        if (player.transform.position.x > camRightEdge)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    



}
