using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowedSword : MonoBehaviour
{
    float Speed = 15f;
    private void Update()
    {
        transform.position += -transform.right * Time.deltaTime * Speed;
        this.tag = "flyingSword";
    }
    

}
