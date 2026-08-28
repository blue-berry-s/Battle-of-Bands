using UnityEngine;

public class EnemyBlock : EnemyState
{
    private Animator player;
    public EnemyBlock(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
    }

    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }

    public override void EnterState()
    {
        base.EnterState();
        enemy.enemyAnimator.SetBool("isBlocking", true);
    }

    public override void ExitState()
    {
        base.ExitState();
        enemy.enemyAnimator.SetBool("isBlocking", false);
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        if (!player.GetBool("isAttacking") && !player.GetBool("isKicking")) {
            enemy.StateMachine.ChangeState(enemy.attackState);
        }
       
        
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
