using System.Collections;
using UnityEngine;

/// <summary>
/// Босс на основе HeavyBandit (Bandits - Pixel Art).
/// Анимации: Idle, Run, Jump, Attack, CombatIdle, Hurt, Death, Recover
/// Добавь на объект вместо (или рядом с отключённым) Bandit.cs
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BossHealth))]
public class BossController : MonoBehaviour
{
    // ─── Ссылки ────────────────────────────────────────────────
    [Header("References")]
    public Transform player;
    public Transform groundCheck;       // пустой дочерний объект у ног
    public LayerMask groundLayer;

    [Header("Melee Hitbox")]
    public Transform attackPoint;       // пустой дочерний объект у руки
    public float attackRadius = 0.8f;
    public LayerMask playerLayer;

    // ─── Движение ──────────────────────────────────────────────
    [Header("Movement")]
    public float moveSpeed = 3f;
    public float phase2Speed = 5f;
    public float jumpForce = 7.5f;
    public float stopDistance = 1.5f;   // дистанция для атаки в ближнем бою

    // ─── Атаки ─────────────────────────────────────────────────
    [Header("Attack Settings")]
    public float meleeDamage = 20f;
    public float attackCooldown = 2f;
    public float phase2Cooldown = 1.2f;
    public float jumpAttackChance = 0.3f; // 30% шанс прыжка к игроку

    // ─── Состояние ─────────────────────────────────────────────
    enum State { Idle, Chase, Attack, JumpAttack, Hurt, Dead }
    State state = State.Idle;

    private Rigidbody2D rb;
    private Animator anim;
    private BossHealth health;

    private float attackTimer = 0f;
    private bool isPhase2 = false;
    private bool isGrounded = false;
    private bool facingRight = false;

    // ─── Animator параметры ────────────────────────────────────
    // Проверь в своём AnimController что параметры называются именно так.
    // Если нет — переименуй здесь.
    const string ANIM_RUN          = "Run";         // bool
    const string ANIM_COMBAT_IDLE  = "CombatIdle";  // bool
    const string ANIM_ATTACK       = "Attack";      // trigger
    const string ANIM_JUMP         = "Jump";        // trigger
    const string ANIM_HURT         = "Hurt";        // trigger
    const string ANIM_DEATH        = "Death";       // trigger

    // ═══════════════════════════════════════════════════════════
    void Awake()
    {
        rb    = GetComponent<Rigidbody2D>();
        anim  = GetComponent<Animator>();
        health = GetComponent<BossHealth>();

        health.onPhase2.AddListener(EnterPhase2);
        health.onDeath.AddListener(() => state = State.Dead);
    }

    void Update()
    {
        if (state == State.Dead) return;
        if (player == null) return;

        CheckGrounded();
        attackTimer -= Time.deltaTime;

        switch (state)
        {
            case State.Idle:
                TransitionToChase();
                break;

            case State.Chase:
                ChasePlayer();
                TryStartAttack();
                break;

            // Attack и JumpAttack управляются корутинами
        }
    }

    // ─── Логика движения ───────────────────────────────────────

    void TransitionToChase()
    {
        state = State.Chase;
        SetAnimBool(ANIM_COMBAT_IDLE, true);
    }

    void ChasePlayer()
    {
        float dist = DistToPlayer();

        if (dist > stopDistance)
        {
            MoveTowards(player.position);
            SetAnimBool(ANIM_RUN, true);
            SetAnimBool(ANIM_COMBAT_IDLE, false);
        }
        else
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            SetAnimBool(ANIM_RUN, false);
            SetAnimBool(ANIM_COMBAT_IDLE, true);
        }
    }

    void MoveTowards(Vector3 target)
    {
        float dir = target.x > transform.position.x ? 1f : -1f;
        rb.linearVelocity = new Vector2(dir * CurrentSpeed(), rb.linearVelocity.y);
        FlipTowards(dir);
    }

    // ─── Атаки ─────────────────────────────────────────────────

    void TryStartAttack()
    {
        if (attackTimer > 0f) return;
        if (DistToPlayer() > stopDistance + 0.5f) return;

        attackTimer = isPhase2 ? phase2Cooldown : attackCooldown;

        // Фаза 2: иногда прыгает
        if (isPhase2 && !isGrounded == false && Random.value < jumpAttackChance)
            StartCoroutine(JumpAttack());
        else
            StartCoroutine(MeleeAttack());
    }

    IEnumerator MeleeAttack()
    {
        state = State.Attack;
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

        SetAnimBool(ANIM_RUN, false);
        SetAnimBool(ANIM_COMBAT_IDLE, false);
        anim.SetTrigger(ANIM_ATTACK);

        yield return new WaitForSeconds(0.4f); // ждём замаха

        // Наносим урон всем игрокам в радиусе
        DealMeleeDamage();

        yield return new WaitForSeconds(0.4f); // ждём окончания анимации

        if (state != State.Dead)
            state = State.Chase;
    }

    IEnumerator JumpAttack()
    {
        state = State.JumpAttack;
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        SetAnimBool(ANIM_RUN, false);

        anim.SetTrigger(ANIM_JUMP);

        // Прыжок в сторону игрока
        float dir = player.position.x > transform.position.x ? 1f : -1f;
        rb.AddForce(new Vector2(dir * 3f, jumpForce), ForceMode2D.Impulse);
        FlipTowards(dir);

        yield return new WaitForSeconds(0.3f);

        // Ждём приземления
        yield return new WaitUntil(() => isGrounded);

        // Атака после приземления
        anim.SetTrigger(ANIM_ATTACK);
        yield return new WaitForSeconds(0.3f);
        DealMeleeDamage();
        yield return new WaitForSeconds(0.3f);

        // Recover анимация (если есть параметр)
        // anim.SetTrigger("Recover");
        // yield return new WaitForSeconds(0.5f);

        if (state != State.Dead)
            state = State.Chase;
    }

    void DealMeleeDamage()
    {
        if (attackPoint == null) return;

        Collider2D[] hits = Physics2D.OverlapCircleAll(
            attackPoint.position, attackRadius, playerLayer);

        foreach (var hit in hits)
        {
            // Замени PlayerHealth на имя своего скрипта здоровья игрока
            hit.GetComponent<BarScript>()?.TakeDamage(meleeDamage);
        }
    }

    // ─── Фаза 2 ────────────────────────────────────────────────

    void EnterPhase2()
    {
        isPhase2 = true;
        Debug.Log("Boss: Phase 2!");
        // Можно добавить эффект (вспышка, звук, частицы)
    }

    // ─── Утилиты ───────────────────────────────────────────────

    void CheckGrounded()
    {
        if (groundCheck == null) return;
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.15f, groundLayer);
    }

    float DistToPlayer() =>
        Mathf.Abs(transform.position.x - player.position.x);

    float CurrentSpeed() => isPhase2 ? phase2Speed : moveSpeed;

    void FlipTowards(float dir)
    {
        bool shouldFaceRight = dir > 0;
        if (shouldFaceRight == facingRight) return;
        facingRight = shouldFaceRight;
        Vector3 s = transform.localScale;
        s.x *= -1;
        transform.localScale = s;
    }

    void SetAnimBool(string param, bool val)
    {
        anim.SetBool(param, val);
    }

    // Рисуем hitbox атаки в редакторе
    void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
        }
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, 0.15f);
        }
    }
}
