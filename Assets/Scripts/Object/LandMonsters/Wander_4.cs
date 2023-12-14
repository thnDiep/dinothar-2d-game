using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Wander_4 : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator ani;
    // private BoxCollider2D boxCollider;
    // [SerializeField] LayerMask groundLayer;

    public float speed = 0.8f;
    Rigidbody2D rg2d;
    int direction = 1;
    float movingTimer;
    public float movingTime = 1.5f;
    private bool hurt, die;
    Vector2 position;

    float hurtTimer;
    public float hurtTime = 2f;
    Vector3 scale;

    void Start()
    {
        ani = GetComponent<Animator>();
        rg2d = GetComponent<Rigidbody2D>();
        // boxCollider = GetComponent<BoxCollider2D>();
        movingTimer = movingTime;

        hurt = false;
        die = false;

        hurtTimer = hurtTime;
        scale = transform.localScale;
        scale.x = -1;
    }

    // Update is called once per frame
    void Update()
    {
        if (!hurt & !die)
        {
            movingTimer -= Time.deltaTime;
            if (movingTimer < 0)
            {
                direction *= -1;
                movingTimer = movingTime;
                scale.x *= -1;

                // Cập nhật localScale
                transform.localScale = scale;
            }
        }

        if (hurt)
        {
            hurtTimer -= Time.deltaTime;
            if (hurtTimer < 0)
            {
                hurtTimer = hurtTime;
                hurt = false;
            }
        }
    }

    private void FixedUpdate()
    {


        // Đảo ngược giá trị của trục X


        if (!hurt & !die)
        {

            position = rg2d.position;
            position.x += speed * Time.deltaTime * direction;

            rg2d.MovePosition(position);
            // if (!isGrounded())
            // {
            //     direction *= -1;
            //     movingTimer = movingTime;
            //     scale.x *= -1;

            //     // Cập nhật localScale
            //     transform.localScale = scale;
            // }
        }
        playAnimation();
    }

    private void playAnimation()
    {
        if (!hurt)
        {
            ani.SetBool("Idle", true);
            ani.SetBool("Hurt", false);
        }
        else
        {
            ani.SetBool("Hurt", true);
            ani.SetBool("Idle", false);
        }
        if (die)
        {
            ani.SetBool("Die", true);
        }
    }

    public void isAttacked()
    {
        if (!hurt)
        {
            hurt = true;
        }
        else
        {
            die = true;
            StartCoroutine(DieAfterDelay(0.9f));
        }

    }

    private IEnumerator DieAfterDelay(float delay)
    {

        yield return new WaitForSeconds(delay);
        if (die)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other != null && other.gameObject.CompareTag("Player"))
        {
            PlayerManager.Instance.changeLife(-1);
        }
    }

    // public bool isGrounded()
    // {
    //     RaycastHit2D groundLayerRaycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);

    //     return groundLayerRaycastHit.collider != null;
    // }

}
