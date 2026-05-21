using UnityEngine;

public class SamuraiIdleState : IEnemyState
{
    private SamuraiEnemy enemy;
    private float timer;

    public SamuraiIdleState(SamuraiEnemy enemyCont)
    {
        this.enemy = enemyCont;
    }

    public void Enter()
    {
        enemy.Anim.Play("Idle");
        timer = 0f;
    }

    public void Update()
    {
        timer += Time.deltaTime;

       
        if (timer < 0.5f) return;

        float distanceToPlayer = Vector2.Distance(enemy.transform.position, enemy.player.position);

        if (distanceToPlayer <= enemy.attackRange)
        {
            enemy.ChangeState(new SamuraiAttackState(enemy));
        }
        else if (distanceToPlayer <= enemy.attackRange * 3f)
        {
            enemy.ChangeState(new SamuraiChaseState(enemy));
        }
    }

    public void Exit()
    {
    }
}