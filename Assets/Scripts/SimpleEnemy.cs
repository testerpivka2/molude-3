using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    public float damage = 10f;
    public int maxHealth = 30;

    [Header("Movement")]
    public float moveSpeed = 2f;

    protected int currentHealth;
    protected Animator anim;
    protected Rigidbody2D rb;
    protected bool isDead = false;
    protected bool isMoving = false;

    [Header("Effects")]
    public GameObject deathEffect;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
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

    protected virtual void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
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

        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
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
}