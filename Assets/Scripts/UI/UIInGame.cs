using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIInGame : MonoBehaviour
{
    public static UIInGame Instance { get; private set; }

    [Header("Move stage")]
    public GameObject bars;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI diamondText;
    public TextMeshProUGUI lifeText;
    public FillBar starBar;

    [Header("Fight stage")]
    public SkillBarUI skillBar1;
    public SkillBarUI skillBar2;

    [Header("Level complete")]
    public GameObject winningScreen;
    public GameObject losingScreen;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        setLockSkill1(!GameManager.Instance.learnedSkill1());
        setLockSkill2(!GameManager.Instance.learnedSkill2());
        setLockCombineSkill(!GameManager.Instance.learnedCombineSkill());

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

    public void setStar(int amount)
    {
        starBar.setFullNodes(amount);
    }

    public void setMoveStage()
    {
        bars.gameObject.SetActive(true);
        skillBar1.gameObject.SetActive(false);
        skillBar2.gameObject.SetActive(false);
    }

    public void setFightStage()
    {
        bars.gameObject.SetActive(false);
        skillBar1.gameObject.SetActive(true);
        skillBar2.gameObject.SetActive(true);
    }

    public void showWinningScreen()
    {
        winningScreen.SetActive(true);
    }

    public void showLosingScreen()
    {
        losingScreen.SetActive(true);
    }

    public void setLockSkill1(bool isLock)
    {
        skillBar1.singleSkill.Lock(isLock);
    }

    public void setLockSkill2(bool isLock)
    {
        skillBar2.singleSkill.Lock(isLock);
    }

    public void setLockCombineSkill(bool isLock)
    {
        skillBar1.combineSkill.Lock(isLock);
        skillBar2.combineSkill.Lock(isLock);
    }
}
