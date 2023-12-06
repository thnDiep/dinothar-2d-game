using TMPro;
using UnityEngine;

public class Money : MonoBehaviour
{
    [SerializeField] int value;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerManager.Instance.changeMoney(value);
        }
        Destroy(gameObject);
    }
}
