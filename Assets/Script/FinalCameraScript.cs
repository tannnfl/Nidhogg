using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class FinalCameraScript : MonoBehaviour
{

    //Search for an active LeftPlayer and an active RightPlayer
    private GameObject Player;

    //Update the followtarget for the virtual cam
    private Transform FinalfollowTarget;


    private GameManager GameManager;

    //get virtual cam and cam width
    private CinemachineVirtualCamera playerFinalCam;
    private Camera camFinal;



    public void Start()
    {
        playerFinalCam = GetComponent<CinemachineVirtualCamera>();

        camFinal = Camera.main;
    }

    public void Update()
    {
        UpdateFollowTarget();

        playerFinalCam.Follow = FinalfollowTarget;
        playerFinalCam.LookAt = FinalfollowTarget;
        
        Vector3 cameraPosition = playerFinalCam.transform.position;
        playerFinalCam.transform.position = cameraPosition;


    }
    void UpdateFollowTarget()
    {
        GameManager gameManager = GameObject.FindObjectOfType<GameManager>();
        // Check the current map and set the follow target accordingly
        if (gameManager.currentMap == "mapR3")
        {
            Player = GameObject.FindWithTag("LeftPlayer");
            FinalfollowTarget = Player.transform;
        }
        else if (gameManager.currentMap == "mapL3")
        {
            Player = GameObject.FindWithTag("RightPlayer");
            FinalfollowTarget = Player.transform;
        }
    }
    private void OnEnable()
    {
        CameraManager.Register(GetComponent<CinemachineVirtualCamera>());
    }

    private void OnDisable()
    {
        CameraManager.Unregister(GetComponent<CinemachineVirtualCamera>());
    }

}
