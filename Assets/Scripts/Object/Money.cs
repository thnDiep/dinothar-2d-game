using UnityEngine;

public class Money : MonoBehaviour
{
    PlayerManager playerManager;
    [SerializeField] GameObject coin;
    private enum money
    {
        Coin1 = 1,
        Coin2 = 2,
        Coin3 = 3
    }

    private void Start()
    {
        playerManager = GetComponent<PlayerManager>();
    }

    private void Update()
    {
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && playerManager != null)
        {
            Debug.Log("va cham");
            if (gameObject.CompareTag("Coin1"))
                playerManager.plusMoney((int)money.Coin1);

            else if (gameObject.CompareTag("Coin2"))
                playerManager.plusMoney((int)money.Coin2);

            else
                playerManager.plusMoney((int)money.Coin3);

        }
        Destroy(gameObject);
    }

}
