using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    //general
    public AudioSource runSnd, jumpSnd, punchSnd, lungeSnd;

    Collider2D body;
    EdgeCollider2D duckBody;
    Collider2D fist;
    Collider2D divekick;
    Collider2D legsweep;

    Collider2D swordLow;
    Collider2D swordMid;
    Collider2D swordHigh;

    Collider2D swordAttackLow;
    Collider2D swordAttackMid;
    Collider2D swordAttackHigh;

    Collider2D disarm;
    [SerializeField] Player player;

    //special
    Animator myAnim;
    string state;

    void Start()
    {
        //general
        body = GameObject.Find("hitboxBody").GetComponent<Collider2D>();
        duckBody = GameObject.Find("hitboxBody").GetComponent<EdgeCollider2D>();
        fist = GameObject.Find("hitboxFist").GetComponent<Collider2D>();
        divekick = GameObject.Find("hitboxDiveKick").GetComponent<Collider2D>();
        legsweep = GameObject.Find("hitboxLegsweep").GetComponent<Collider2D>();

        swordLow = GameObject.Find("hitboxSword-1").GetComponent<Collider2D>();
        swordMid = GameObject.Find("hitboxSword0").GetComponent<Collider2D>();
        swordHigh = GameObject.Find("hitboxSword1").GetComponent<Collider2D>();

        swordAttackLow = GameObject.Find("hitboxSwordAttack-1").GetComponent<Collider2D>();
        swordAttackMid = GameObject.Find("hitboxSwordAttack0").GetComponent<Collider2D>();
        swordAttackHigh = GameObject.Find("hitboxSwordAttack1").GetComponent<Collider2D>();

        disarm = GameObject.Find("hitboxDisarm").GetComponent<Collider2D>();

        //special
        myAnim = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //state = myAnim.GetCurrentAnimatorStateInfo(0).nameHash.ToString();

        //duck
        if (myAnim.GetCurrentAnimatorStateInfo(0).IsName("Fist_Duck_Animation")
            || myAnim.GetCurrentAnimatorStateInfo(0).IsName("Fist_Legsweep_Animation")
            || myAnim.GetCurrentAnimatorStateInfo(0).IsName("Sword_Legsweep_Animation")
            || myAnim.GetCurrentAnimatorStateInfo(0).IsName("Sword_Duck_Animation"))
        {
            duckBody.enabled = true;
            body.enabled = false;
        }
        else
        {
            duckBody.enabled = false;
            body.enabled = true;
        }

        //fist
        /*
        if (myAnim.GetCurrentAnimatorStateInfo(0).IsName("Fist_Attack_Animation"))
        {
            fist.enabled = true;
        }
        else
        {
            fist.enabled = false;
        }
        */

        //if (state == "Fist_Divekick_Animation")


        //Hitbox activate/ deactivate for each single hitbox
        //HitboxPosition


        if (myAnim.GetCurrentAnimatorStateInfo(0).IsName("Fist_Run_Animation")
            || myAnim.GetCurrentAnimatorStateInfo(0).IsName("Sword_Run_Animation"))
        {
            //GameManager.PlaySound(runSnd);
        }

        if (myAnim.GetCurrentAnimatorStateInfo(0).IsName("Fist_Jump_Animation")
            || myAnim.GetCurrentAnimatorStateInfo(0).IsName("Sword_Jump_Animation"))
        {
            GameManager.PlaySound(jumpSnd);
        }

        if (myAnim.GetCurrentAnimatorStateInfo(0).IsName("Fist_Attack_Animation"))
        {
            GameManager.PlaySound(punchSnd);
        }

        if (myAnim.GetCurrentAnimatorStateInfo(0).IsName("Pos0_Attack_Animation")
            || myAnim.GetCurrentAnimatorStateInfo(0).IsName("Pos-1_Attack_Animation")
            || myAnim.GetCurrentAnimatorStateInfo(0).IsName("Pos1_Attack_Animation"))
        {
            GameManager.PlaySound(lungeSnd);
        }


        switch (state)
        {
        //fist
            case "Fist_Stand_Animation":
                //hbSwordHead. Deactivate


                break;
            case "Fist_Run_Animation":
                //PlaySound(runSnd);
                break;
            case "Fist_Jump_Animation":
                //PlaySound(jumpSnd);
                print("jump");
                break;
            case "Fist_Divekick_Animation":
                //...
                break;
            case "Fist_Duck_Animation":
                break;
            case "Fist_Legsweep_Animation":
                break;
            case "Fist_Attack_Animation":
                break;
        //sword
            case "Pos-1_Fence_Animation":
                //hbSwordHead. Activate
                break;
            case "Pos-1_Attack_Animation":
                ////hbSwordHead. Activate
                break;
            case "Pos0_Fence_Animation":
                break;
            case "Pos0_Attack_Animation":
                break;
            case "Pos1_Fence_Animation":
                break;
            case "Pos1_Attack_Animation":
                break;
            case "PrepSword_Animation":
                break;
            case "Sword_Duck_Animation":
                break;
            case "Sword_Legsweep_Animation":
                break;
            case "Sword_Jump_Animation":
                break;
            case "Sword_Divekick_Animation":
                break;
            case "Sword_Run_Animation":
                break;
            case "ThrowSword_Animation":
                break;
        }
    }

}

