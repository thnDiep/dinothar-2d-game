using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightAre : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerManager.Instance.Stage = PlayerManager.PlayerStage.Fight;
            Destroy(gameObject);
        }
    }
}
