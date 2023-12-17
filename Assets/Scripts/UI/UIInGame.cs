using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class UIInGame : MonoBehaviour
{
    public static UIInGame Instance { get; private set; }

    //[Header("Move stage")]
    [Header("Stage")]
    public GameObject MoveStageUI;
    public GameObject FightStageUI;

    [Header("Bars")]
    public FillBar starBar;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI diamondText;
    public TextMeshProUGUI lifeText;

    [Header("Skill")]
    public SkillBarUI skillBar1;
    public SkillBarUI skillBar2;

    [Header("Health bars")]
    public PlayersHealthBar playerHealthBar;
    public BossHealthBar bossHealthBar;

    [Header("Button")]
    public ClueCollecitonBtn clueCollectionBtn;

    [Header("Level complete")]
    public GameObject winningScreen;
    public GameObject losingScreen;

    public ScreenFader screenFader;

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
        StartCoroutine(CheckGameManagerInstance());
        
        winningScreen.SetActive(false);
        losingScreen.SetActive(false);

        PlayerManager.HealthChangeEvent += playerHealthBar.setHealth;
        Boss.HealthChangeEvent += bossHealthBar.setHealth;
    }

    private IEnumerator CheckGameManagerInstance()
    {
        yield return new WaitForSeconds(0.1f); // Đợi một khoảng thời gian ngắn
        if (GameManager.Instance != null)
        {
            setLockSkill1(!GameManager.Instance.learnedSkill1());
            setLockSkill2(!GameManager.Instance.learnedSkill2());
            setLockCombineSkill(!GameManager.Instance.learnedCombineSkill());
        }
    }

    private void OnDestroy()
    {
        PlayerManager.HealthChangeEvent -= playerHealthBar.setHealth;
        Boss.HealthChangeEvent -= bossHealthBar.setHealth;
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

    public void updateLife(int amount)
    {
        setLife(amount);
        playerHealthBar.loseLife(amount);
    }

    public void setStage(bool isFightStage)
    {
        MoveStageUI.gameObject.SetActive(!isFightStage);
        FightStageUI.gameObject.SetActive(isFightStage);
    }

    public void showWinningScreen()
    {
        FightStageUI.SetActive(false);
        winningScreen.SetActive(true);
    }

    public void showLosingScreen()
    {
        FightStageUI.SetActive(false);
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

    public void startScreenFade()
    {
        screenFader.gameObject.SetActive(true);
        screenFader.StartScreenFade();
    }
}
