using UnityEngine;
using UnityEngine.UI;

public class BarScript : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHP = 100f;
    private float currentHP;

    [Header("UI")]
    public Image barImage; 
    public float regenRate = 0.01f; 

    private Animator anim;

    private PlayerMove playerMove;

    void Start()
    {
        currentHP = maxHP;

        playerMove = GetComponent<PlayerMove>();

        if (barImage == null)
        {
            barImage = GameObject.Find("HPBar")?.GetComponent<Image>();
        }

        anim = GetComponent<Animator>();
        UpdateBar();
    }

    void Update()
    {
 
        if (currentHP < maxHP && currentHP > 0)
        {
            currentHP += regenRate;
            if (currentHP > maxHP) currentHP = maxHP;
            UpdateBar();
        }
    }

    public void TakeDamage(float damage)
    {
        if (currentHP <= 0) return;

        if (playerMove != null && playerMove.IsDashing())
        {
            return;
        }

        currentHP -= damage;
        if (currentHP < 0) currentHP = 0;

        UpdateBar();

    }

    public void Heal(float amount)
    {
        currentHP += amount;
        if (currentHP > maxHP) currentHP = maxHP;
        UpdateBar();
    }

    private void UpdateBar()
    {
        if (barImage != null)
        {
            barImage.fillAmount = currentHP / maxHP;
        }
    }

    public float GetCurrentHP()
    {
        return currentHP;
    }

    private void Die()
    {

        GetComponent<PlayerMove>().enabled = false;

        if (anim != null)
        {
            anim.SetTrigger("Death");
        }
 
    }
}