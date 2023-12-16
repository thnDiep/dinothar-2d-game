using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIInGame : MonoBehaviour
{ 
    [Header("Move stage")]
    public GameObject bars;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI diamondText;
    public TextMeshProUGUI lifeText;
    //public Image currentStarImg;
    //public Sprite starImg1, starImg2, starImg3;
    public FillBar starBar;

    [Header("Fight stage")]
    public SkillBarUI skillBar1;
    public SkillBarUI skillBar2;

    [Header("Level complete")]
    public GameObject winningScreen;
    public GameObject losingScreen;

    private void Start()
    {
        winningScreen.SetActive(false);
        losingScreen.SetActive(false);
    }

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
    public void showWinningScreen()
    {
        winningScreen.SetActive(true);
    }

    public void showLosingScreen()
    {
        losingScreen.SetActive(true);
    }

    //public void setStar(int numberStar)
    //{
    //    if (numberStar == 1)
    //    {
    //        currentStarImg.sprite = starImg1;
    //    }
    //    else if (numberStar == 2)
    //    {
    //        currentStarImg.sprite = starImg2;
    //    }
    //    else
    //    {
    //        currentStarImg.sprite = starImg3;
    //    }
    //}
}
