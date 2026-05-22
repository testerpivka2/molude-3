using UnityEngine;
using UnityEngine.Events;

public class BossHealth : MonoBehaviour
{
    [Header("Stats")]
    public float maxHealth = 300f;
    public float currentHealth;

    [Header("Phase 2 Threshold (0-1)")]
    [Range(0f, 1f)]
    public float phase2At = 0.5f; // 50% HP

    [HideInInspector] public UnityEvent onPhase2;
    [HideInInspector] public UnityEvent onDeath;

    private bool phase2Triggered = false;
    private bool isDead = false;
    private Animator animator;

    void Awake()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();

        if (onPhase2 == null) onPhase2 = new UnityEvent();
        if (onDeath == null) onDeath = new UnityEvent();
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0f);

        animator.SetTrigger("Hurt"); // → HeavyBandit_Hurt

        if (!phase2Triggered && currentHealth / maxHealth <= phase2At)
        {
            phase2Triggered = true;
            onPhase2.Invoke();
        }

        if (currentHealth <= 0f)
            Die();
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        animator.SetTrigger("Death"); // → HeavyBandit_Death
        onDeath.Invoke();

        GetComponent<BossController>().enabled = false;
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;

        Destroy(gameObject, 2.5f);
    }

    // Вызывай в UI для полоски здоровья
    public float HealthPercent => currentHealth / maxHealth;
}
