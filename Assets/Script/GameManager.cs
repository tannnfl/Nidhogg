using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    public CinemachineVirtualCamera cam0, camR1, camR2, camR3, camL1, camL2, camL3;

    public GameObject LeftPlayer;
    public GameObject RightPlayer;

    public static GOState currentGOState;

    public string currentMap;

    private Vector3 LeftPlayerRespawnPos;
    private Vector3 RightPlayerRespawnPos;


    //remember that Leftplayer always GORight, and Rightplayer always GOLeft!!!
    public enum GOState
    {
        GORight,
        GOLeft,
        NoGO,
    }

    private void Start()
    {
        currentGOState = GOState.GORight;
        print(currentGOState);
    }

    private void Update()
    {
        UpdateGOState();

        if (LeftPlayer == null)
        {
            LeftPlayer = GameObject.FindWithTag("LeftPlayer");
        }
        if (RightPlayer == null)
        {
            RightPlayer = GameObject.FindWithTag("RightPlayer");
        }

        //detect spawn points
        LeftPlayerRespawnPos.x = RightPlayer.transform.position.x - 30;
        RightPlayerRespawnPos.x = LeftPlayer.transform.position.x + 30;
    }

    private void UpdateGOState()
    {
        switch (GameManager.currentGOState)
        {
            case GameManager.GOState.GORight:
                if (LeftPlayer == null)
                {
                    LeftPlayer = GameObject.FindWithTag("LeftPlayer");
                }
                if (LeftPlayer != null)
                {
                    if (Player.isOutOfRightCameraEdge(LeftPlayer))
                    {
                        RightPlayer.GetComponent<Player>().Die(RightPlayerRespawnPos);
                    }

                }
                break;

            case GameManager.GOState.GOLeft:
                if (RightPlayer == null)
                {
                    RightPlayer = GameObject.FindWithTag("RightPlayer");
                }
                if (RightPlayer != null)
                {
                    if (Player.isOutOfLeftCameraEdge(RightPlayer))
                    {
                        LeftPlayer.GetComponent<Player>().Die(LeftPlayerRespawnPos);
                    }
                }
                break;

            case GameManager.GOState.NoGO:

                break;
        }
    }

    public void changeScene(string mapNum)
    {
        switch (mapNum)
        {
            case "map0":
                break;
            case "mapR1":
                print("transform");
                currentMap = "mapR1";
                LeftPlayer.transform.position = new Vector3(76, 7, 0);
                RightPlayer.transform.position = new Vector3(105, 5, 0);
                CameraManager.SwitchCamera(camR1);

                break;
            case "mapR2":
                break;
            case "mapR3":
                break;
            case "mapL1":
                break;
            case "mapL2":
                break;
            case "mapL3":
                break;
        }
    }

    /* if need a respawn system that need to destroy the prefab?
    public void DestroyPlayer(GameObject player)
    {
        Destroy(player);
    }

    public void RespawnPlayer(string player, float respawnTime, Vector3 spawnPos)
    {
        player = 
        transform.position = _respawnPos;
    }
    */
}

