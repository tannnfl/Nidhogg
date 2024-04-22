using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    Animator myAnim;
    string state;

    [SerializeField]Collider2D hbSwordHead;

    void Start()
    {
        myAnim = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        state = myAnim.GetCurrentAnimatorStateInfo(0).ToString();

        //Hitbox activate/ deactivate for each single hitbox
        //Hitbox Position
        switch (state)
        {
        //fist
            case "Fist_Stand_Animation":
                //hbSwordHead. Deactivate


                break;
            case "Fist_Run_Animation":
                //hbSwordHead. Deactivate
                break;
            case "Fist_Jump_Animation":
                //hbSwordHead. Deactivate
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
