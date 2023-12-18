using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject[] players;
    private GameObject player;
    [SerializeField] float leftLimit = -29.7f;
    [SerializeField] float rightLimit = -5.7f;
    [SerializeField] float downLimit = -7.3f;
    [SerializeField] float topLimit = 21f;
    [SerializeField] float distanceToUpdateDirY = 5f;

    bool outScreen;
    float dirX, dirY;

    private bool isFightStage = false;

    void Awake()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length > 0)
        {
            player = players[0];
            if (player != null)
            {
                dirY = player.transform.position.y;
            }
        }
        else
        {
            Debug.LogError("Không tìm thấy đối tượng 'Player'.");
        }
        outScreen = false;
    }



    // Update is called once per frame
    void Update()
    {
        //if(PlayerManager.Instance.Stage == PlayerManager.PlayerStage.Move)
        //{
        if (!isFightStage)
        {

            followPlayer();
        }
        else
        {
            followPlayerFightBoss();
        }
        //}
        //else if (!isFixed)
        //{
        //    isFixed = true;
        //    transform.position = new Vector3(rightLimit, topLimit, player.transform.position.z - 20);
        //    Camera.main.orthographicSize = 4f;
        //}
    }

    private void followPlayer()
    {
        if (player != null)
        {
            if (transform.position.y - player.transform.position.y < -distanceToUpdateDirY || transform.position.y - player.transform.position.y > distanceToUpdateDirY)
            {
                outScreen = true;
            }

            if (outScreen)
            {
                dirY = player.transform.position.y;
                outScreen = false;
            }

            transform.position = new Vector3(Mathf.Clamp(player.transform.position.x, leftLimit, rightLimit), Mathf.Clamp(dirY, downLimit, topLimit), player.transform.position.z - 8);
        }
    }

    private void followPlayerFightBoss()
    {
        transform.position = new Vector3(Mathf.Clamp(player.transform.position.x + 4f, leftLimit, rightLimit), 15f, player.transform.position.z - 8);

    }

    public void setIsFightStage(bool isFight)
    {
        this.isFightStage = isFight;
    }
}
