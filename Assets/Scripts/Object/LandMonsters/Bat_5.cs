using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Bat_5 : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator ani;
    // private BoxCollider2D boxCollider;
    // [SerializeField] LayerMask groundLayer;

    public float speed = 1.5f;
    Rigidbody2D rg2d;
    int direction = 1;
    float movingTimer;
    public float movingTime = 4f;
    private bool die;
    Vector2 position;
    Vector3 scale;

    void Start()
    {
        ani = GetComponent<Animator>();
        rg2d = GetComponent<Rigidbody2D>();
        // boxCollider = GetComponent<BoxCollider2D>();
        movingTimer = movingTime;
        die = false;

        scale = transform.localScale;
        scale.x = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (!die)
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
    }

    private void FixedUpdate()
    {


        // Đảo ngược giá trị của trục X


        if (!die)
        {
            position = rg2d.position;
            position.x += speed * Time.deltaTime * direction;

            rg2d.MovePosition(position);
        }
        playAnimation();
    }

    private void playAnimation()
    {

        if (die)
        {
            ani.SetBool("Die", true);
        }

    }

    public void isAttacked()
    {
        die = true;
        StartCoroutine(DieAfterDelay(0.9f));
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

}
