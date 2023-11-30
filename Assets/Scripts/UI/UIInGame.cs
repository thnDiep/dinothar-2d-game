using UnityEngine;

public class UIInGame : MonoBehaviour
{
    PlayerManager playerManager;
    

    private void Start()
    {
        playerManager = GetComponent<PlayerManager>();
        //playerManager.money.OnValueChanged += UpdateMoney;

    }

    private void Update()
    {

    }

    private void UpdateMoney()
    {
        if (playerManager != null)
        {

        }
    }

}
