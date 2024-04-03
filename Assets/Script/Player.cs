using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Control")]
    public KeyCode left;
    public KeyCode right;
    public KeyCode up;
    public KeyCode down;
    public KeyCode jump;
    public KeyCode lunge;

    [Header("Movement Tuning")]
    public float moveSpeed;

    //components
    Rigidbody2D rb;

    private void Start()
    {
        //components
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        //control
        ControllerMove();
    }

    private void ControllerMove()
    {
        if (Input.GetKeyDown(left))
        {
            rb.AddForce(new Vector2(-moveSpeed, 0));
        }
        else if (Input.GetKeyDown(right))
        {
            rb.AddForce(new Vector2(moveSpeed, 0));
        }
    }
}
