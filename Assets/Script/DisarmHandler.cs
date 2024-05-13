using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisarmHandler
{
    private Player player1;
    private Player player2;
    private hitboxDisarm player1Hitbox;
    private hitboxDisarm player2Hitbox;

    public DisarmHandler()
    {
        player1 = GameObject.Find("Player1").GetComponent<Player>();
        player2 = GameObject.Find("Player2").GetComponent<Player>();
        player1Hitbox = GameObject.Find("Player1/hitboxDisarm").GetComponent<hitboxDisarm>();
        player2Hitbox = GameObject.Find("Player2/hitboxDisarm").GetComponent<hitboxDisarm>();

        Player.OnSwordPosChanged += OnSwordPosChange;
    }
    private void OnSwordPosChange(int playerIndex)
    {
        Debug.Log(1);
        int player1SwordPos = player1.GetSwordPos();
        int player2SwordPos = player2.GetSwordPos();

        Player attacker = null;
        Player victim = null;
        int attackerSwordPos = -100;
        int victimSwordPos = -100;
        if (playerIndex == 1)
        {
            attacker = player1;
            victim = player2;
            attackerSwordPos = player1SwordPos;
            victimSwordPos = player2SwordPos;
        }
        if (playerIndex == 2)
        {
            attacker = player2;
            victim = player1;
            attackerSwordPos = player2SwordPos;
            victimSwordPos = player1SwordPos;
        }

        Debug.Log(2);
        if (player2Hitbox == null) return;
        if (player1Hitbox == null) return;
        (bool low, bool mid, bool high) isPlayer1In2 = player2Hitbox.isStapped();
        (bool low, bool mid, bool high) isPlayer2In1 = player1Hitbox.isStapped();

        (bool low, bool mid, bool high) isVictimStapped = (false, false, false);
        if (playerIndex == 1) isVictimStapped = isPlayer1In2;
        else isVictimStapped = isPlayer2In1;

        bool isStapped = false;
        if (attackerSwordPos == -1)
            isStapped = isVictimStapped.low;
        if (attackerSwordPos == 0)
            isStapped = isVictimStapped.mid;
        if (attackerSwordPos == 1)
            isStapped = isVictimStapped.high;
        if (isStapped == false) return;

        Debug.Log(3);

        bool isSameHeight = player1SwordPos == player2SwordPos;
        if (isSameHeight == false) return;
        Debug.Log(4);

        bool isAttackerFencing = attacker.myAnim.GetCurrentAnimatorStateInfo(0).IsName($"Pos{attackerSwordPos}_Fence_Animation");
        if (isAttackerFencing == false) return;
        Debug.Log(5);

        bool isVictimFencing = victim.myAnim.GetCurrentAnimatorStateInfo(0).IsName($"Pos{victimSwordPos}_Fence_Animation");
        bool isVictimAttacking = victim.myAnim.GetCurrentAnimatorStateInfo(0).IsName($"Pos{victimSwordPos}_Attack_Animation");
        if (!isVictimFencing && !isVictimAttacking) return;
        Debug.Log(6);

        victim.disArmed();
        Debug.Log(7);
    }
}
