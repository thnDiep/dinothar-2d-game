using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using static PlayerManager;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private PlayerData playerData;

    public static event Action<int> MoneyChangedEvent;
    public static event Action<int> DiamondChangedEvent;

    public int DEFAULT_HP = 100;
    public int DEFAULT_ATK = 10;
    public int DEFAULT_ATTACK_SPEED = 200;
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
        PlayerPrefs.DeleteKey("PlayerData"); // Hoàn thành game thì xóa dòng này
        LoadPlayerData();
    }

    void FixedUpdate()
    {
        Debug.Log(playerData.money);
    }

    void SavePlayerData()
    {
        PlayerPrefs.SetString("PlayerData", JsonUtility.ToJson(playerData));
        PlayerPrefs.Save();
        MoneyChangedEvent?.Invoke(playerData.money);
        DiamondChangedEvent?.Invoke(playerData.diamond);
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

    public void LevelCompleted(int level, int money, int diamond, int clue)
    {
        // Cập nhật trạng thái khi người chơi hoàn thành cấp độ
        playerData.level = level;

        // Lấy số sao lớn nhất
        if(clue > playerData.stars[level])
            playerData.stars[level] = clue;

        playerData.money += money;
        playerData.diamond += diamond;

        // Lưu dữ liệu sau khi người chơi hoàn thành cấp độ
        SavePlayerData();

        //// Chuyển scene sau khi hoàn thành cấp độ
        //SceneManager.LoadScene("NextLevelScene");
    }
    public void LevelFailed(int level, int clue)
    {
        // xử lý sau
        //playerData.clue = level
        SavePlayerData();
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
        if(playerData.diamond < SKILL1_PRICE) 
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
}