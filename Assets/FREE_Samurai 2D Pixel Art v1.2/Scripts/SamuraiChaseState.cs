using UnityEngine;

public class SamuraiChaseState : IEnemyState
{
    private SamuraiEnemy enemy;

    public SamuraiChaseState(SamuraiEnemy enemyContext)
    {
        this.enemy = enemyContext;
    }

    public void Enter()
    {
        enemy.Anim.Play("Run");
    }

    public void Update()
    {
        enemy.transform.position = Vector2.MoveTowards(
            enemy.transform.position,
            enemy.player.position,
            enemy.speed * Time.deltaTime
        );

        FlipTowardsPlayer();

        float distance = Vector2.Distance(enemy.transform.position, enemy.player.position);

        if (distance <= enemy.attackRange)
        {
            enemy.ChangeState(new SamuraiAttackState(enemy));
        }
        else if (distance > enemy.attackRange * 3f)
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