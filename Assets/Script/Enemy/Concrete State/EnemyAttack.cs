using UnityEngine;
using System.Collections;

public class EnemyAttack : EnemyState
{
    public EnemyAttack(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
    }

    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }

    public override void EnterState()
    {
        base.EnterState();
        enemy.enemyAnimator.SetBool("isAttacking", true);
        //Debug.Log("I'M ATTACKING NOW");
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        if (!enemy.isWithinAttackingDistance) {
            enemy.StateMachine.ChangeState(enemy.idleState);
        }
        else
        {
            enemy.attackPlayer();
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    
}
