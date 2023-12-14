using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bane_1 : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator ani;
    public float upwardForce = 0.005f;
    private BoxCollider2D boxCollider;

    [SerializeField] LayerMask playerMask1;
    [SerializeField] LayerMask playerMask2;

    // public enum PlayerState
    // {
    //     Idle,
    //     Running,
    //     Sitting,
    //     Jumping,
    //     Carrying,
    // }
    void Start()
    {
        ani = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // player bi trung vao monster
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerManager.Instance.changeLife(-1);
        }
    }

    // nhay len dau
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Rigidbody2D rigid = other.gameObject.GetComponent<Rigidbody2D>();
            if (rigid != null && checkIsJumped())
            {
                Vector2 velocity = rigid.velocity;
                velocity.y = Mathf.Sqrt(2 * 2f * Mathf.Abs(Physics2D.gravity.y));
                rigid.velocity = velocity;

                ani.SetBool("Die", true);
                StartCoroutine(DieAfterDelay(0.4f));
            }


        }
    }

    private IEnumerator DieAfterDelay(float delay)
    {

        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }


    private bool checkIsJumped()
    {
        RaycastHit2D raycastHit1 = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.up, 0.1f, playerMask1);
        RaycastHit2D raycastHit2 = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.up, 0.1f, playerMask2);

        return raycastHit1.collider != null || raycastHit2.collider != null;
    }
}