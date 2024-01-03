using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour
{
    [SerializeField] private Image cooldownImage;
    [SerializeField] private TextMeshProUGUI cooldownText;  
    [SerializeField] private Image lockImage;

    public void Awake()
    {
        setCooldown(false);
    }

    public void Start()
    {
        //Lock(true);
    }

    public void Lock(bool isLock)
    {
        if (isLock)
        {
            lockImage.gameObject.SetActive(true);
        }
        else
        {
            lockImage.gameObject.SetActive(false);
        }
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
