using TMPro;
using UnityEngine;

public class Money : MonoBehaviour
{
    [SerializeField] PlayerManager players;
    [SerializeField] int value;
    TextMeshProUGUI moneyText;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            players.changeMoney(value);
        }
        Destroy(gameObject);
    }
}
