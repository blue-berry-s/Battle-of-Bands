using UnityEngine;

public class EnemyDisabled : EnemyState
{
    public EnemyDisabled(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
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
        
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        enemy.enemyRigidBody.linearVelocityX = 0;
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
