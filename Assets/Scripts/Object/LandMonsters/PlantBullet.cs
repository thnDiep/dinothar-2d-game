using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlantBullet : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D rb;
    [SerializeField] private float timeExist = 0.5f;
    Vector2 startPosition;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = new Vector2(0, 0);

        StartCoroutine(DestroyBullet());
    }

    public void Launch(Vector2 direction, float force, Vector2 startPos)
    {
        transform.localScale = new Vector3(direction.x, 1, 1);
        Debug.Log("direction: " + direction + "force: " + force);
        rb.AddForce(direction * force);
        startPosition = startPos;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // PlayerController playerController = other.GetComponent<PlayerController>();

        // if (playerController != null)
        // {
        //     PlayerManager.Instance.changeLife(-1);
        //     Destroy(gameObject);
        // }

        if (other != null)
        {

            if (other.CompareTag("Player"))
            {
                // PlayerManager.Instance.changeLife(-1);
                other.GetComponent<PlayerController>().Hurt();
                Destroy(gameObject);
            }
            else if (other.CompareTag("Ground"))
            {
                Destroy(gameObject);

            }
        }
    }

    IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(timeExist);
        Destroy(gameObject);
    }
}
