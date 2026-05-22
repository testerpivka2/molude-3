using UnityEngine;

public class SlimeDemon : Enemy
{
    [Header("Boss Settings")]
    public float jumpForce = 8f;
    public float jumpCooldown = 3f;

    private bool canJump = true;
    private float originalMoveSpeed;

    protected override void Start()
    {
        base.Start();

        maxHealth = 100;
        currentHealth = maxHealth;
        damage = 20f;
        originalMoveSpeed = moveSpeed;
        moveSpeed = 3f;
        agroRange = 10f;
        attackRange = 2f;
        attackCooldown = 1.5f;
    }

    protected override void Update()
    {
        base.Update();

        if (isDead || player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (canJump && distanceToPlayer < 3f && IsGrounded())
        {
            StartCoroutine(BossJump());
        }

        if (currentHealth <= maxHealth / 2)
        {
            moveSpeed = originalMoveSpeed * 1.5f;
        }
    }

    private bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.2f);
        return hit.collider != null;
    }

    private System.Collections.IEnumerator BossJump()
    {
        canJump = false;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        yield return new WaitForSeconds(jumpCooldown);
        canJump = true;
    }

    protected override void DealDamage()
    {
        base.DealDamage();
    }

    protected override void Die()
    {
        if (isDead) return;

        isDead = true;
        StopMoving();

        if (anim != null)
        {
            anim.SetTrigger("Death");
        }


        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
            col.enabled = false;

        Destroy(gameObject, 3f);
    }
}