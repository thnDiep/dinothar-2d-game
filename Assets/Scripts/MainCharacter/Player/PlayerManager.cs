using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }
    [Header("Stats")]
    public float HP = 100;
    public int ATK = 10;
    public float ATTACK_SPEED = 200f;
    public float DEF = 0;

    [Header("Frictions")]
    public PhysicsMaterial2D maxFriction;
    public PhysicsMaterial2D highFriction;
    public PhysicsMaterial2D normalFriction;

    [Header("Content")]
    public PlayerController player1;
    public PlayerController player2;
    [SerializeField] GameObject fightArea;
    [SerializeField] Tilemap gate;
    [SerializeField] GameObject rope;

    [Header("UI")]
    public UIInGame uIInGame;
    [SerializeField] ClueCollecitonBtn clueCollection;

    [Header("Information")]
    public PlayerStage Stage;
    public PlayerState State;

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

    private float health;
    private bool dead;
    public Vector3 revivalPosition;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        player1Input = new PlayerInputConfig(Player.Player1);
        player2Input = new PlayerInputConfig(Player.Player2);

        // Giai đoạn di chuyển
        setStage(false);

        currentMoney = 0;
        currentDiamond = 0;
        currentLife = maxLife - 1;
        currentClue = 0;

        health = HP;
        revivalPosition = new Vector3(fightArea.transform.position.x - 1.0f, fightArea.transform.position.y + 5.0f, 0);

        // Update UI lúc bắt đầu
        uIInGame.setMoney(currentMoney);
        uIInGame.setDiamond(currentDiamond);
        uIInGame.setLife(currentLife);
        uIInGame.starBar.setStars(currentClue);
    }

    private void Update()
    {
        if (Stage == PlayerStage.Move)
        {
            if (player1.transform.position.x < fightArea.transform.position.x && player2.transform.position.x < fightArea.transform.position.x
                && player1.transform.position.y > fightArea.transform.position.y && player2.transform.position.y > fightArea.transform.position.y)
            {
                Stage = PlayerStage.Fight;
                setStage(true); // giai đoạn chiến đấu  
            }

        }
        else if (Stage == PlayerStage.Fight)
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
        Stage = isFightStage ? PlayerStage.Fight : PlayerStage.Move;

        // Move stage
        uIInGame.bars.gameObject.SetActive(!isFightStage);

        // Fight stage
        gate.gameObject.SetActive(isFightStage);
        uIInGame.skillBar1.gameObject.SetActive(isFightStage);
        uIInGame.skillBar2.gameObject.SetActive(isFightStage);
    }

    public void changeMoney(int money)
    {
        this.currentMoney += money;

        if (this.currentMoney < 0)
            this.currentMoney = 0;

        uIInGame.setMoney(this.currentMoney);
    }

    public void changeDiamond(int diamond)
    {
        this.currentDiamond += diamond;

        if (this.currentDiamond < 0)
            this.currentDiamond = 0;

        uIInGame.setDiamond(this.currentDiamond);
    }

    public void changeLife(int life)
    {
        this.currentLife = Mathf.Clamp(this.currentLife + life, 0, maxLife);
        uIInGame.setLife(this.currentLife);

        if (currentLife == 0)
        {
            Debug.Log("Losing");
        }
    }

    public void changeClue(int clue)
    {
        this.currentClue = Mathf.Clamp(this.currentClue + clue, 0, maxClue);
        clueCollection.unblockClue();
        uIInGame.starBar.setStars(currentClue);
    }

    public void TakeDamage(int damage)
    {
        float lossHealth = (1 - DEF / 10.0f) * damage;
        health -= lossHealth;

        // Nếu hết máu, trừ 1 mạng và fill đầy máu lại (nếu còn mạng)
        if (health <= 0)
        {
            changeLife(-1);
            dead = true;
            player1.Die();
            player2.Die();
            rope.SetActive(false);

            Debug.Log("Dead");
            Debug.Log("Current life" + currentLife);

            // Hồi sinh nếu còn mạng
            if (currentLife > 0)
            {
                StartCoroutine(RevialAfterDelay(2f));
            }
        }
        Debug.Log("Health:" + health);
    }

    IEnumerator RevialAfterDelay(float delay)
    {
        // Chờ đợi cho animation chết hoàn thành
        yield return new WaitForSeconds(delay);
        dead = false;

        health = HP;
        player1.Revival();
        player2.Revival();
        rope.SetActive(true);
    }

    public bool isDead()
    {
        return dead;
    }

    public void Win()
    {
        Debug.Log("Winning");
    }
}
