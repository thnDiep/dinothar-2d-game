using Unity.VisualScripting;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    public enum PlayerState
    {
        Normal,
        Rotate
    }
    private PlayerState state;


    public enum Player
    {
        Player1,
        Player2,
    }
    private PlayerInputConfig player1Input, player2Input;


    private int minLife = 0;
    private int maxLife = 5;
    private int currentMoney = 0;
    private int currentDiamond = 0;
    private int currentLife;
    [SerializeField] PlayerController player1;
    [SerializeField] PlayerController player2;

    // Các thành phần UI
    [SerializeField] UIInGame uIInGame;
    public PlayerState State
    {
        get { return state; }
        set { state = value; }
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
            DontDestroyOnLoad(gameObject);
        }

        player1Input = new PlayerInputConfig(Player.Player1);
        player2Input = new PlayerInputConfig(Player.Player2);

        currentLife = maxLife - 1;

        // Update UI lúc bắt đầu
        uIInGame.setMoney(currentMoney);
        uIInGame.setDiamond(currentDiamond);
        uIInGame.setLife(currentLife);
    }

    private void Update()
    {
        if (Input.GetKey(player1Input.useCombineSkill) && Input.GetKey(player2Input.useCombineSkill)
            && player1.getCanUseCombineSkill() && player2.getCanUseCombineSkill())
        {
            player1.UseCombineSkill();
            player2.UseCombineSkill();
        }
    }

    public void changeMoney(int money)
    {
        this.currentMoney += money;
        uIInGame.setMoney(this.currentMoney);
    }

    public void changeDiamond(int diamond)
    {
        this.currentDiamond += diamond;
        uIInGame.setDiamond(this.currentDiamond);
    }

    public void changeLife(int life)
    {
        this.currentLife = Mathf.Clamp(this.currentLife + 1, minLife, maxLife);
        uIInGame.setLife(this.currentLife);
    }

    public int getMoney()
    {
        return this.currentMoney;
    }
}
