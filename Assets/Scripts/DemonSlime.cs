using UnityEngine;

public class SlimeDemon : Enemy
{
    [Header("Slime Demon Settings")]
    public float agroRange = 20f;

    private Transform player;

    protected override void Start()
    {
        base.Start();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

    }

    protected override void Move(Vector2 direction)
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
            scale.x = Mathf.Abs(scale.x) * -Mathf.Sign(direction.x); 
            transform.localScale = scale;
        }
    }

    void Update()
    {
        if (isDead) return;
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= agroRange)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            Move(direction); 
        }
        else
        {
            StopMoving();
        }
    }

}