using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hitboxBody : MonoBehaviour
{
    //general
    Collider2D body;
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
    float hp;
    [SerializeField] float maxHp;
    

    // Start is called before the first frame update
    void Start()
    {
        body = GameObject.Find("hitboxBody").GetComponent<Collider2D>();
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


        hp = maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        if (hp == 0) player.DieStartPos();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //print("on trigger");
        if (collision == fist)
        {
            hp -= 1f;
            print("fist hit");
        }
    }
}
