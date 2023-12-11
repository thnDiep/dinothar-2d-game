using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UIInGame : MonoBehaviour
{
    [Header("Move stage")]
    public GameObject moneyBar;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI diamondText;
    public TextMeshProUGUI lifeText;

    [Header("Fight stage")]
    public SkillBarUI skillBar1;
    public SkillBarUI skillBar2;
    public void setMoney(int amount)
    {
        if (moneyText != null)
        {
            moneyText.text = amount.ToString();
        }
    }
    public void setDiamond(int amount)
    {
        if (diamondText != null)
        {
            diamondText.text = amount.ToString();
        }
    }

    public void setLife(int amount)
    {
        if (diamondText != null)
        {
            lifeText.text = amount.ToString();
        }
    }

}
