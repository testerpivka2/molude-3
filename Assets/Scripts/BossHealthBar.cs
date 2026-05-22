using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    public BossHealth bossHealth;
    public Image fillImage;          
    public GameObject bossBarRoot;   

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

    
    public void ShowBar()
    {
        if (bossBarRoot != null)
            bossBarRoot.SetActive(true);
    }
}
