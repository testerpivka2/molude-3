using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] private Text damageText;
    [SerializeField] private Text healsText;
    [SerializeField] private Text goldText;

    private void OnEnable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnStatsChanged.AddListener(Refresh);
            Refresh();
        }
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnStatsChanged.RemoveListener(Refresh);
    }

    private void Start()
    {
        if (GameManager.Instance != null) Refresh();
    }

    private void Refresh()
    {
        var gm = GameManager.Instance;
        if (damageText != null) damageText.text = $"Урон: {gm.Damage}";
        if (healsText  != null) healsText.text  = $"Хилки: {gm.Heals}";
        if (goldText   != null) goldText.text   = $"Золото: {gm.Gold}";
    }
}
