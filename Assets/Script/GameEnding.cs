using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameEnding : MonoBehaviour
{
    [SerializeField] Sprite Ending_Yellow;
    [SerializeField] Sprite Ending_Orange;
    public void Setup(string playerSide) 
    {
        var image = GetComponent<Image>();
        
        if (playerSide == "Left")
        {
            image.sprite = Ending_Yellow;
            gameObject.SetActive(true);
        }
        if (playerSide == "Right")
        {
            image.sprite = Ending_Orange;
            gameObject.SetActive(true);
        }

    }
}
