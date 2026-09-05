using UnityEngine;

public class EnemyRetreat : EnemyState
{
    Transform player;
    Transform enemyPos;
    float goalDest;
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
        goalDest = player.position.x + 7f;
        enemy.moveEnemy(new Vector2(goalDest, 0f));
        enemy.stopAttacking();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        if ((enemyPos.position.x - goalDest) <= 1 || enemyPos.position.x >= enemy.outOfBounds)
        {
            enemy.StateMachine.ChangeState(enemy.idleState);
        }
        else {
            // 2. Calculate the direction vector (-1 for Left, 1 for Right)
            //float directionX = player.position.x + 7f > enemy.transform.position.x ? 1f : -1f;

            // 3. Pass the true DIRECTION vector to the movement function
            //enemy.moveEnemy(new Vector2(goalDest, 0));

            //I kind of like the dash back right now
            enemy.moveEnemy(new Vector2(goalDest, 0f));
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
