using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    public GameObject screen;

    [Header("Header")]
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI diamondText;
    [SerializeField] GameObject[] pages;
    [SerializeField] TextMeshProUGUI pageNumber;
    private int currentIndex;

    [Header("Product")]
    public ProductInShopUI life;
    public ProductInShopUI health;
    public ProductInShopUI damage;
    public ProductInShopUI attackSpeed;

    void Start()
    {
        screen.SetActive(false);
        currentIndex = 0;
        pageNumber.text = (currentIndex + 1).ToString() + "/" + pages.Length.ToString();
        for(int i = 0; i < pages.Length; i++)
        {
            if(i == currentIndex)
                pages[i].gameObject.SetActive(true);
            else
                pages[i].gameObject.SetActive(false);
        }

        GameManager.MoneyChangedEvent += HandleMoneyChanged;
        GameManager.DiamondChangedEvent += HandleDiamondChanged;
        PlayerManager.LifeChangeEvent += HandleChangeLife;
        PlayerManager.StageChangeEvent += HandleChangeStage;

        HandleMoneyChanged(GameManager.Instance.getMoney());
        HandleDiamondChanged(GameManager.Instance.getDiamond());
        HandleChangeLife(PlayerManager.Instance.life);
    }

    private void OnDestroy()
    {
        GameManager.MoneyChangedEvent -= HandleMoneyChanged;
        GameManager.DiamondChangedEvent -= HandleDiamondChanged;
        PlayerManager.LifeChangeEvent -= HandleChangeLife;
        PlayerManager.StageChangeEvent -= HandleChangeStage;

    }

    private void HandleMoneyChanged(int newMoney)
    {
        moneyText.text = newMoney.ToString();
    }

    private void HandleDiamondChanged(int newDiamond)
    {
        diamondText.text = newDiamond.ToString();
    }

    private void HandleChangeLife(int newLife)
    {
        life.setCanBuy(newLife < PlayerManager.Instance.getMaxLife());
    }

    private void HandleChangeStage(PlayerManager.PlayerStage newStage)
    {
        bool canBuy = (newStage == PlayerManager.PlayerStage.Fight);
        health.setCanBuy(canBuy);
        damage.setCanBuy(canBuy);
        attackSpeed.setCanBuy(canBuy);
    }

    public void nextPage()
    {
        pages[currentIndex].gameObject.SetActive(false);
        currentIndex = (currentIndex + 1) % pages.Length;
        pages[currentIndex].gameObject.SetActive(true);
        pageNumber.text = (currentIndex + 1).ToString() + "/" + pages.Length.ToString();
    }

    public void prevPage()
    {
        pages[currentIndex].gameObject.SetActive(false);
        currentIndex = (currentIndex - 1 + pages.Length) % pages.Length;
        pages[currentIndex].gameObject.SetActive(true);
        pageNumber.text = (currentIndex + 1).ToString() + "/" + pages.Length.ToString();
    }

    public void buyHeart()
    {
        if(life.buyProduct())
        {
            PlayerManager.Instance.changeLife(1);
        }
    }

    public void buyHealth()
    {
        if (health.buyProduct())
        {
            float amount = GameManager.Instance.getHP() * 0.1f;
            PlayerManager.Instance.buyHealth(amount);
        }
    }

    public void buyDamage()
    {
        if (damage.buyProduct())
        {
            int amount = Mathf.RoundToInt(GameManager.Instance.getATK() * 0.1f);
            PlayerManager.Instance.buyDamage(amount);
        }
    }

    public void buyAttackSpeed()
    {
        if (attackSpeed.buyProduct())
        {
            float amount = GameManager.Instance.getAttackSpeed() * 0.1f;
            PlayerManager.Instance.buyAttackSpeed(amount);
        }
    }
}
