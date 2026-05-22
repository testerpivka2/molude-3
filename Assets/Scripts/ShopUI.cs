using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopUI : MonoBehaviour
{
    [Header("References")]
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI damageText;
    public Button buyDamageButton;
    public Button buyHealButton;
    public Button exitButton;
    public ShopTrigger shopTrigger;

    [Header("Prices")]
    public int damageUpgradeCost = 10;
    public int healCost = 5;
    public int damageUpgradeAmount = 5;

    private PlayerMove playerMove;
    private BarScript playerHealth;

    void Start()
    {
        shopTrigger = GetComponentInParent<ShopTrigger>();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerMove = player.GetComponent<PlayerMove>();
            playerHealth = player.GetComponent<BarScript>();
        }

        buyDamageButton.onClick.AddListener(BuyDamage);
        buyHealButton.onClick.AddListener(BuyHeal);
        exitButton.onClick.AddListener(CloseShop);

        UpdateUI();
    }

    void UpdateUI()
    {
        if (goldText != null)
        {
            goldText.text = $"Current gold: {PlayerPrefs.GetInt("Gold", 0)}";
        }

    }

    void BuyDamage()
    {
        int currentGold = PlayerPrefs.GetInt("Gold", 0);

        if (currentGold >= damageUpgradeCost)
        {
            PlayerPrefs.SetInt("Gold", currentGold - damageUpgradeCost);

            int currentDamage = PlayerPrefs.GetInt("PlayerDamage", 10);
            PlayerPrefs.SetInt("PlayerDamage", currentDamage + damageUpgradeAmount);

            // Обновляем урон в скрипте атаки игрока
            //if (playerMove != null)
            //{
            //    // Нужно обновить attackDamage в PlayerMove
            //    playerMove.UpdateDamage(PlayerPrefs.GetInt("PlayerDamage", 10));
            //}

            UpdateUI();
        }
        else
        {

        }
    }

    void BuyHeal()
    {
        int currentGold = PlayerPrefs.GetInt("Gold", 0);

        if (currentGold >= healCost)
        {
            PlayerPrefs.SetInt("Gold", currentGold - healCost);

            if (playerHealth != null)
            {
                playerHealth.Heal(10);
            }

            UpdateUI();

        }
        else
        {

        }
    }

    public void CloseShop()
    {

        if (shopTrigger == null)
        {
            return;
        }

        shopTrigger.CloseShop();
    }
}