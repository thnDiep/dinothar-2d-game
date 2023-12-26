//using TMPro;
//using UnityEngine;

//public class MoneySpawner : MonoBehaviour
//{
//    public GameObject moneyPrefabs; // Kéo và thả Prefab của đồng xu từ UI trong Inspector
//    public int numberOfMoney = 5; // Số lượng đồng xu xuất hiện

//    public void SpawnMoney(Vector3 position)
//    {
//        for (int i = 0; i < numberOfMoney; i++)
//        {
//            // Tạo một đồng xu mới
//            GameObject coin = Instantiate(moneyPrefabs, position, Quaternion.identity);

//            // Áp dụng một lực ngẫu nhiên để đồng xu văng đi
//            Rigidbody2D coinRb = coin.GetComponent<Rigidbody2D>();
//            coinRb.AddForce(new Vector2(Random.Range(-5f, 5f), Random.Range(5f, 10f)), ForceMode2D.Impulse);
//        }
//    }
//}
