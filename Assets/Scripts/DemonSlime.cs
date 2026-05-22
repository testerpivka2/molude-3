using UnityEngine;

public class SlimeDemon : Enemy
{

    private float originalMoveSpeed;

    [Header("Boss Attack")]
    public Transform attackPoint;
    public LayerMask playerLayer;
    public int bossDamage = 15;

    protected override void Start() 
    {
        base.Start();

        maxHealth = 100;
        currentHealth = maxHealth;
        damage = 20f;
        originalMoveSpeed = moveSpeed;
        moveSpeed = 3f;
        agroRange = 10f;
        attackRange = 5f;
        attackCooldown = 1.5f;
    }

    protected override void Update()
    {
        base.Update();

        if (isDead || player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);


        if (currentHealth <= maxHealth / 2)
        {
            moveSpeed = originalMoveSpeed * 2f;
        }
    }

    private bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.2f);
        return hit.collider != null;
    }


    protected override void DealDamage()
    {
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);

        foreach (Collider2D player in hitPlayers)
        {
            if (player.CompareTag("Player"))
            {
                BarScript playerHealth = player.GetComponent<BarScript>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(bossDamage);
                }
            }
        }
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

        Destroy(gameObject, 3f);
    }
}