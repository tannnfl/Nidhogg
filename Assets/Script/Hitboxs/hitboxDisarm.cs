using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitboxDisarm : MonoBehaviour
{
    Animator myAnim;
    [SerializeField] Player player;
    int mySwordPos;
    int oldSwordPos;

    bool isStappedByLow;
    bool isStappedByMid;
    bool isStappedByHigh;

    DisarmHandler dh;

    // Start is called before the first frame update
    void Start()
    {
        myAnim = player.GetComponent<Animator>();
        dh = new DisarmHandler();
    }

    // Update is called once per frame
    void Update()
    {

    }



   public (bool, bool, bool) isStapped()
    {
        return (isStappedByLow, isStappedByMid, isStappedByHigh);
    }


    void OnTriggerStay2D(Collider2D collision)
    {
        if (divekickedCheck(collision.gameObject, player)) { player.disArmed(); }

        // CHANGE: 这里会抛出 NullPointerException，底下collision.gameobject.transform.parent sometimes don't exist
        // For instance, Map0_CamConfiner, Player1, and Player2 may not have parent object
        if (collision.gameObject.name == "Map0_CamConfiner") return;
        if (collision.gameObject.name == "Player1") return;
        if (collision.gameObject.name == "Player2") return;

        GameObject opponent = collision.gameObject.transform.parent.gameObject;

        if (opponent.name == "Player1" || opponent.name == "Player2")
        {
            //print(opponent);

            //print("low: " + oldPosLow + ", mid: " + oldPosMid + ", high" + oldPosHigh);
            //print(collision.gameObject.name == "hitboxSwordAttack-1" +" "+ isAnim(opponent, "Pos-1_Attack_animation") + " " + isAnim(opponent, "Pos-1_Fence_Animation"));
            //print(collision.gameObject.name == "hitboxSwordAttack0" + " " + isAnim(opponent, "Pos0_Attack_animation") + " " + isAnim(opponent, "Pos0_Fence_Animation"));
            //print(collision.gameObject.name == "hitboxSwordAttack1" + " " + isAnim(opponent, "Pos1_Attack_animation") + " " + isAnim(opponent, "Pos1_Fence_Animation"));
            if (collision.name == "hitboxSwordAttack0" || collision.name == "hitboxSwordAttack1" || collision.name == "hitboxSwordAttack-1") return;
            if (getOldSwordPos(collision, opponent) != -2)
            {
                Player player = opponent.GetComponent<Player>();
                //print(player);
                if (player != null)
                {
                    HandleSwordPosChanged(player.GetSwordPos(), collision, opponent);
                }
            }
        }
    }

    public static bool divekickedCheck(GameObject opp, Player pl)
    {
        return (opp.name == "hitboxDiveKick"
            && (isAnim(opp, "Fist_Divekick_Animation") || isAnim(opp, "Sword_Divekick_Animation")));
    }


    private static bool isAnim(GameObject obj, string animName)
    {
        return obj.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName(animName);
    }
    private static bool isAnimBool(GameObject obj, string boolName)
    {
        return obj.GetComponentInParent<Animator>().GetBool(boolName);
    }
    private bool isAnim(string animName)
    {
        return myAnim.GetCurrentAnimatorStateInfo(0).IsName(animName);
    }
    private int getOldSwordPos(Collider2D collision, GameObject opponent)
    {
        //
        bool temp2 = isAnim(opponent, "Pos1_Fence_Animation");
        bool oldPosLow = (collision.gameObject.name == "hitboxSword-1" && isAnim(opponent, "Pos-1_Fence_Animation"));
        bool oldPosMid = (collision.gameObject.name == "hitboxSword0" && isAnim(opponent, "Pos0_Fence_Animation"));
        bool oldPosHigh = (collision.gameObject.name == "hitboxSword1" && isAnim(opponent, "Pos1_Fence_Animation"));
        if (oldPosHigh) return 1;
        else if (oldPosMid) return 0;
        else if (oldPosLow) return -1;
        else return -2;
    }

    void HandleSwordPosChanged(int newSwordPos, Collider2D collision, GameObject opponent)
    {
        print("HandleSwordPosChanged");
        //get opponent sword position before change
        oldSwordPos = getOldSwordPos(collision, opponent);
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

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "hitboxSword-1")
            isStappedByLow = true;
        if (collision.gameObject.name == "hitboxSword0")
            isStappedByMid = true;
        if (collision.gameObject.name == "hitboxSword1")
            isStappedByHigh = true;
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "hitboxSword-1")
            isStappedByLow = false;
        if (collision.gameObject.name == "hitboxSword0")
            isStappedByMid = false;
        if (collision.gameObject.name == "hitboxSword1")
            isStappedByHigh = false;
    }

}
