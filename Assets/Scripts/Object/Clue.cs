using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clue : MonoBehaviour
{
    public int value;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerManager.Instance.collectClue(value);
            SoundManager.Instance.PlaySoundCollectMoney();
            Destroy(gameObject);
        }
    }
}
