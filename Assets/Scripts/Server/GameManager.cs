using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using static PlayerManager;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private PlayerData playerData;

    public static event Action<int> CurrentLevelChangedEvent;
    public static event Action<int> MoneyChangedEvent;
    public static event Action<int> DiamondChangedEvent;
    public static event Action<int[]> ClueChangedEvent;

    public static event Action<bool> Skill1ChangedEvent;
    public static event Action<bool> Skill2ChangedEvent;
    public static event Action<bool> CombineSkillChangedEvent;

    public int DEFAULT_HP = 100;
    public int DEFAULT_ATK = 10;
    public int DEFAULT_ATTACK_SPEED = 50;
    public int DEFAULT_DEF = 1;

    public int SKILL1_PRICE = 20;
    public int SKILL2_PRICE = 20;
    public int COMBINE_SKILL_PRICE = 30;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        LoadPlayerData();
    }

    void SavePlayerData()
    {
        PlayerPrefs.SetString("PlayerData", JsonUtility.ToJson(playerData));
        PlayerPrefs.Save();

        CurrentLevelChangedEvent?.Invoke(playerData.currentLevel);
        MoneyChangedEvent?.Invoke(playerData.money);
        DiamondChangedEvent?.Invoke(playerData.diamond);
        Skill1ChangedEvent?.Invoke(playerData.singleSkill1);
        Skill2ChangedEvent?.Invoke(playerData.singleSkill2);
        CombineSkillChangedEvent?.Invoke(playerData.combineSkill);
    }

    void LoadPlayerData()
    {
        // Đọc dữ liệu người chơi từ PlayerPrefs
        string savedData = PlayerPrefs.GetString("PlayerData", "");

        if (!string.IsNullOrEmpty(savedData))
        {
            playerData = JsonUtility.FromJson<PlayerData>(savedData);
            Debug.Log("Player Data Loaded");
        }
        else
        {
            playerData = new PlayerData();
            Debug.Log("New Player Data Created");
        }
    }

    public void NewGame()
    {
        PlayerPrefs.DeleteKey("PlayerData");
        LoadPlayerData();
    }

    public void LevelCompleted(int level, int money, int diamond, int clue)
    {
        playerData.level = level;

        // Lấy số sao lớn nhất
        if (clue > playerData.stars[level - 1])
            playerData.stars[level - 1] = clue;

        playerData.money += money;
        playerData.diamond += diamond;
        SavePlayerData();

        // chuyển tới level tiếp theo
        //if(level < 5)
        //{
        //    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        //}
    }

    public void StartNewLevel(int lv)
    {
        playerData.currentLevel = lv;
        Debug.Log("Start New Level: " + lv + playerData.currentLevel);
    }

    public void LevelFailed(int level, int clue)
    {
        // xử lý sau
        //playerData.clue = level
        SavePlayerData();
    }

    public void UpdateClue(int indexClue)
    {
        if (indexClue >= 0 && indexClue < playerData.clue.Length)
        {
            if (playerData.clue[indexClue] == 0)
            {
                playerData.clue[indexClue] = 1;
                ClueChangedEvent?.Invoke(playerData.clue);
                SavePlayerData();
            }
        }

    }

    public bool UpgradeAttack(int atkIndex, int price)
    {
        if (playerData.money < price)
            return false;

        playerData.money -= price;
        playerData.atkIndex = atkIndex;
        SavePlayerData();
        return true;
    }

    public bool UpgradeHealth(int hpIndex, int price)
    {
        if (playerData.money < price)
            return false;

        playerData.money -= price;
        playerData.hpIndex = hpIndex;
        SavePlayerData();
        return true;
    }

    public bool UpgradeSpeed(int speedIndex, int price)
    {
        if (playerData.money < price)
            return false;

        playerData.money -= price;
        playerData.attackSpeedIndex = speedIndex;
        SavePlayerData();
        return true;
    }

    public bool UpgradeDefense(int defIndex, int price)
    {
        if (playerData.money < price)
            return false;

        playerData.money -= price;
        playerData.defIndex = defIndex;
        SavePlayerData();
        return true;
    }

    public bool UnlockSkill1()
    {
        if (playerData.diamond < SKILL1_PRICE)
            return false;

        playerData.diamond -= SKILL1_PRICE;
        playerData.singleSkill1 = true;
        SavePlayerData();
        return true;
    }

    public bool UnlockSkill2()
    {
        if (playerData.diamond < SKILL2_PRICE)
            return false;

        playerData.diamond -= SKILL2_PRICE;
        playerData.singleSkill2 = true;
        SavePlayerData();
        return true;
    }
    public bool UnlockCombineSkill()
    {
        if (playerData.diamond < COMBINE_SKILL_PRICE)
            return false;

        playerData.diamond -= COMBINE_SKILL_PRICE;
        playerData.combineSkill = true;
        SavePlayerData();
        return true;
    }



    public int getLevel()
    {
        return playerData.level;
    }

    public int getCurrentLevel()
    {
        return playerData.currentLevel;
    }

    public int getMoney()
    {
        return playerData.money;
    }

    public int getDiamond()
    {
        return playerData.diamond;
    }

    public int[] getStars()
    {
        return playerData.stars;
    }

    public int[] getClueCollection()
    {
        return playerData.clue;
    }

    public int getATK()
    {
        return playerData.atkIndex * DEFAULT_ATK;
    }

    public int getHP()
    {
        return playerData.hpIndex * DEFAULT_HP;
    }

    public int getAttackSpeed()
    {
        return playerData.attackSpeedIndex * DEFAULT_ATTACK_SPEED;
    }

    public int getDEF()
    {
        return playerData.defIndex * DEFAULT_DEF;
    }

    public int getAtkIndex()
    {
        return playerData.atkIndex;
    }

    public int getHpIndex()
    {
        return playerData.hpIndex;
    }

    public int getAttackSpeedIndex()
    {
        return playerData.attackSpeedIndex;
    }

    public int getDefIndex()
    {
        return playerData.defIndex;
    }

    public bool learnedSkill(Player player)
    {
        if (player == Player.Player1)
            return playerData.singleSkill1;
        else
            return playerData.singleSkill2;
    }

    public bool learnedSkill1()
    {
        return playerData.singleSkill1;
    }

    public bool learnedSkill2()
    {
        return playerData.singleSkill2;
    }

    public bool learnedCombineSkill()
    {
        return playerData.combineSkill;
    }

    public bool isCheat()
    {
        return playerData.isCheat;
    }

    public void setCheat(bool isCheat)
    {
        playerData.isCheat = isCheat;
        SavePlayerData();
    }
}