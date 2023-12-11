using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Life : MonoBehaviour
{
    float moveSpeed = 0.5f;

    int direction = 1;
    float movingTime;
    float movingTimer = 0.4f;

    private void Start()
    {
        movingTime = movingTimer;
    }
    private void Update()
    {
        movingTime -= Time.deltaTime;
        if (movingTime < 0)
        {
            direction *= -1;
            movingTime = movingTimer;
        }
        transform.Translate(Vector3.up * direction * moveSpeed * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerManager.Instance.changeLife(1);
            Destroy(gameObject);
        }
    }
}
