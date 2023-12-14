using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spiky_3 : MonoBehaviour
{


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerManager.Instance.changeLife(-1);
        }
    }
}
