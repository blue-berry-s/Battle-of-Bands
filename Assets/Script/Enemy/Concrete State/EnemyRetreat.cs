using UnityEngine;

public class EnemyRetreat : EnemyState
{
    Transform player;
    Transform enemyPos;
    public EnemyRetreat(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        enemyPos = enemy.gameObject.GetComponent<Transform>();
    }

    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }

    public override void EnterState()
    {
        base.EnterState();
        enemy.moveEnemy(new Vector2(player.position.x + 7f, 0f));
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        if (enemy.isWithinAttackingDistance)
        {
            enemy.StateMachine.ChangeState(enemy.attackState);
        }
        else if (enemyPos.position.x >= player.position.x + 7f || enemyPos.position.x >= 8.5f)
        {
            enemy.StateMachine.ChangeState(enemy.idleState);
        }
        else {
            enemy.moveEnemy(new Vector2(player.position.x + 7f, 0f));
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
