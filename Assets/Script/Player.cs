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
    public float gravity;

    //components
    Rigidbody2D rb;
    Transform tf;

    //movement bools
    int direction;
    bool is_moving;

    private void Start()
    {
        //components
        rb = GetComponent<Rigidbody2D>();
        tf = GetComponent<Transform>();
    }

    void Update()
    {
        //control
        Controller();
        if (is_moving) Move();

        
    }

    //controller
    private void Controller()
    {
        if (Input.GetKey(left))
        {
            direction = -1;
            is_moving = true;

        }
        else if (Input.GetKey(right))
        {
            direction = 1;
            is_moving = true;
        }
        else
        {
            is_moving = false;
        }
    }

    //transform movement
    private void Move()
    {
        tf.position += new Vector3(moveSpeed * direction, 0, 0);
    }

    //rigidbody movement
    private void RBGravity()
    {
        rb.AddForce(new Vector2(0, -gravity));
        print("gravity");
    }
    private void RBMove()
    {
        rb.AddForce(new Vector2(direction * moveSpeed, 0));
    }
}
