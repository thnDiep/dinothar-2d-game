using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diamond : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] int value;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerManager.Instance.changeDiamond(value);
            Destroy(gameObject);
        }
    }
}
