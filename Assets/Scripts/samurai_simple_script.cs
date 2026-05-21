using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 5f;
    public Transform groundCheck;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator anim;
    private bool isGrounded;
    private bool isBlocking;

    // АТАКИ
    private int comboStep = 0;
    private float lastAttackTime = 0f;
    private bool isAttacking = false;

    [Header("Attack Settings")]
    [SerializeField] private float comboDelay = 1.0f;
    private float[] attackDurations = new float[] { 0.333f, 0.533f };

    // CIRCLE ATTACK (вместо хитбокса)
    [Header("Circle Attack")]
    public Transform attackPoint;
    public float attackRange = 0.8f;
    public LayerMask enemyLayer;
    private int[] attackDamage = { 10, 15 };

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");

        if (!isAttacking)
        {
            Vector2 move = new Vector2(moveX, 0);
            move.Normalize();
            rb.linearVelocity = new Vector2(move.x * speed, rb.linearVelocity.y);
        }

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        if (Input.GetKeyDown(KeyCode.W) && isGrounded && !isAttacking)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        isBlocking = Input.GetKey(KeyCode.LeftShift);
        anim.SetBool("isBlocking", isBlocking);

        bool isRunning = moveX != 0 && !isAttacking;
        anim.SetBool("isRunning", isRunning);

        if (moveX != 0 && !isAttacking)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * Mathf.Sign(moveX);
            transform.localScale = scale;
        }

        if (Input.GetMouseButtonDown(0))
        {
            TryAttack();
        }

        if (!isAttacking && Time.time - lastAttackTime > comboDelay && comboStep > 0)
        {
            ResetCombo();
        }
    }

    void TryAttack()
    {
        if (!isAttacking)
        {
            StartAttack(0);
        }
        else if (isAttacking && comboStep < 1 && Time.time - lastAttackTime < comboDelay)
        {
            CancelInvoke(nameof(EndAttack));
            StartAttack(comboStep + 1);
        }
    }

    void StartAttack(int step)
    {
        isAttacking = true;
        comboStep = step;
        lastAttackTime = Time.time;
        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);

        anim.SetInteger("AttackType", comboStep);
        anim.SetTrigger("Attack");

        Invoke(nameof(EndAttack), attackDurations[step]);
    }

    void EndAttack()
    {
        isAttacking = false;
        if (comboStep >= 1) ResetCombo();
    }

    void ResetCombo()
    {
        comboStep = 0;
        isAttacking = false;
        anim.SetInteger("AttackType", 0);
    }

    public void DealDamage()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                Enemy enemyScript = enemy.GetComponent<Enemy>();
                if (enemyScript != null)
                {
                    enemyScript.TakeDamage(attackDamage[comboStep]);
                    Debug.Log($"Урон {attackDamage[comboStep]} по {enemy.name}");
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }
    }
}