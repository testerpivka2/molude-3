using UnityEngine;
using TMPro;

public class PlayerGold : MonoBehaviour
{
    [Header("Gold Settings")]
    public int currentGold = 0;
    public TextMeshProUGUI goldText;

    public event System.Action<int> OnGoldChanged;

    private void Start()
    {
        UpdateUI();
    }

    public void AddGold(int amount)
    {
        currentGold += amount;
        UpdateUI();
        OnGoldChanged?.Invoke(currentGold);
    }

    public bool SpendGold(int amount)
    {
        if (currentGold >= amount)
        {
            currentGold -= amount;
            UpdateUI();
            OnGoldChanged?.Invoke(currentGold);
            return true;
        }
        else
        {
            return false;
        }
    }

    private void UpdateUI()
    {
        if (goldText != null)
        {
            goldText.text = $"Gold: {currentGold}";
        }
    }
}