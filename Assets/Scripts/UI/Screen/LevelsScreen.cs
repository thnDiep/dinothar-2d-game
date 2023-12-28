using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelsScreen : MonoBehaviour
{
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI diamondText;

    public Image[] levelImages;
    public GameObject[] levelButtons;
    public FillBar[] levelStars;
    public Image[] level1Planets;
    public Image[] level2Planets;
    public Image[] level3Planets;
    public Image[] level4Planets;
    public Image[] level5Planets;

    void Start()
    {
        setMoney(GameManager.Instance.getMoney());
        setDiamond(GameManager.Instance.getDiamond());
        setLevelStar(GameManager.Instance.getStars());

        GameManager.MoneyChangedEvent += setMoney;
        GameManager.DiamondChangedEvent += setDiamond;
    }

    public void setLevelStar(int[] stars)
    {
        bool isFirstLockLevel = false;

        Image[][] planetImages = { level1Planets, level2Planets, level3Planets, level4Planets, level5Planets };

        for (int i = 0; i < stars.Length; i++)
        {
            if (stars[i] == -1)
            {
                levelStars[i].gameObject.SetActive(false);
                if (!isFirstLockLevel)
                {
                    isFirstLockLevel = true;
                    changeAlpha(levelImages[i], 1.0f);
                    levelButtons[i].GetComponent<Selectable>().interactable = true;
                    foreach (Image planet in planetImages[i])
                    {
                        changeAlpha(planet, 1.0f);
                    }
                }
                else
                {
                    changeAlpha(levelImages[i], 0.76f);
                    levelButtons[i].GetComponent<Selectable>().interactable = false;
                    foreach (Image planet in planetImages[i])
                    {
                        changeAlpha(planet, 0.76f);
                    }
                }
            }
            else
            {
                changeAlpha(levelImages[i], 1.0f);
                foreach (Image planet in planetImages[i])
                {
                    changeAlpha(planet, 1.0f);
                }
                levelStars[i].gameObject.SetActive(true);
                levelStars[i].setFullNodes(stars[i]);
            }
        }
    }

    public void changeAlpha(Image image, float alpha)
    {
        Color currentColor = image.color;
        currentColor.a = alpha;
        image.color = currentColor;
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
}