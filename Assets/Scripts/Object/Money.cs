using UnityEngine;

public class Money : MonoBehaviour
{
    [SerializeField] PlayerManager playerManager;
    [SerializeField] PlayerController Player1;
    [SerializeField] PlayerController Player2;
    [SerializeField] GameObject coin;
    private enum money
    {
        Coin1 = 1,
        Coin2 = 2,
        Coin3 = 3
    }

    private void Start()
    {
    }

    private void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Debug.Log("va cham");
            if (this.CompareTag("Coin1"))
            {

                // Debug.Log("va cham dong 1");
                playerManager.plusMoney((int)money.Coin1);
            }

            else if (this.CompareTag("Coin2"))
            {

                // Debug.Log("va cham dong 2");
                playerManager.plusMoney((int)money.Coin2);
            }
            else
            {

                // Debug.Log("va cham dong 3");
                playerManager.plusMoney((int)money.Coin3);
            }

        }
        Destroy(gameObject);
    }

}
