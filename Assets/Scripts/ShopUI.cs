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
    public int healCost = 10;
    public int damageUpgradeAmount = 1;
    public int HealAmount = 5;

    private PlayerMove playerMove;
    private BarScript playerHealth;
    private PlayerGold playerGold;

    void Start()
    {
        shopTrigger = GetComponentInParent<ShopTrigger>();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerMove = player.GetComponent<PlayerMove>();
            playerHealth = player.GetComponent<BarScript>();
            playerGold = player.GetComponent<PlayerGold>();
        }

        buyDamageButton.onClick.AddListener(BuyDamage);
        buyHealButton.onClick.AddListener(BuyHeal);
        exitButton.onClick.AddListener(CloseShop);

        UpdateUI();
    }

    void UpdateUI()
    {
        if (goldText != null && playerGold != null)
        {
            goldText.text = $"Current gold: {playerGold.currentGold}";
        }
    }

    public void BuyDamage()
    {
        PlayerGold playerGold = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerGold>();
        PlayerDamage playerDamage = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerDamage>();

        if (playerGold != null && playerDamage != null && playerGold.SpendGold(damageUpgradeCost))
        {
            playerDamage.UpgradeDamage(damageUpgradeAmount);
            UpdateUI();
        }
    }


    public void BuyHeal()
    {
        PlayerGold playerGold = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerGold>();
        BarScript playerHealth = GameObject.FindGameObjectWithTag("Player")?.GetComponent<BarScript>();

        if (playerGold != null && playerHealth != null && playerGold.SpendGold(healCost))
        {
            playerHealth.Heal(HealAmount);
            UpdateUI();
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