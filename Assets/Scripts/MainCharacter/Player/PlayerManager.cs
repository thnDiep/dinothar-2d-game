using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;


public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    public static event Action<int> LifeChangeEvent;
    public static event Action<PlayerStage> StageChangeEvent;
    public static event Action<float> HealthChangeEvent;

    [Header("Stats")]
    public float HP = 100;
    public int ATK = 10;
    public float ATTACK_SPEED = 50f;
    public float DEF = 0;

    [Header("Frictions")]
    public PhysicsMaterial2D maxFriction;
    public PhysicsMaterial2D highFriction;
    public PhysicsMaterial2D normalFriction;

    [Header("Content")]
    public int level;
    public PlayerController player1;
    public PlayerController player2;
    [SerializeField] GameObject rope;

    [Header("Map")]
    [SerializeField] GameObject fightArea;
    [SerializeField] Tilemap gate;
    public GameObject StreetFrame;
    public GameObject FightBossFrame;

    [Header("Server")]
    public CameraController cameraController;
    public CameraController minimap;

    [Header("Information")]
    public PlayerStage Stage;
    public PlayerState State;

    [Header("Shop")]
    public float healthBonus;
    public int damageBonus;
    public float attackSpeedBonus;


    public enum Player
    {
        Player1,
        Player2,
    }

    public enum PlayerState
    {
        Normal,
        Rotate,
    }

    public enum PlayerStage
    {
        Move,
        Fight
    }

    private PlayerInputConfig player1Input, player2Input;

    private int maxLife = 5;
    private int maxClue = 3;

    private int currentMoney, currentDiamond, currentLife, currentClue;

    public int life
    {
        get { return currentLife; }
        set
        {
            if (currentLife != value)
            {
                currentLife = value;
                LifeChangeEvent?.Invoke(currentLife);
            }
        }
    }

    private float currentHealth;
    private bool dead;
    public Vector3 revivalPosition;

    public float health
    {
        get { return currentHealth; }
        set
        {
            if (currentHealth != value)
            {
                currentHealth = value;
                HealthChangeEvent?.Invoke(currentHealth);
            }
        }
    }


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

    void Start()
    {
        player1Input = new PlayerInputConfig(Player.Player1);
        player2Input = new PlayerInputConfig(Player.Player2);

        // Giai đoạn di chuyển
        Stage = PlayerStage.Move;
        StageChangeEvent?.Invoke(Stage);
        setStage(false);

        currentMoney = 0;
        currentDiamond = 0;
        //currentLife = maxLife;
        life = maxLife;
        Debug.Log("PLayerManager Life:" + life);
        currentClue = 0;

        if (UIInGame.Instance != null)
        {
            UIInGame.Instance.setMoney(currentMoney);
            UIInGame.Instance.setDiamond(currentDiamond);
            //UIInGame.Instance.setLife(currentLife);
            UIInGame.Instance.setStar(currentClue);
        }

        // Giai đoạn chiến đấu
        healthBonus = 0;
        damageBonus = 0;
        attackSpeedBonus = 0;
        updatePower();
        health = HP;
        revivalPosition = new Vector3(fightArea.transform.position.x - 1.0f, fightArea.transform.position.y + 5.0f, 0);
    }

    private void Update()
    {
        if (Stage == PlayerStage.Move)
        {
            if (player1.transform.position.x < fightArea.transform.position.x && player2.transform.position.x < fightArea.transform.position.x
                && player1.transform.position.y > fightArea.transform.position.y && player2.transform.position.y > fightArea.transform.position.y)
            {
                // chuyển sang giai đoạn chiến đấu
                Stage = PlayerStage.Fight;
                StageChangeEvent?.Invoke(Stage);
                setStage(true);
            }
        }
        else if (Stage == PlayerStage.Fight && GameManager.Instance.learnedCombineSkill())
        {
            if (Input.GetKey(player1Input.useCombineSkill) && Input.GetKey(player2Input.useCombineSkill)
            && player1.getCanUseCombineSkill() && player2.getCanUseCombineSkill())
            {
                player1.UseCombineSkill();
                player2.UseCombineSkill();
            }
        }
    }

    public void setStage(bool isFightStage)
    {
        minimap.gameObject.SetActive(!isFightStage);
        FightBossFrame.SetActive(!isFightStage);

        gate.gameObject.SetActive(isFightStage);
        StreetFrame.SetActive(isFightStage);
        cameraController.setIsFightStage(isFightStage);
        if (UIInGame.Instance != null)
            UIInGame.Instance.setStage(isFightStage);
    }

    // giai đoạn di chuyển
    public void changeMoney(int money)
    {
        this.currentMoney += money;

        if (this.currentMoney < 0)
            this.currentMoney = 0;

        if (UIInGame.Instance != null)
            UIInGame.Instance.setMoney(this.currentMoney);
    }

    public int getMoney()
    {
        return currentMoney;
    }

    public void changeDiamond(int diamond)
    {
        this.currentDiamond += diamond;

        if (this.currentDiamond < 0)
            this.currentDiamond = 0;

        if (UIInGame.Instance != null)
            UIInGame.Instance.setDiamond(this.currentDiamond);
    }

    public int getDiamond()
    {
        return currentDiamond;
    }

    public int getMaxLife()
    {
        return maxLife;
    }

    public void changeLife(int amount)
    {
        //this.currentLife = Mathf.Clamp(this.currentLife + life, 0, maxLife);
        life = Mathf.Clamp(life + amount, 0, maxLife);

        //if (UIInGame.Instance != null)
        //    UIInGame.Instance.updateLife(currentLife);

        // Thua nếu hết mạng
        if (life <= 0)
        {
            Lose();
        }
    }

    public void collectClue(int clueIndex)
    {
        this.currentClue = Mathf.Clamp(this.currentClue + 1, 0, maxClue);

        if (GameManager.Instance != null)
            GameManager.Instance.UpdateClue(clueIndex);
        if (UIInGame.Instance != null)
            UIInGame.Instance.setStar(this.currentClue);
    }

    public int getStar()
    {
        return currentClue;
    }


    // giai đoạn chiến đấu
    public void buyHealth(float amount)
    {
        healthBonus += amount;
        health += amount;
        Debug.Log("Health" + health);
    }

    public void buyDamage(int amount)
    {
        damageBonus += amount;
        ATK += amount;
        Debug.Log("ATK" + ATK);
    }

    public void buyAttackSpeed(float amount)
    {
        attackSpeedBonus += amount;
        ATTACK_SPEED += amount;
        Debug.Log("ATTACK_SPEED" + attackSpeedBonus);
    }

    public void updatePower()
    {
        if (GameManager.Instance != null)
        {
            ATK = GameManager.Instance.getATK() + damageBonus;
            HP = GameManager.Instance.getHP() + healthBonus;
            ATTACK_SPEED = GameManager.Instance.getAttackSpeed() + attackSpeedBonus;
            DEF = GameManager.Instance.getDEF();

            if (UIInGame.Instance != null)
                UIInGame.Instance.playerHealthBar.setMaxHealth(HP);
        }
    }

    public void TakeDamage(int damage)
    {
        float lossHealth = (1 - DEF / 10.0f) * damage;
        health = Mathf.Clamp(health - lossHealth, 0, HP);

        // Nếu hết máu, trừ 1 mạng và fill đầy máu lại (nếu còn mạng)
        if (health <= 0)
        {
            changeLife(-1);
            dead = true;
            player1.Die();
            player2.Die();
            rope.SetActive(false);

            // Hiệu ứng hồi sinh nếu còn mạng
            if (currentLife > 0)
            {
                StartCoroutine(RevivalAfterDelay(2f));
            }
        }
    }

    IEnumerator RevivalAfterDelay(float delay)
    {
        if (UIInGame.Instance != null)
            UIInGame.Instance.startScreenFade();
        // Chờ đợi cho animation chết hoàn thành
        yield return new WaitForSeconds(delay);
        dead = false;

        updatePower();
        health = HP;
        player1.Revival();
        player2.Revival();
        rope.SetActive(true);
    }

    public bool isDead()
    {
        return dead;
    }

    // Kết thúc level
    public void Win()
    {
        if (UIInGame.Instance != null)
            if (GameManager.Instance.getLevel() == 5)
            {
                SceneLoader.Instance.ToCutScreen();
            }
            else
            {
                UIInGame.Instance.showWinningScreen();
            }
        if (GameManager.Instance != null)
            GameManager.Instance.LevelCompleted(level, currentMoney, currentDiamond, currentClue);
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlaySoundWinLevel();
    }

    public void Lose()
    {
        if (UIInGame.Instance != null)
            UIInGame.Instance.showLosingScreen();
        if (GameManager.Instance != null)
            GameManager.Instance.LevelFailed(level, currentClue);
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlaySoundGameOver();
    }
}
