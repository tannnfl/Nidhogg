using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitboxDisarm : MonoBehaviour
{
    Animator myAnim;
    [SerializeField] Player player;
    int mySwordPos;
    int oldSwordPos;

    // Start is called before the first frame update
    void Start()
    {
        myAnim = player.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    /*
    void OnTriggerStay2D(Collider2D collision)
    {
        GameObject opponent = collision.gameObject.transform.parent.gameObject;

        if (opponent.name == "Player1" || opponent.name == "Player2")
        {
            //print(opponent);
            bool oldPosLow = (collision.gameObject.name == "hitboxSwordAttack-1" && (isAnim(opponent, "Pos-1_Attack_animation") || isAnim(opponent, "Pos-1_Fence_Animation")));
            bool oldPosMid = (collision.gameObject.name == "hitboxSwordAttack0" && (isAnim(opponent, "Pos0_Attack_animation") || isAnim(opponent, "Pos0_Fence_Animation")));
            bool oldPosHigh = (collision.gameObject.name == "hitboxSwordAttack1" && (isAnim(opponent, "Pos1_Attack_animation") || isAnim(opponent, "Pos1_Fence_Animation")));
            //print("low: " + oldPosLow + ", mid: " + oldPosMid + ", high" + oldPosHigh);
            //print(collision.gameObject.name == "hitboxSwordAttack-1" +" "+ isAnim(opponent, "Pos-1_Attack_animation") + " " + isAnim(opponent, "Pos-1_Fence_Animation"));
            //print(collision.gameObject.name == "hitboxSwordAttack0" + " " + isAnim(opponent, "Pos0_Attack_animation") + " " + isAnim(opponent, "Pos0_Fence_Animation"));
            //print(collision.gameObject.name == "hitboxSwordAttack1" + " " + isAnim(opponent, "Pos1_Attack_animation") + " " + isAnim(opponent, "Pos1_Fence_Animation"));
            if (oldPosLow || oldPosMid || oldPosHigh)
            {
                Player player = opponent.GetComponent<Player>();
                //print(player);
                if (player != null)
                {
                    HandleSwordPosChanged(player.GetSwordPos(), oldPosLow, oldPosMid, oldPosHigh);
                }
            }
        }
    }
    */



    private static bool isAnim(GameObject obj, string animName)
    {
        return obj.GetComponentInParent<Animator>().GetCurrentAnimatorStateInfo(0).IsName(animName);
    }
    private static bool isAnimBool(GameObject obj, string boolName)
    {
        return obj.GetComponentInParent<Animator>().GetBool(boolName);
    }
    private bool isAnim(string animName)
    {
        return myAnim.GetCurrentAnimatorStateInfo(0).IsName(animName);
    }


    void HandleSwordPosChanged(int newSwordPos, bool wasLow, bool wasMid, bool wasHigh)
    {
        print("HandleSwordPosChanged");
        //get opponent sword position before change
        if (wasLow) oldSwordPos = -1;
        else if (wasMid) oldSwordPos = 0;
        else if (wasHigh) oldSwordPos = 1;
        //get my sword position

        if (isAnim("Pos-1_Attack_animation") || isAnim("Pos-1_Fence_Animation"))
        {
            mySwordPos = -1;
        }
        else if (isAnim("Pos0_Attack_animation") || isAnim("Pos0_Fence_Animation"))
        {
            mySwordPos = 0;
        }
        else if (isAnim("Pos1_Attack_animation") || isAnim("Pos1_Fence_Animation"))
        {
            mySwordPos = 1;
        }

        //handle disarm scenarios:
        switch (mySwordPos)
        {
            case -1:
                //up
                if (oldSwordPos == -1 && newSwordPos == 0) player.disArmed();
                //down
                if (oldSwordPos == 0 && newSwordPos == -1) player.disArmed();
                break;
            case 0:
                //up
                if (oldSwordPos == -1 && newSwordPos == 0) player.disArmed();
                if (oldSwordPos == 0 && newSwordPos == 1) player.disArmed();
                //down
                if (oldSwordPos == 0 && newSwordPos == -1) player.disArmed();
                if (oldSwordPos == 1 && newSwordPos == 0) player.disArmed();
                break;
            case 1:
                //up
                if (oldSwordPos == 0 && newSwordPos == 1) player.disArmed();
                //down
                if (oldSwordPos == 1 && newSwordPos == 0) player.disArmed();
                break;
        }

    }
}
