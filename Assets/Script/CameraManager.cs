using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    //Search for an active LeftPlayer and an active RightPlayer
    public GameObject LeftPlayer;
    public GameObject RightPlayer;
    public GameObject PlayerMidPoint;

    public Transform followTarget;

    //to take whatever state is current in GameManager
    public GOState currentGOState;

    private CinemachineVirtualCamera playerVirtualCam;

    //remember that Leftplayer always GORight, and Rightplayer always GOLeft!!!
    public enum GOState
    {
        GORight,
        GOLeft,
        NoGO,
    }

    public void Start()
    {
        playerVirtualCam = GetComponent<CinemachineVirtualCamera>();
    }

    public void Update()
    {
        playerVirtualCam.Follow = followTarget;
        playerVirtualCam.LookAt = followTarget;

        switch (currentGOState)
        {
            case GOState.GORight:
                if (LeftPlayer == null)
                {
                    LeftPlayer = GameObject.FindWithTag("LeftPlayer");
                }
                if (LeftPlayer != null)
                {
                    followTarget = PlayerMidPoint.transform;
                }
                    break;

            case GOState.GOLeft:
                if (RightPlayer == null)
                {
                    RightPlayer = GameObject.FindWithTag("RightPlayer");
                }
                if (RightPlayer != null)
                {
                    followTarget = PlayerMidPoint.transform;
                }
                break;

            case GOState.NoGO:

                followTarget = null;
                
                break;

        }
    }

}
