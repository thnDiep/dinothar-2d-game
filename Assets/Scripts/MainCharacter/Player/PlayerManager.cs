using TMPro;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }
    private PlayerState state;
    public enum PlayerState
    {
        Normal,
        Rotate
    }

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

        // Update UI lúc bắt đầu
        moneyBarUI.setMoney(money);
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
