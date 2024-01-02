using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossHealthBar : MonoBehaviour
{
    // Start is called before the first frame update
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public TextMeshProUGUI amount;
    public TextMeshProUGUI percent;

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
}
