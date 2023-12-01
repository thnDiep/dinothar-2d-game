using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static PlayerInputConfig;

public class PlayerManager : MonoBehaviour
{

    private int money = 0;

    [SerializeField] TextMeshProUGUI MoneyTxt;
    public static PlayerManager Instance { get; private set; }
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

    // Update is called once per frame
    void Update()
    {

    }


    public void setMoney(int money)
    {
        this.money = money;
        if (MoneyTxt != null)
        {
            MoneyTxt.text = money.ToString();
        }
    }

    public void plusMoney(int money)
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
