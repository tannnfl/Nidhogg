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

    //Update the followtarget for the virtual cam
    public Transform followTarget;

    //take whatever state that is current in the GameManager
    public GOState currentGOState;

    //get virtual cam and cam width
    private CinemachineVirtualCamera playerVirtualCam;
    private Camera cam;
    private float camHalfWidth;

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

        cam = Camera.main;
        camHalfWidth = cam.aspect * cam.orthographicSize;
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
                    var distance = LeftPlayer.transform.position.x - PlayerMidPoint.transform.position.x;
                    //if distance indicates the leftplayer is heading left, it cannot move once reaching the point where rightplayer would be left out
                    //if distance indicates the leftplayer is heading right, the camera shifts and a new right player would spawn on the right
                    if (distance > camHalfWidth)
                    {
                        //out of camera border
                        //actually this part has little camera behavior, it's just respawning the rightplayer on the right side of the leftplayer
                    }
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
