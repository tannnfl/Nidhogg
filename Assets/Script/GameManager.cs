using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    public CinemachineVirtualCamera cam0, camR1, camR2, camR3, camL1, camL2, camL3;
  
    public PolygonCollider2D map0, mapR1, mapR2, mapR3, mapL1, mapL2, mapL3;

    public GameObject LeftPlayer;
    public GameObject RightPlayer;

    public GameObject LeftGOArrow;
    public GameObject RightGOArrow;

    public static GOState currentGOState;

    public string currentMap;

    //changing spawn points when die
    public static Vector3 LeftPlayerRespawnPos;
    public static Vector3 RightPlayerRespawnPos;

    private float mapLeftEdgeX = float.MaxValue;
    private float mapRightEdgeX = float.MinValue;

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
        currentMap = "map0";

    }

    private void Update()
    {
        UpdateScene();
        UpdateGOState();
        UpdateRespawnPos();
    }

    private void UpdateScene()
    {
        switch (currentMap)
        {
            case "map0":
                mapLeftEdgeX = float.MaxValue;
                mapRightEdgeX = float.MinValue;
                checkForMapEdges(map0);
                break;

            case "mapR1":
                mapLeftEdgeX = float.MaxValue;
                mapRightEdgeX = float.MinValue;
                checkForMapEdges(mapR1);

                break;
            case "mapR2":
                mapLeftEdgeX = float.MaxValue;
                mapRightEdgeX = float.MinValue;
                checkForMapEdges(mapR2);
                break;
            case "mapR3":
                mapLeftEdgeX = float.MaxValue;
                mapRightEdgeX = float.MinValue;
                checkForMapEdges(mapR3);
                break;
            case "mapL1":
                break;
            case "mapL2":
                break;
            case "mapL3":
                break;

        }
    }

    private void UpdateGOState()
    {
        switch (GameManager.currentGOState)
        {
            case GameManager.GOState.GORight:

                LeftGOArrow.SetActive(false);
                RightGOArrow.SetActive(true);

                if (LeftPlayer == null)
                {
                    LeftPlayer = GameObject.FindWithTag("LeftPlayer");
                }
                if (LeftPlayer != null)
                {
                    if ((Player.isOutOfRightCameraEdge(LeftPlayer)) && (RightPlayer.GetComponent<Player>().canRespawn))
                    {
                        RightPlayer.GetComponent<Player>().Die(RightPlayerRespawnPos);
                    }

                }

                if (RightPlayer == null)
                {
                    RightPlayer = GameObject.FindWithTag("RightPlayer");
                }
                if (RightPlayer != null)
                {

                }

                break;

            case GameManager.GOState.GOLeft:

                LeftGOArrow.SetActive(true);
                RightGOArrow.SetActive(false);

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

                if (LeftPlayer == null)
                {
                    LeftPlayer = GameObject.FindWithTag("LeftPlayer");
                }
                if (LeftPlayer != null)
                {

                }

                break;

            case GameManager.GOState.NoGO:
                LeftGOArrow.SetActive(false);
                RightGOArrow.SetActive(false);
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
                currentMap = "mapR1";
                RightPlayer.GetComponent<Player>().canRespawn = false;
                //Need to have different spawn positions for different maps
                LeftPlayer.transform.position = new Vector3(70, 7, 0);
                RightPlayer.transform.position = new Vector3(99, 5, 0);
                CameraManager.SwitchCamera(camR1);

                break;
            case "mapR2":
                currentMap = "mapR2";
                RightPlayer.GetComponent<Player>().canRespawn = false;
                //Need to have different spawn positions for different maps
                LeftPlayer.transform.position = new Vector3(140, 7, 0);
                RightPlayer.transform.position = new Vector3(170, 5, 0);
                CameraManager.SwitchCamera(camR2);
                break;
            case "mapR3":
                currentMap = "mapR3";
                RightPlayer.GetComponent<Player>().canRespawn = false;
                //Need to have different spawn positions for different maps
                LeftPlayer.transform.position = new Vector3(238, 7, 0);
                //remove player right at this point
                CameraManager.SwitchCamera(camR3);
                break;
            case "mapL1":
                break;
            case "mapL2":
                break;
            case "mapL3":
                break;
        }
    }

    private void UpdateRespawnPos()
    {
        //respawning player, need revisions and tuning, check Nidhogg original game
        if(RightPlayer.transform.position.x-40 > mapLeftEdgeX)
        {
            LeftPlayer.GetComponent<Player>().canRespawn = true;
            LeftPlayerRespawnPos.x = RightPlayer.transform.position.x - 20;
        }
        else
        {
            LeftPlayerRespawnPos.x = RightPlayer.transform.position.x + 20;
        }

        if (LeftPlayer.transform.position.x + 40 < mapRightEdgeX)
        {
            RightPlayer.GetComponent<Player>().canRespawn = true;
            RightPlayerRespawnPos.x = LeftPlayer.transform.position.x + 20;
        }
        else
        {
            RightPlayerRespawnPos.x = LeftPlayer.transform.position.x - 20;
        }
    }

    private void checkForMapEdges(PolygonCollider2D map)
    {
        for (int i = 0; i < map.points.Length; i++)
        {
            Vector2 pointPos = map.points[i];
            if (pointPos.x < mapLeftEdgeX)
            {
                mapLeftEdgeX = pointPos.x;
            }
            if (pointPos.x > mapRightEdgeX)
            {
                mapRightEdgeX = pointPos.x;
            }
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

        private void DeathAndRespawn()
    {
        if ((LeftPlayer == null) && Input.GetKey(KeyCode.Space))
        {
            newLeftPlayer = Instantiate(pinball, pinballStartPos.transform.position, pinballStartPos.transform.rotation);
            newBall.GetComponent<Rigidbody2D>().AddForce(transform.up * thrust, ForceMode2D.Impulse);
            OnePinballExists = true;
            ball_script = newBall.GetComponent<BallScript>();
        }

        if (OnePinballExists && ball_script.lose)
        {
            Destroy(newBall);
            OnePinballExists = false;
            if (hp > 0)
            {
                hp -= 1;
                UpdateHeartUI();

            }
        }
    }
    */
}

