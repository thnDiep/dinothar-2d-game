using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour
{
    [Header("Attack")]
    public FillBar attack;
    public GameObject attackPlus;
    private int atkIndex;

    [Header("Health")]
    public FillBar health;
    public GameObject healthPlus;
    private int hpIndex;

    [Header("Speed")]
    public FillBar speed;
    public GameObject speedPlus;
    private int speedIndex;

    [Header("Defense")]
    public FillBar defense;
    public GameObject defensePlus;
    private int defIndex;

    void Start()
    {
        atkIndex = (int) PlayerManager.Instance.ATK / 10;
        hpIndex = (int) PlayerManager.Instance.HP / 100;
        speedIndex = (int) (PlayerManager.Instance.ATTACK_SPEED - 100) / 100;
        defIndex = (int) PlayerManager.Instance.DEF / 10;

        attack.setFullNodes(atkIndex);
        health.setFullNodes(hpIndex);
        speed.setFullNodes(speedIndex);
        defense.setFullNodes(defIndex);
    }


    public void UpgradeAttack()
    {
        atkIndex += 1;
        if(atkIndex >= attack.nodes.Length)
        {
            attackPlus.GetComponent<Selectable>().interactable = false;
        }
        attack.setFullNodes(atkIndex);
        PlayerManager.Instance.UpgradeAttack(atkIndex);
    }

    public void UpgradeHealth()
    {
        hpIndex += 1;
        if (hpIndex >= health.nodes.Length)
        {
            healthPlus.GetComponent<Selectable>().interactable = false;
        }
        health.setFullNodes(hpIndex);
        PlayerManager.Instance.UpgradeHealth(hpIndex);
    }

    public void UpgradeSpeed()
    {
        speedIndex += 1;
        if (speedIndex >= speed.nodes.Length)
        {
            speedPlus.GetComponent<Selectable>().interactable = false;
        }
        speed.setFullNodes(speedIndex);
        PlayerManager.Instance.UpgradeSpeed(speedIndex);
    }

    public void UpgradeDefense()
    {
        defIndex += 1;
        if (defIndex >= defense.nodes.Length)
        {
            defensePlus.GetComponent<Selectable>().interactable = false;
        }
        defense.setFullNodes(defIndex);
        PlayerManager.Instance.UpgradeDefense(defIndex);
    }
}
