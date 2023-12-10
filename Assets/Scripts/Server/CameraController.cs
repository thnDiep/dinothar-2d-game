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
    private float leftLimit = -7;
    float rightLimit = 14;
    float downLimit = -4f;
    float topLimit = 10;
    float distanceToUpdateDirY = 7.5f;

    bool outScreen;
    float dirX, dirY;

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

            transform.position = new Vector3(Mathf.Clamp(player.transform.position.x, leftLimit, rightLimit), Mathf.Clamp(dirY, downLimit, topLimit), player.transform.position.z - 20);
        }

    }
}
