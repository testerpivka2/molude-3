using UnityEngine;

public class SamuraiAttackState : IEnemyState
{
    private SamuraiEnemy enemy;
    private float timer;
    
    private float attackDuration = 0.65f; 

    public SamuraiAttackState(SamuraiEnemy enemyContext)
    {
        this.enemy = enemyContext;
    }

    public void Enter()
    {
        enemy.Anim.Play("Attack", -1, 0f); 
        timer = 0f; 
    }

    public void Update()
    {
        timer += Time.deltaTime;

        if (timer >= attackDuration)
        {
            enemy.lastAttackTime = Time.time; 

            int randomChoice = Random.Range(0, 100);

            if (randomChoice < 50) 
            {
                enemy.ChangeState(new SamuraiRetreatState(enemy));
            }
            else 
            {
                enemy.ChangeState(new SamuraiIdleState(enemy));
            }
        }
    }

    public void Exit()
    {
    }
}