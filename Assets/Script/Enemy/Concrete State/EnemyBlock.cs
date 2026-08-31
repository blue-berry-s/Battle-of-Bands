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
        enemy.stopAttacking();
        enemy.enemyAnimator.SetBool("isMoving", false);
        enemy.doneBlocking = false;
        enemy.performBlockWait();
    }

    public override void ExitState()
    {
        base.ExitState();
        enemy.enemyAnimator.SetBool("isBlocking", false);
        enemy.doneBlocking = false;
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();


        if (enemy.doneBlocking) {
            if (!player.GetBool("isAttacking") && !player.GetBool("isKicking"))
            {
                enemy.StateMachine.ChangeState(enemy.attackState);
            }
            else
            {
                enemy.StateMachine.ChangeState(enemy.idleState);
            }
        }
       
       
        
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
