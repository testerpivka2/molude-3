using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    [Header("Цены и эффекты")]
    [SerializeField] private int damageCost = 50;
    [SerializeField] private int damageAmount = 5;
    [SerializeField] private int healCost = 30;

    [Header("UI")]
    [SerializeField] private Text goldText;
    [SerializeField] private Button damageButton;
    [SerializeField] private Button healButton;
    [SerializeField] private Text messageText;

    private void OnEnable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnStatsChanged.AddListener(Refresh);
            Refresh();
        }
        if (messageText != null) messageText.text = "";
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnStatsChanged.RemoveListener(Refresh);
    }

    private void Refresh()
    {
        var gm = GameManager.Instance;
        if (goldText != null) goldText.text = $"Золото: {gm.Gold}";

        if (damageButton != null) damageButton.interactable = gm.Gold >= damageCost;
        if (healButton   != null) healButton.interactable   = gm.Gold >= healCost;
    }

    public void BuyDamage()
    {
        if (GameManager.Instance.TrySpend(damageCost))
        {
            GameManager.Instance.AddDamage(damageAmount);
            ShowMessage($"Урон +{damageAmount}!");
        }
        else ShowMessage("Недостаточно золота");
    }

    public void BuyHeal()
    {
        if (GameManager.Instance.TrySpend(healCost))
        {
            GameManager.Instance.AddHeals(1);
            ShowMessage("Хилка куплена!");
        }
        else ShowMessage("Недостаточно золота");
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    private void ShowMessage(string msg)
    {
        if (messageText != null) messageText.text = msg;
    }
}
