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


    private int money = 0;
    [SerializeField] PlayerController player1;
    [SerializeField] PlayerController player2;

    // Các thành phần UI
    [SerializeField] MoneyBar moneyBarUI;
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

        // Update UI lúc bắt đầu
        moneyBarUI.setMoney(money);

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
        this.money += money;
        moneyBarUI.setMoney(this.money);
    }

    public int getMoney()
    {
        return this.money;
    }
}
