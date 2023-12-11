using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;

    public enum BulletType
    {
        basic = 1,                  // 1: khi bắn bình thường
        singleSkill = 2,            // 2: khi sử dụng kỹ năng đặc biệt
        combineSkill = 3            // 3: khi sử dụng kỹ năng kết hợp
    }

    [SerializeField] private float timeExist = 1.5f;
    private int damage;
    private BulletType bulletType;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        StartCoroutine(DestroyBullet());
    }

    IEnumerator DestroyBullet()
    {
        yield return new WaitForSeconds(timeExist);
        Destroy(gameObject);
    }

    public void Launch(Vector2 direction, float force, BulletType bulletType, int damage)
    {
        anim.SetInteger("bulletType", (int)bulletType);
        transform.localScale = new Vector3(direction.x, 1, 1);
        rb.AddForce(direction * force);

        this.damage = damage;
        this.bulletType = bulletType;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        Boss boss = collider.GetComponent<Boss>();
        if (boss != null)
        {
            boss.TakeDamage(damage * (int)bulletType);
            Destroy(gameObject);
        }

        //Instantiate(impactEffect, transform.position, transform.rotation);

    }
}
