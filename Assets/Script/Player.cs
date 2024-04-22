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
    [SerializeField] float moveSpeedDuck;

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
<<<<<<< Updated upstream
        Idle,FistAttack,
        Duck,DuckAttack,
        SwordDuck,LungeLow,LungeMid,LungeHigh,PrepThrow,
        AttackLow,AttackMid,AttackHigh,
        SwordDuckAttack
=======
        fist_stand, fist_jump, fist_duck,                                                                                                                                                                                                                                                                 
        sword_stand, sword_jump, sword_duck

>>>>>>> Stashed changes
    }
    State currentState;
    float tState;

    //sword
    bool isCollideWithSword;
    bool isArmed;
<<<<<<< Updated upstream
=======
    bool isFence;
    bool isPrepThrow;
    //0,1,2 for low, mid, high
    int swordPos = 0;
>>>>>>> Stashed changes

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
<<<<<<< Updated upstream
        tState -= Time.deltaTime;
=======

        //print(currentState);
        //print(hasJumped);

        //Animator setup
        ColorAnimation(playerSide, isArmed, spriteRenderer, playerColor);





>>>>>>> Stashed changes
    }

    void StartState(State newState)
    {
        EndState(currentState);
        currentState = newState;
        switch (newState)
        {
            case State.FistAttack:
                //tState = fist attack animate time
                break;
<<<<<<< Updated upstream
            case State.DuckAttack:
                //tState = duck attack animate time
=======
            case State.fist_jump:
                //animation
                if (Input.GetKey(down))
                {
                    myAnim.SetBool("isDuck", true);
                }
                else
                {
                    myAnim.SetBool("isJump", true);
                    
                }
>>>>>>> Stashed changes
                break;
            case State.AttackLow:
                //tState = attack low animate time
                break;
            case State.AttackMid:
                //tState = attack mid animate time
                break;
<<<<<<< Updated upstream
            case State.AttackHigh:
                //tState = attack high animate time
=======
            case State.sword_jump:
                //animation
                if (Input.GetKey(down))
                {
                    myAnim.SetBool("isDuck", true);
                }
                else if (Input.GetKey(up)) //prepthrow
                {
                    myAnim.SetInteger("swordPos", 2);
                }
                else
                {
                    myAnim.SetBool("isJump", true);
                }
>>>>>>> Stashed changes
                break;
            case State.SwordDuckAttack:
                //tState = sword duck attack animate time
                break;
        }
    }
    void UpdateState()
    {
        switch (currentState)
        {
<<<<<<< Updated upstream
            case State.Idle:
                Move();
                Jump();
                //codes here
                //...

                if (Input.GetKeyDown(lunge)) StartState(State.FistAttack);
                if (Input.GetKeyDown(down)) StartState(State.Duck);
=======
            case State.fist_stand:
                Move(moveSpeed);

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
                    myAnim.SetBool("isAttack", false);
                }

                if (Input.GetKey(down)) 
                {
                    StartState(State.fist_duck);
                }
>>>>>>> Stashed changes
                break;
            case State.FistAttack:
                //codes here
                //...

<<<<<<< Updated upstream
                if (tState <= 0) StartState(State.Idle);
                break;
            case State.Duck:
                Move();
                Jump();
                //codes here
                //...

                if (isCollideWithSword)
=======

            case State.fist_jump:

                tempTimer++;

                Move(moveSpeed);

                if (!hasJumped)
                {
                    Jump(jumpPower);
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
                    print("232");
                        if (Input.GetKey(down) && myAnim.GetCurrentAnimatorStateInfo(0).IsName("Fist_Duck_Animation"))
                        {
                            StartState(State.fist_duck);
                        print(236);
                        }
                        else if (myAnim.GetCurrentAnimatorStateInfo(0).IsName("Fist_Jump_Animation"))
                        {
                            StartState(State.fist_stand);
                        print(240);
                        }
 
                }

                if (Input.GetKeyDown(attack)&&(!Input.GetKey(down)))
                {
                    //call up dive kick function
                }


                break;

            case State.fist_duck:
                Move(moveSpeed);
                //change the sprite and collider for the player here
                myAnim.SetBool("isDuck", true);

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
                    myAnim.SetBool("isLegSweep", false);
                }

                if (isCollideWithSword && myAnim.GetCurrentAnimatorStateInfo(0).IsName("Fist_Duck_Animation"))
>>>>>>> Stashed changes
                {
                    pickSword();
                    StartState(State.LungeMid);
                }
                if (!Input.GetKey(lunge)) StartState(State.Idle);
                break;
            case State.DuckAttack:
                //codes here
                //...

<<<<<<< Updated upstream
                if (tState <= 0) StartState(State.Duck);
=======
            case State.sword_stand:
                Move(moveSpeed);
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
                        myAnim.SetBool("isAttack", true);
                    }
                    if (Input.GetKeyUp(attack))
                    {
                        myAnim.SetBool("isSwordAttacking", false);
                    }
                } 

                if (isPrepThrow && Input.GetKeyDown(attack))
                {
                        myAnim.SetBool("isAttack", true);
                        //call throw sword function
                        disArmed();
                        StartState(State.fist_stand);
       
                    if (Input.GetKeyUp(attack))
                    {
                        myAnim.SetBool("isAttack", false);
                    }
                }

>>>>>>> Stashed changes
                break;
            case State.SwordDuck:
                //codes here
                //...

<<<<<<< Updated upstream
                if (Input.GetKey(up)) StartState(State.LungeLow);
=======
            case State.sword_jump:

                tempTimer++;

                Move(moveSpeed);

                if (!hasJumped)
                {
                    Jump(jumpPower);
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
                        myAnim.SetBool("isAttack", true);
                        //call throw sword function
                    }

                    if(Input.GetKeyUp(attack))
                    {
                        myAnim.SetBool("isAttack", false);
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


>>>>>>> Stashed changes
                break;
            case State.LungeLow:
                //codes here
                //...

<<<<<<< Updated upstream
                if (Input.GetKey(up)) StartState(State.LungeMid);
                if (Input.GetKey(down)) StartState(State.SwordDuck);
                if (Input.GetKey(lunge)) StartState(State.AttackLow);
=======
            case State.sword_duck:
                Move(moveSpeedDuck);

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


>>>>>>> Stashed changes
                break;
            case State.LungeMid:
                //codes here
                //...

                if (Input.GetKey(up)) StartState(State.LungeHigh);
                if (Input.GetKey(down)) StartState(State.LungeLow);
                if (Input.GetKey(lunge)) StartState(State.AttackMid);
                break;
            case State.LungeHigh:
                //codes here
                //...

                if (Input.GetKey(up)) StartState(State.PrepThrow);
                if (Input.GetKey(down)) StartState(State.LungeMid);
                if (Input.GetKey(lunge)) StartState(State.AttackHigh);
                break;
            case State.PrepThrow:
                //codes here
                //...

                if (Input.GetKey(down)) StartState(State.LungeHigh);
                if (Input.GetKey(lunge)) throwSword ();
                break;
            case State.SwordDuckAttack:
                //codes here
                //...

                if (tState <= 0) StartState(State.SwordDuck);
                break;
            case State.AttackLow:
                //codes here
                //...

                if (tState <= 0) StartState(State.LungeLow);
                    break;
            case State.AttackMid:
                //codes here
                //...

                if (tState <= 0) StartState(State.LungeMid);
                    break;
            case State.AttackHigh:
                //codes here
                //...

                if (tState <= 0) StartState(State.LungeHigh);
                    break;
        }
    }
    void EndState(State currentState)
    {
        switch (currentState)
        {
<<<<<<< Updated upstream
=======
            case State.fist_stand:
                //tState = fist attack time
                break;
            case State.fist_jump:
                hasJumped = false;
                tempTimer = 0;
                isDivekicking = false;
                myAnim.SetBool("isDiveKick", false);
                myAnim.SetBool("isJump", false);
                break;
            case State.fist_duck:
                myAnim.SetBool("isDuck", false);
                break;
            case State.sword_stand:
                //tState = fist attack time
                break;
            case State.sword_jump:
                hasJumped = false;
                tempTimer = 0;
                isDivekicking = false;
                myAnim.SetBool("isDiveKick", false);
                myAnim.SetBool("isJump", false);
                swordPos = 0;
                break;
            case State.sword_duck:
                myAnim.SetBool("isDuck", false);

                break;
>>>>>>> Stashed changes

        }
    }

    //horizontal move
    private void Move(float _moveSpeed)
    {

        if (Input.GetKey(left))
        {
            direction = -1;
            tf.localScale = new Vector3(-1, 1, 1);
<<<<<<< Updated upstream
=======
            myAnim.SetBool("isRun", true);
>>>>>>> Stashed changes

        }
        else if (Input.GetKey(right))
        {
            direction = 1;
            tf.localScale = new Vector3(1, 1, 1);
<<<<<<< Updated upstream
=======
            myAnim.SetBool("isRun", true);
>>>>>>> Stashed changes
        }
        else
        {
            direction = 0;
<<<<<<< Updated upstream

=======
            myAnim.SetBool("isRun", false);
>>>>>>> Stashed changes
            if (playerSide == "Left")
            {
                tf.localScale = new Vector3(1, 1, 1);
            }
            else if (playerSide == "Right")
            {
                tf.localScale = new Vector3(-1, 1, 1);
            }

        }
        rb.velocity = new Vector2(direction * _moveSpeed, rb.velocity.y);
    }

    private void Jump(float _jumpPower)
    {
<<<<<<< Updated upstream
        if (Input.GetKeyDown(jump) && IsGrounded())
        {
            myAnim.SetBool("isJumping", true);
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
=======
        rb.velocity = new Vector2(rb.velocity.x, _jumpPower);

        hasJumped = true;
    }

    private void FistAttack()
    {
        myAnim.SetBool("isAttack", true);

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
                    myAnim.SetBool("isAttack", true);
                    //code for sword action and hitboxes at different height
                }
                if (Input.GetKeyUp(attack))
                {
                    myAnim.SetBool("isAttack", false);
                }

                break;

            case 0:
                myAnim.SetInteger("swordPos", 0);

                if (Input.GetKeyDown(attack))
                {
                    myAnim.SetBool("isAttack", true);
                    //code for sword action and hitboxes at different height
                }
                if (Input.GetKeyUp(attack))
                {
                    myAnim.SetBool("isAttack", false);
                }


                break;

            case 1:
                myAnim.SetInteger("swordPos", 1);

                if (Input.GetKeyDown(attack))
                {
                    myAnim.SetBool("isAttack", true);
                    //code for sword action and hitboxes at different height
                }
                if (Input.GetKeyUp(attack))
                {
                    myAnim.SetBool("isAttack", false);
                }

                break;
>>>>>>> Stashed changes
        }

        if (IsGrounded() && myAnim.GetCurrentAnimatorStateInfo(0).IsName("Jump_Animation"))
        {
<<<<<<< Updated upstream
            myAnim.SetBool("isJumping", false);
=======
            myAnim.SetInteger("swordPos", 2);

            if (Input.GetKeyDown(attack)) //throwing sword
            {
                myAnim.SetBool("isAttack", true);
                disArmed();
            }
            if (Input.GetKeyUp(attack))
            {
                myAnim.SetBool("isAttack", false);
            }
        }

    }

    private void Divekick()
    {
        if (!isArmed)
        {
            myAnim.SetBool("isDiveKick", true);
        }

        else if (isArmed)
        {
            myAnim.SetBool("isDiveKick", true);
        }

        //divekick attack and movement code here
    }

    private void Legsweep()
    {
        if (!isArmed)
        {
            myAnim.SetBool("isLegSweep", true);
        }

        else if (isArmed)
        {
            myAnim.SetBool("isLegSweep", true);
>>>>>>> Stashed changes
        }
    }

    private void pickSword()
    {
        isArmed = true;
        //code here
        //...

        StartState(State.LungeMid);
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

<<<<<<< Updated upstream
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }
=======
    

>>>>>>> Stashed changes
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //fall out of scene 
        if (collision.CompareTag("Fallen"))
        {
            Die(startPos);
        }//death
        //exist pickable sword
        if (collision.CompareTag("sword"))
        {
            isCollideWithSword = true;
        }//change bool 
        else
        {
            isCollideWithSword = false;
        }
    }
    //--------------------------------- DO NOT CHANGE CODES BELOW ---------------------------------
    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }
    //------------------------------- Perfectly Encapsulated Methods ------------------------------
    private static void ColorAnimation(string _playerSide, bool _isArmed, SpriteRenderer _spriteRenderer, Color _playerColor)
    {
        //Animator setup, if need different colored sword animations for different players
        if (_playerSide == "Left")
        {
            if (!_isArmed)
            {
                _spriteRenderer.color = _playerColor;
            }

            if (_isArmed)
            {
                _spriteRenderer.color = Color.white;
            }
        }
        if (_playerSide == "Right")
        {
            if (!_isArmed)
            {
                _spriteRenderer.color = _playerColor;
            }

            if (_isArmed)
            {
                _spriteRenderer.color = Color.white;
            }
        }
    }
    void Die(Vector3 _respawnPos) 
    {
        transform.position = _respawnPos;
    }
}
