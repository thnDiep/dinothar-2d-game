using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static PlayerInputConfig;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    private int money = 0;
    [SerializeField] TextMeshProUGUI MoneyTxt;
    [SerializeField] PlayerController player1;
    [SerializeField] PlayerController player2;

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
        if (MoneyTxt != null)
        {
            MoneyTxt.text = money.ToString();
        }
    }

    public void changeMoney(int money)
    {
        this.money += money;
        if (MoneyTxt != null)
        {
            MoneyTxt.text = this.money.ToString();
        }
    }

    public int getMoney()
    {
        return money;
    }
}
