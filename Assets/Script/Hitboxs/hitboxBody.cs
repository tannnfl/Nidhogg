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
        //fist
        if (collision.gameObject.name == "hitboxFist"
            && isAnim(collision.gameObject, "Fist_Attack_Animation"))
        {
            player.DieStartPos();
        }

        //sword die
        if(
            //sword low
            (
            collision.gameObject.name == "hitboxSword-1" &&(
             isAnim(collision.gameObject,"Pos-1_Attack_Animation")
             || isAnim(collision.gameObject, "Pos-1_Fence_Animation"))
            )
            //sword mid
            || (
            collision.gameObject.name == "hitboxSword0" &&(
            isAnim(collision.gameObject,"Pos0_Attack_Animation")
            || isAnim(collision.gameObject,"Pos0_Fence_Animation"))
            )
            //sword high
            || (
            collision.gameObject.name == "hitboxSword1" &&(
            isAnim(collision.gameObject, "Pos1_Attack_Animation")
            || isAnim(collision.gameObject,"Pos1_Fence_Animation"))
            )
        )
        {
            player.DieStartPos();
        }
    }

    private static bool isAnim(GameObject obj, string animName)
    {
        return obj.GetComponentInParent<Animator>().GetCurrentAnimatorStateInfo(0).IsName(animName);
    }
}
