using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Pillar_2 : MonoBehaviour
{
    // Start is called before the first frame update
    private CapsuleCollider2D capsuleCollider;
    private float changeTime;
    private float changeTimer = 0.495f;
    float tallHeight = 1f;
    float shortHeight = 0.6f;
    float currentHeight;
    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        changeTime = changeTimer;
        currentHeight = shortHeight;
    }

    // Update is called once per frame
    void Update()
    {
        changeTime -= Time.deltaTime;
        if (changeTime < 0)
        {
            changeTime = changeTimer;
            if (currentHeight == tallHeight)
            {
                currentHeight = shortHeight;
            }
            else
            {
                currentHeight = tallHeight;
            }
            float lerpValue = changeTime / changeTimer;
            float newSizeY = Mathf.Lerp(capsuleCollider.size.y, currentHeight, lerpValue);
            capsuleCollider.size = new Vector2(capsuleCollider.size.x, newSizeY);

        }


    }
}
