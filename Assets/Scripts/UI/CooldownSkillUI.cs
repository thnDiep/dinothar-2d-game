using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CooldownSkillUI : MonoBehaviour
{
    [SerializeField] private Image cooldownImage;
    [SerializeField] private TextMeshProUGUI cooldownText;

    public void Start()
    {
        setCooldown(false);
    }

    public void applyCooldown(float remainingTime, float cooldownTime, bool isCooldown)
    {
        cooldownText.text = Mathf.RoundToInt(remainingTime).ToString();

        if(isCooldown)
            cooldownImage.fillAmount = remainingTime / cooldownTime;
        else
            cooldownImage.fillAmount = 1.0f - remainingTime / cooldownTime;

    }

    public void setCooldown(bool isUsing)
    {
        if(isUsing)
        {
            cooldownText.gameObject.SetActive(true);
            cooldownImage.fillAmount = 1.0f;
        } else
        {
            cooldownText.gameObject.SetActive(false);
            cooldownImage.fillAmount = 0.0f;
        }
    }
}
