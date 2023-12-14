using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Plant_6 : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject plantBulletPrefab;
    private Rigidbody2D rg2d;
    float shootTimer, unShootTimer;
    float shootTime = 3f, unShootTime = 2f;
    private float countDownShootTime = 1f;
    float attackSpeed = 300f;
    bool shooting = true;


    bool canShoot = true;
    Vector2 direction;

    void Start()
    {
        rg2d = GetComponent<Rigidbody2D>();
        direction = new Vector2(-1, 0);
        shootTimer = shootTime;
        unShootTimer = unShootTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (shooting)
        {
            if (canShoot)
                Shoot();
            shootTimer -= Time.deltaTime;
            if (shootTimer < 0)
            {
                shootTimer = shootTime;
                shooting = false;
            }
        }
        else
        {
            unShootTimer -= Time.deltaTime;
            if (unShootTimer < 0)
            {
                unShootTimer = unShootTime;
                shooting = true;
            }
        }
    }

    private void Shoot()
    {
        GameObject plantBulletObject = Instantiate(plantBulletPrefab, rg2d.position + direction * 0.5f, Quaternion.identity);
        PlantBullet plantBullet = plantBulletObject.GetComponent<PlantBullet>();
        plantBullet.Launch(direction, 300f, transform.position);

        StartCoroutine(SetCanShoot());
    }

    IEnumerator SetCanShoot()
    {
        canShoot = false;
        yield return new WaitForSeconds(countDownShootTime);
        canShoot = true;
    }
}
