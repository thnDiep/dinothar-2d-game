using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }
    [SerializeField] PlayerController player1;
    [SerializeField] PlayerController player2;
    [SerializeField] Tilemap gate;
    public UIInGame uIInGame;
    [SerializeField] ClueCollecitonBtn clueCollection;

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
    private int maxClue = 3;

    private int currentMoney, currentDiamond, currentLife, currentClue;

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

        currentMoney = 0;
        currentDiamond = 0;
        currentLife = maxLife - 1;
        currentClue = 0;

        // Update UI lúc bắt đầu
        uIInGame.setMoney(currentMoney);
        uIInGame.setDiamond(currentDiamond);
        uIInGame.setLife(currentLife);
        uIInGame.starBar.setStars(currentClue);
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
        this.currentLife = Mathf.Clamp(this.currentLife + life, minLife, maxLife);
        uIInGame.setLife(this.currentLife);
    }

    public void changeClue(int clue)
    {
        this.currentClue = Mathf.Clamp(this.currentClue + 1, 0, maxClue);
        clueCollection.unblockClue();
        //uIInGame.setStar(this.currentClue);
        uIInGame.starBar.setStars(currentClue);
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
