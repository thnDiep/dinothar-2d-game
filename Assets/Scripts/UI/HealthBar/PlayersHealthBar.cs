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
        percent.text = health.ToString();
        amount.text = health.ToString();

    }

    public void setHealth(float health)
    {
        slider.value = health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
        amount.text = health.ToString();
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
