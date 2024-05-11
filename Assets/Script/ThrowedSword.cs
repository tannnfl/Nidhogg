using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowedSword : MonoBehaviour
{
    public float Speed = 4.5f;
    private void Update()
    {

            transform.position += -transform.right * Time.deltaTime * Speed;

    }

}
