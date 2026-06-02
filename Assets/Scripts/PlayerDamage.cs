using UnityEngine;
using TMPro;

public class PlayerDamage : MonoBehaviour
{
    [Header("Damage Settings")]
    public int currentDamage = 10;
    public TextMeshProUGUI damageText;

    private PlayerMove playerMove;

    public event System.Action<int> OnDamageChanged;

    void Start()
    {
        playerMove = GetComponent<PlayerMove>();
        UpdateUI();
    }

    public void UpgradeDamage(int amount)
    {
        currentDamage += amount;
        UpdateUI();
        OnDamageChanged?.Invoke(currentDamage);

        if (playerMove != null)
        {
            playerMove.UpdateDamage(currentDamage);
        }

    }

    private void UpdateUI()
    {
        if (damageText != null)
        {
            damageText.text = $"Damage: {currentDamage}";
        }
    }
}