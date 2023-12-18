using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour
{
    [Header("Attack")]
    public FillBar attack;
    public GameObject attackPlus;
    private int atkIndex;
    public TextMeshProUGUI atkUpgradePrice;

    [Header("Health")]
    public FillBar health;
    public GameObject healthPlus;
    private int hpIndex;
    public TextMeshProUGUI healthUpgradePrice;

    [Header("Speed")]
    public FillBar speed;
    public GameObject speedPlus;
    private int speedIndex;
    public TextMeshProUGUI speedUpgradePrice;

    [Header("Defense")]
    public FillBar defense;
    public GameObject defensePlus;
    private int defIndex;
    public TextMeshProUGUI defUpgradePrice;

    [Header("Single Skill 1")]
    public GameObject skill1Button;
    public GameObject skill1Lock;
    public GameObject skill1Price;
    public TextMeshProUGUI skill1PriceText;

    [Header("Single Skill 2")]
    public GameObject skill2Button;
    public GameObject skill2Lock;
    public GameObject skill2Price;
    public TextMeshProUGUI skill2PriceText;

    [Header("Combine skill")]
    public GameObject combineSkillButton;
    public GameObject combineSkillLock;
    public GameObject combineSkillPrice;
    public TextMeshProUGUI combineSkillPriceText;


    void Start()
    {
        //StartCoroutine(CheckGameManagerInstance());
        HandleMoneyChanged(GameManager.Instance.getMoney());
        GameManager.MoneyChangedEvent += HandleMoneyChanged;

        atkIndex = GameManager.Instance.getAtkIndex();
        attack.setFullNodes(atkIndex);
        atkUpgradePrice.text = ((atkIndex + 1) * 100).ToString();

        hpIndex = GameManager.Instance.getHpIndex();
        health.setFullNodes(hpIndex);
        healthUpgradePrice.text = ((hpIndex + 1) * 100).ToString();

        speedIndex = GameManager.Instance.getAttackSpeedIndex();
        speed.setFullNodes(speedIndex);
        speedUpgradePrice.text = ((speedIndex + 1) * 100).ToString();

        defIndex = GameManager.Instance.getDefIndex();
        defense.setFullNodes(defIndex);
        defUpgradePrice.text = ((defIndex + 1) * 100).ToString();

        if (GameManager.Instance.learnedSkill1())
        {
            skill1Button.GetComponent<Selectable>().interactable = false;
            skill1Lock.gameObject.SetActive(false);
            skill1Price.gameObject.SetActive(false);
        }

        if (GameManager.Instance.learnedSkill2())
        {
            skill2Button.GetComponent<Selectable>().interactable = false;
            skill2Lock.gameObject.SetActive(false);
            skill2Price.gameObject.SetActive(false);
        }

        if (GameManager.Instance.learnedCombineSkill())
        {
            combineSkillButton.GetComponent<Selectable>().interactable = false;
            combineSkillLock.gameObject.SetActive(false);
            combineSkillPrice.gameObject.SetActive(false);
        }

        skill1PriceText.text = GameManager.Instance.SKILL1_PRICE.ToString();
        skill2PriceText.text = GameManager.Instance.SKILL2_PRICE.ToString();
        combineSkillPriceText.text = GameManager.Instance.COMBINE_SKILL_PRICE.ToString();
    }

    //private IEnumerator CheckGameManagerInstance()
    //{
    //    yield return new WaitForSeconds(0.1f); // Đợi một khoảng thời gian ngắn
    //    if (GameManager.Instance != null)
    //    {
    //        GameManager.MoneyChangedEvent += HandleMoneyChanged;

    //        atkIndex = GameManager.Instance.getAtkIndex();
    //        attack.setFullNodes(atkIndex);
    //        atkUpgradePrice.text = ((atkIndex + 1) * 100).ToString();

    //        hpIndex = GameManager.Instance.getHpIndex();
    //        health.setFullNodes(hpIndex);
    //        healthUpgradePrice.text = ((hpIndex + 1) * 100).ToString();

    //        speedIndex = GameManager.Instance.getAttackSpeedIndex();
    //        speed.setFullNodes(speedIndex);
    //        speedUpgradePrice.text = ((speedIndex + 1) * 100).ToString();

    //        defIndex = GameManager.Instance.getDefIndex();
    //        defense.setFullNodes(defIndex);
    //        defUpgradePrice.text = ((defIndex + 1) * 100).ToString();

    //        if (GameManager.Instance.learnedSkill1())
    //        {
    //            skill1Button.GetComponent<Selectable>().interactable = false;
    //            skill1Lock.gameObject.SetActive(false);
    //            skill1Price.gameObject.SetActive(false);
    //        }

    //        if (GameManager.Instance.learnedSkill2())
    //        {
    //            skill2Button.GetComponent<Selectable>().interactable = false;
    //            skill2Lock.gameObject.SetActive(false);
    //            skill2Price.gameObject.SetActive(false);
    //        }

    //        if (GameManager.Instance.learnedCombineSkill())
    //        {
    //            combineSkillButton.GetComponent<Selectable>().interactable = false;
    //            combineSkillLock.gameObject.SetActive(false);
    //            combineSkillPrice.gameObject.SetActive(false);
    //        }

    //        skill1PriceText.text = GameManager.Instance.SKILL1_PRICE.ToString();
    //        skill2PriceText.text = GameManager.Instance.SKILL2_PRICE.ToString();
    //        combineSkillPriceText.text = GameManager.Instance.COMBINE_SKILL_PRICE.ToString();
    //    }
    //}

    private void OnDestroy()
    {
        GameManager.MoneyChangedEvent -= HandleMoneyChanged;
    }


    private void HandleMoneyChanged(int newMoney)
    {
        if(newMoney < ((atkIndex + 1) * 100))
        {
            attackPlus.GetComponent<Selectable>().interactable = false;
        }

        if (newMoney < ((hpIndex + 1) * 100))
        {
            healthPlus.GetComponent<Selectable>().interactable = false;
        }

        if (newMoney < ((speedIndex + 1) * 100))
        {
            speedPlus.GetComponent<Selectable>().interactable = false;
        }

        if (newMoney < ((defIndex + 1) * 100))
        {
            defensePlus.GetComponent<Selectable>().interactable = false;
        }
    }

    public void UpgradeAttack()
    {
        atkIndex += 1;
        int price = atkIndex * 100;
        bool isSuccess = GameManager.Instance.UpgradeAttack(atkIndex, price);
        if (!isSuccess)
            return;

        int nextPrice = (atkIndex + 1) * 100;
        if (atkIndex >= attack.nodes.Length)
        {
            attackPlus.GetComponent<Selectable>().interactable = false;
        }
        attack.setFullNodes(atkIndex);
        atkUpgradePrice.text = nextPrice.ToString();
    }

    public void UpgradeHealth()
    {
        hpIndex += 1;
        int price = hpIndex * 100;
        bool isSuccess = GameManager.Instance.UpgradeHealth(hpIndex, price);
        if (!isSuccess)
            return;

        int nextPrice = (hpIndex + 1) * 100;
        if (hpIndex >= health.nodes.Length)
        {
            healthPlus.GetComponent<Selectable>().interactable = false;
        }
        health.setFullNodes(hpIndex);
        healthUpgradePrice.text = nextPrice.ToString();
    }

    public void UpgradeSpeed()
    {
        speedIndex += 1;
        int price = speedIndex * 100;
        bool isSuccess = GameManager.Instance.UpgradeSpeed(speedIndex, price);
        if (!isSuccess)
            return;

        int nextPrice = (speedIndex + 1) * 100;
        if (speedIndex >= speed.nodes.Length)
        {
            speedPlus.GetComponent<Selectable>().interactable = false;
        }
        speed.setFullNodes(speedIndex);
        speedUpgradePrice.text = nextPrice.ToString();
    }

    public void UpgradeDefense()
    {
        defIndex += 1;
        int price = defIndex * 100;
        bool isSuccess = GameManager.Instance.UpgradeDefense(defIndex, price);
        if (!isSuccess)
            return;

        int nextPrice = (defIndex + 1) * 100;
        if (defIndex >= defense.nodes.Length)
        {
            defensePlus.GetComponent<Selectable>().interactable = false;
        }
        defense.setFullNodes(defIndex);
        defUpgradePrice.text = nextPrice.ToString();
    }

    public void LearnSkill1()
    {
        bool isSuccess = GameManager.Instance.UnlockSkill1();
        if(!isSuccess)
            return;

        skill1Button.GetComponent<Selectable>().interactable = false;
        skill1Lock.gameObject.SetActive(false);
        skill1Price.gameObject.SetActive(false);
        //UIInGame.Instance.setLockSkill1(false);
    }

    public void LearnSkill2()
    {
        bool isSuccess = GameManager.Instance.UnlockSkill2();
        if (!isSuccess)
            return;

        skill2Button.GetComponent<Selectable>().interactable = false;
        skill2Lock.gameObject.SetActive(false);
        skill2Price.gameObject.SetActive(false);
        //UIInGame.Instance.setLockSkill2(false);
    }

    public void LearnCombineSkill()
    {
        bool isSuccess = GameManager.Instance.UnlockCombineSkill();
        if (!isSuccess)
            return;

        combineSkillButton.GetComponent<Selectable>().interactable = false;
        combineSkillLock.gameObject.SetActive(false);
        combineSkillPrice.gameObject.SetActive(false);
        //UIInGame.Instance.setLockCombineSkill(false);
    }
}
