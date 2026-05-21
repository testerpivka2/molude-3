using UnityEngine;

public class SamuraiRetreatState : IEnemyState
{
    private SamuraiEnemy enemy;
    private float timer;
    private float retreatDuration = 1.5f; 

    public SamuraiRetreatState(SamuraiEnemy enemyContext)
    {
        this.enemy = enemyContext;
    }

    public void Enter()
    {
        enemy.Anim.Play("Run");
        timer = 0f;
    }

    public void Update()
    {
        timer += Time.deltaTime;

        Vector2 retreatDirection = (enemy.transform.position - enemy.player.position).normalized;

        enemy.transform.position = Vector2.MoveTowards(
            enemy.transform.position,
            (Vector2)enemy.transform.position + retreatDirection,
            enemy.retreatSpeed * Time.deltaTime
        );

        FlipTowardsPlayer(); 

        
        if (timer >= retreatDuration)
        {
            enemy.ChangeState(new SamuraiIdleState(enemy));
        }
    }

    public void Exit()
    {
    }

    private void FlipTowardsPlayer()
    {
        Vector3 scale = enemy.transform.localScale;
        if (enemy.player.position.x > enemy.transform.position.x)
            scale.x = Mathf.Abs(scale.x);
        else if (enemy.player.position.x < enemy.transform.position.x)
            scale.x = -Mathf.Abs(scale.x);

        enemy.transform.localScale = scale;
    }
}