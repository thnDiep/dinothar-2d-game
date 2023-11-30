using UnityEngine;
using UnityEngine.UI;
using static PlayerInputConfig;

public class PlayerManager : MonoBehaviour
{

    public int money = 0;

    public Text MoneyTxt;
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
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void setMoney(int money)
    {
        this.money = money;
    }

    public void plusMoney(int money)
    {
        this.money += money;
    }

    public int getMoney()
    {
        return money;
    }

    void onGUI()
    {

    }




}
