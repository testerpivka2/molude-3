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

    private int comboStep = 0;
    private float lastAttackTime = 0f;
    private bool isAttacking = false;

    [Header("Attack Settings")]
    [SerializeField] private float comboDelay = 1.0f;
    private float[] attackDurations = new float[] { 0.333f, 0.533f };

    [Header("Circle Attack")]
    public Transform attackPoint;
    public float attackRange = 0.8f;
    public LayerMask enemyLayer;
    private int[] attackDamage = { 10, 15 };

    [Header("Dash Settings")]
    public float dashForce = 15f;
    public float dashDuration = 0.333f;
    public float dashCooldown = 1f;

    private bool isDashing = false;
    private bool canDash = true;
    private float dashTimer = 0f;
    private float dashCooldownTimer = 0f;
    private Vector2 dashDirection;

    private int defaultLayer;
    private int dashLayerInt;
    private int playerLayerInt;
    private int enemyLayerInt;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        
        defaultLayer = gameObject.layer;
        dashLayerInt = LayerMask.NameToLayer("Dash");
        playerLayerInt = LayerMask.NameToLayer("Player");
        enemyLayerInt = LayerMask.NameToLayer("Enemy");

        
        if (dashLayerInt == -1) dashLayerInt = 0;
        if (playerLayerInt == -1) playerLayerInt = 0;
        if (enemyLayerInt == -1) enemyLayerInt = 0;
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space) && canDash && !isAttacking && !isDashing)
        {
            StartDash();
        }

        if (isDashing)
        {
            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0)
            {
                EndDash();
            }
        }

        if (!canDash)
        {
            dashCooldownTimer -= Time.deltaTime;
            if (dashCooldownTimer <= 0)
            {
                canDash = true;
            }
        }

        float moveX = Input.GetAxisRaw("Horizontal");

        if (!isAttacking && !isDashing)
        {
            Vector2 move = new Vector2(moveX, 0);
            move.Normalize();
            rb.linearVelocity = new Vector2(move.x * speed, rb.linearVelocity.y);
        }

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);


        if (Input.GetKeyDown(KeyCode.W) && isGrounded && !isAttacking && !isDashing)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        isBlocking = Input.GetKey(KeyCode.LeftShift);
        anim.SetBool("isBlocking", isBlocking);

        bool isRunning = moveX != 0 && !isAttacking && !isDashing;
        anim.SetBool("isRunning", isRunning);

        if (moveX != 0 && !isAttacking && !isDashing)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * Mathf.Sign(moveX);
            transform.localScale = scale;
        }

        if (Input.GetMouseButtonDown(0) && !isDashing)
        {
            TryAttack();
        }

        if (!isAttacking && Time.time - lastAttackTime > comboDelay && comboStep > 0)
        {
            ResetCombo();
        }
    }

    void StartDash()
    {
        isDashing = true;
        canDash = false;
        dashCooldownTimer = dashCooldown;
        dashTimer = dashDuration;

        float dashDir = transform.localScale.x > 0 ? 1f : -1f;
        dashDirection = new Vector2(dashDir, 0);

        gameObject.layer = dashLayerInt;

        Physics2D.IgnoreLayerCollision(playerLayerInt, enemyLayerInt, true);

        rb.linearVelocity = dashDirection * dashForce;

    }

    void EndDash()
    {
        isDashing = false;

        gameObject.layer = defaultLayer;

        Physics2D.IgnoreLayerCollision(playerLayerInt, enemyLayerInt, false);

        rb.linearVelocity = Vector2.zero;
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