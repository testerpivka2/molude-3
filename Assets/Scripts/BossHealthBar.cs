using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Полоска здоровья босса.
/// Создай Canvas → Image (фон) → Image (заливка, Image Type: Filled, Fill Method: Horizontal)
/// Прикрепи этот скрипт на Canvas и назначь поля.
/// </summary>
public class BossHealthBar : MonoBehaviour
{
    public BossHealth bossHealth;
    public Image fillImage;          // Image с Fill Method = Horizontal
    public GameObject bossBarRoot;   // весь UI — скрываем до встречи с боссом

    void Start()
    {
        if (bossBarRoot != null)
            bossBarRoot.SetActive(false);

        if (bossHealth != null)
            bossHealth.onDeath.AddListener(() => bossBarRoot?.SetActive(false));
    }

    void Update()
    {
        if (bossHealth == null || fillImage == null) return;
        fillImage.fillAmount = bossHealth.HealthPercent;
    }

    // Вызови когда игрок входит в комнату босса
    public void ShowBar()
    {
        if (bossBarRoot != null)
            bossBarRoot.SetActive(true);
    }
}
