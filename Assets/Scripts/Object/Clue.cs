using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clue : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerManager.Instance.changeClue(1);
            SoundManager.Instance.PlaySoundCollectMoney();
            Destroy(gameObject);
        }
    }
}
