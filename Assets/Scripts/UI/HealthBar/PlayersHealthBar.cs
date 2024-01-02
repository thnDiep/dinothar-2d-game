using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayersHealthBar : MonoBehaviour
{
    // Start is called before the first frame update
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public TextMeshProUGUI amount;
    public TextMeshProUGUI percent;

    public Image life1, life2, life3, life4, life5;


    public void setMaxHealth(float health)
    {
        slider.maxValue = health;
        slider.value = health;
        fill.color = gradient.Evaluate(1f);
        percent.text = Mathf.RoundToInt(health).ToString();
        amount.text = Mathf.RoundToInt(health).ToString();

    }

    public void setHealth(float health)
    {
        slider.value = health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
        amount.text = Mathf.RoundToInt(health).ToString();
    }

    public void setLife(int lifeNumber)
    {
        if(lifeNumber == 5)
        {
            life5.gameObject.SetActive(true);
            life4.gameObject.SetActive(true);
            life3.gameObject.SetActive(true);
            life2.gameObject.SetActive(true);
            life1.gameObject.SetActive(true);
        }
        else if(lifeNumber == 4)
        {
            life5.gameObject.SetActive(false);
            life4.gameObject.SetActive(true);
            life3.gameObject.SetActive(true);
            life2.gameObject.SetActive(true);
            life1.gameObject.SetActive(true);
        }
        else if(lifeNumber == 3)
        {
            life5.gameObject.SetActive(false);
            life4.gameObject.SetActive(false);
            life3.gameObject.SetActive(true);
            life2.gameObject.SetActive(true);
            life1.gameObject.SetActive(true);
        }
        else if (lifeNumber == 2)
        {
            life5.gameObject.SetActive(false);
            life4.gameObject.SetActive(false);
            life3.gameObject.SetActive(false);
            life2.gameObject.SetActive(true);
            life1.gameObject.SetActive(true);
        }
        else if (lifeNumber == 1)
        {
            life5.gameObject.SetActive(false);
            life4.gameObject.SetActive(false);
            life3.gameObject.SetActive(false);
            life2.gameObject.SetActive(false);
            life1.gameObject.SetActive(true);
        }
        else if (lifeNumber == 0)
        {
            life5.gameObject.SetActive(false);
            life4.gameObject.SetActive(false);
            life3.gameObject.SetActive(false);
            life2.gameObject.SetActive(false);
            life1.gameObject.SetActive(false);
        }
    }
    public void loseLife(int LifeNumber)
    {
        if (LifeNumber == 4 && life5 != null)
        {
            life5.gameObject.SetActive(false);
        }
        else if (LifeNumber == 3 && life4 != null)
        {
            life4.gameObject.SetActive(false);
        }
        else if (LifeNumber == 2 && life3 != null)
        {
            life3.gameObject.SetActive(false);
        }
        else if (LifeNumber == 1 && life2 != null)
        {
            life2.gameObject.SetActive(false);
        }
    }
}
