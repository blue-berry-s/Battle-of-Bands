using UnityEngine;

public class EnemyDead : EnemyState
{
    public EnemyDead(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
    }

    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }

    public override void EnterState()
    {
        base.EnterState();
        enemy.disableAllAnimator();
        enemy.disableEnemy();
        enemy.enemyAnimator.SetBool("isDead", true);
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        enemy.enemyRigidBody.linearVelocityX = 0;
        base.FrameUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
