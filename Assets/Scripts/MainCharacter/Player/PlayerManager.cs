using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }
    [SerializeField] PlayerController player1;
    [SerializeField] PlayerController player2;
    [SerializeField] Tilemap gate;
    public UIInGame uiInGame;

    public enum Player
    {
        Player1,
        Player2,
    }

    public enum PlayerState
    {
        Normal,
        Rotate
    }

    public enum PlayerStage
    {
        Move,
        Fight
    }


    public PlayerStage Stage;
    private bool isSetFightStage = false;
    public PlayerState State;
    private PlayerInputConfig player1Input, player2Input;


    private int minLife = 0;
    private int maxLife = 5;
    private int currentMoney = 0;
    private int currentDiamond = 0;
    private int currentLife;


    private int maxHealth = 100;
    private int health;

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

        // đánh boss
        health = maxHealth;

        currentLife = maxLife - 1;

        // Update UI lúc bắt đầu
        uiInGame.setMoney(currentMoney);
        uiInGame.setDiamond(currentDiamond);
        uiInGame.setLife(currentLife);
    }

    private void Update()
    {
        if(Stage == PlayerStage.Fight)
        {
            if(!isSetFightStage)
                setStage(true);

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
        isSetFightStage = isFightStage;

        Stage = isFightStage ? PlayerStage.Fight : PlayerStage.Move;

        // UI in Move stage
        uiInGame.moneyBar.gameObject.SetActive(!isFightStage);

        // UI in Fight stage
        gate.gameObject.SetActive(isFightStage);
        uiInGame.skillBar1.gameObject.SetActive(isFightStage);
        uiInGame.skillBar2.gameObject.SetActive(isFightStage);
    }

    public void changeMoney(int money)
    {
        this.currentMoney += money;
        uiInGame.setMoney(this.currentMoney);
    }

    public void changeDiamond(int diamond)
    {
        this.currentDiamond += diamond;
        uiInGame.setDiamond(this.currentDiamond);
    }

    public void changeLife(int life)
    {
        this.currentLife = Mathf.Clamp(this.currentLife + 1, minLife, maxLife);
        uiInGame.setLife(this.currentLife);
    }

    public int getMoney()
    {
        return this.currentMoney;
    }

    public void changeHealth(int amount)
    {
        health = Mathf.Clamp(health + amount, 0, maxHealth);
        if (health <= 0)
        {
            Debug.Log("Dead");
            return;
        }
        Debug.Log("Health:" + health);
    }
}
