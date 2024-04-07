using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMidPoint : MonoBehaviour
{
    public Transform Player1;
    public Transform Player2;

    private void Update()
    {
        float midpointX = (Player1.position.x + Player2.position.x) / 2f;

        // Set the position of this object to the calculated midpoint
        this.transform.position = new Vector3(midpointX, this.transform.position.y, this.transform.position.z);
    }

}
