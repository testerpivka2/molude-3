using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    public float damage = 10f;
    public int maxHealth = 30;
    public float moveSpeed = 2f;
    public float agroRange = 5f;

    [Header("Attack")]
    public float attackCooldown = 1f;
    public float attackRange = 1.5f;

    protected int currentHealth;
    protected Animator anim;
    protected Rigidbody2D rb;
    protected Transform player;
    protected bool isDead = false;
    protected bool isMoving = false;
    protected bool canAttack = true;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    protected virtual void Update()
    {
        if (isDead || player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            StopMoving();
            TryAttack();
        }
        else if (distanceToPlayer <= agroRange)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            Move(direction);
        }
        else
        {
            StopMoving();
        }
    }

    protected virtual void Move(Vector2 direction)
    {
        if (isDead) return;

        rb.linearVelocity = new Vector2(direction.x * moveSpeed, rb.linearVelocity.y);
        isMoving = direction.x != 0;

        if (anim != null)
        {
            anim.SetBool("IsWalking", isMoving);
        }

        if (direction.x != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * Mathf.Sign(direction.x);
            transform.localScale = scale;
        }
    }

    protected virtual void StopMoving()
    {
        if (isDead) return;

        rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        isMoving = false;

        if (anim != null)
        {
            anim.SetBool("IsWalking", false);
        }
    }

    protected virtual void TryAttack()
    {
        if (!canAttack) return;

        canAttack = false;

        if (anim != null)
        {
            anim.SetTrigger("Attack");
        }

        Invoke(nameof(ResetAttack), attackCooldown);
    }

    protected virtual void ResetAttack()
    {
        canAttack = true;
    }

    protected virtual void DealDamage()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);
        if (distance <= attackRange)
        {
            BarScript playerHealth = player.GetComponent<BarScript>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }
    }

    public virtual void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;

        if (anim != null)
        {
            anim.SetTrigger("TakeHit");
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
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

        Destroy(gameObject, 2f);
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isDead)
        {
            BarScript playerHealth = collision.gameObject.GetComponent<BarScript>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }
    }

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, agroRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}