using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitboxBody : MonoBehaviour
{
    Animator myAnim;
    [SerializeField] Player player;


    // Start is called before the first frame update
    void Start()
    {
        myAnim = player.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (fistedCheck(collision.gameObject, player)
            || swordedCheck(collision.gameObject, player)) { player.DieStartPos(); }
    }

    //get hit check
    public static bool fistedCheck(GameObject opp,Player pl) {
        return (opp.name == "hitboxFist"
            && isAnim(opp.gameObject, "Fist_Attack_Animation"));
    }
    public static bool divekickedCheck(GameObject opp, Player pl)
    {
        return (opp.name == "hitboxDiveKick"
            && (isAnim(opp, "Fist_Divekick_Animation") || isAnim(opp, "Sword_Divekick_Animation")));
    }
    public static bool LegsweepedCheck(GameObject opp, Player pl)
    {
        return (opp.name == "hitboxLegsweep"
            && (isAnim(opp, "Fist_Legsweep_Animation") || isAnim(opp, "Sword_Legsweep_Animation")));
        }
    public static bool swordedCheck(GameObject opp, Player pl)
    {
        return
            (
            opp.name == "hitboxSword-1" && (
             isAnim(opp, "Pos-1_Attack_Animation")
             || isAnim(opp, "Pos-1_Fence_Animation"))
            )
            //sword mid
            || (
            opp.name == "hitboxSword0" && (
            isAnim(opp, "Pos0_Attack_Animation")
            || isAnim(opp, "Pos0_Fence_Animation"))
            )
            //sword high
            || (
            opp.name == "hitboxSword1" && (
            isAnim(opp, "Pos1_Attack_Animation")
            || isAnim(opp, "Pos1_Fence_Animation"))
            );
    }
    public static bool isAnim(GameObject obj, string animName)
    {
        return obj.GetComponentInParent<Animator>().GetCurrentAnimatorStateInfo(0).IsName(animName);
    }
}
