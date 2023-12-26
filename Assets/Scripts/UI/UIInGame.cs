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
        //StartCoroutine(CheckGameManagerInstance());
        setUnlockSkill1(GameManager.Instance.learnedSkill1());
        setUnlockSkill2(GameManager.Instance.learnedSkill2());
        setUnlockCombineSkill(GameManager.Instance.learnedCombineSkill());

        winningScreen.SetActive(false);
        losingScreen.SetActive(false);

        PlayerManager.HealthChangeEvent += playerHealthBar.setHealth;
        Boss.HealthChangeEvent += bossHealthBar.setHealth;
        GameManager.Skill1ChangedEvent += setUnlockSkill1;
        GameManager.Skill2ChangedEvent += setUnlockSkill2;
        GameManager.CombineSkillChangedEvent += setUnlockCombineSkill;
    }

    //private IEnumerator CheckGameManagerInstance()
    //{
    //    yield return new WaitForSeconds(0.1f); // Đợi một khoảng thời gian ngắn
    //    if (GameManager.Instance != null)
    //    {
    //        setUnlockSkill1(GameManager.Instance.learnedSkill1());
    //        setUnlockSkill2(GameManager.Instance.learnedSkill2());
    //        setUnlockCombineSkill(GameManager.Instance.learnedCombineSkill());
    //    }
    //}

    private void OnDestroy()
    {
        PlayerManager.HealthChangeEvent -= playerHealthBar.setHealth;
        Boss.HealthChangeEvent -= bossHealthBar.setHealth;
        GameManager.Skill1ChangedEvent -= setUnlockSkill1;
        GameManager.Skill2ChangedEvent -= setUnlockSkill2;
        GameManager.CombineSkillChangedEvent -= setUnlockCombineSkill;
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

    public void setUnlockSkill1(bool isLearned)
    {
        skillBar1.singleSkill.Lock(!isLearned);
    }

    public void setUnlockSkill2(bool isLearned)
    {
        skillBar2.singleSkill.Lock(!isLearned);
    }

    public void setUnlockCombineSkill(bool isLearned)
    {
        skillBar1.combineSkill.Lock(!isLearned);
        skillBar2.combineSkill.Lock(!isLearned);
    }

    public void startScreenFade()
    {
        screenFader.gameObject.SetActive(true);
        screenFader.StartScreenFade();
    }
}
