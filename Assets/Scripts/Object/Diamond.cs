using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diamond : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] int value;
    // float moveSpeed = 0.2f;

    // int direction = 1;
    // float movingTime;
    // float movingTimer = 0.3f;

    // private void Start()
    // {
    //     movingTime = movingTimer;
    // }
    // private void Update()
    // {
    //     movingTime -= Time.deltaTime;
    //     if (movingTime < 0)
    //     {
    //         direction *= -1;
    //         movingTime = movingTimer;
    //     }
    //     transform.Translate(Vector3.up * direction * moveSpeed * Time.deltaTime);
    // }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerManager.Instance.changeDiamond(value);
            SoundManager.Instance.PlaySoundCollectMoney();
            Destroy(gameObject);
        }
    }
}
