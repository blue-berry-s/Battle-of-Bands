using UnityEngine;

public class EnemyIdle : EnemyState
{
    //this should observe/calculate the next best move (based on reaction time)
    private Vector3 playerPos;
    
    
    public EnemyIdle(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
    }

    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }

    public override void EnterState()
    {
        base.EnterState();
        if (enemy.isWithinAttackingDistance)
        {
            enemy.StateMachine.ChangeState(enemy.attackState);
        }
        else
        {
            enemy.StateMachine.ChangeState(enemy.moveState);
        }
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
        else
        {
            enemy.StateMachine.ChangeState(enemy.moveState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
