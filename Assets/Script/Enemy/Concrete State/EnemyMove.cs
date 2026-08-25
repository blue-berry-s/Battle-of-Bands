using UnityEngine;

public class EnemyMove : EnemyState
{


    private Transform player;
    public EnemyMove(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    public override void AnimationTriggerEvent(Enemy.AnimationTriggerType triggerType)
    {
        base.AnimationTriggerEvent(triggerType);
    }

    public override void EnterState()
    {
        base.EnterState();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        //calculate how far player is and move player towards enemy
        Vector2 moveDirection = (player.position - enemy.transform.position).normalized;
        enemy.moveEnemy(moveDirection);

        if (enemy.isWithinAttackingDistance) {
            enemy.StateMachine.ChangeState(enemy.attackState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

}
