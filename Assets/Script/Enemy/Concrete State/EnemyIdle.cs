using UnityEngine;

public class EnemyIdle : EnemyState
{
    Transform player;
    float randDist;
    float currentTargetX;
    public EnemyIdle(Enemy enemy, EnemyStateMachine enemyStateMachine) : base(enemy, enemyStateMachine)
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
        //chose a random point within player range
        randDist = Random.Range(3, 6);
        currentTargetX = player.position.x + randDist;
        enemy.stopAttacking();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
        if (currentTargetX >= enemy.outOfBounds)
        {
            enemy.StateMachine.ChangeState(enemy.idleState);
        }
        // 1. Check if we arrived at the target position
        else if (Mathf.Abs(enemy.transform.position.x - currentTargetX) < 0.2f)
        {
            MakeDanceDecision();
        }
        else
        {
            // 2. Calculate the direction vector (-1 for Left, 1 for Right)
            float directionX = currentTargetX > enemy.transform.position.x ? 1f : -1f;

            // 3. Pass the true DIRECTION vector to the movement function
            enemy.moveEnemy(new Vector2(directionX, 0));
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    private void MakeDanceDecision()
    {
        float randFloat = Random.Range(0f, 1f);

        // A: Transition to block state if roll succeeds or player bounds check forces it
        if (randFloat < 0.33f || (player.position.x + 7f) > enemy.outOfBounds)
        {
            //Debug.Log("Neutral Dance -> Block State");
            enemy.StateMachine.ChangeState(enemy.blockState);
        }
        // B: Reposition to an farther distance
        else if (randFloat >= 0.33f && randFloat < 0.66f)
        {
            //Debug.Log("Neutral Dance -> Move Back");
            // Command the movement immediately

            enemy.StateMachine.ChangeState(enemy.retreatState);
        }
        // C: Escape backwards
        else
        {
            //Debug.Log("Neutral Dance -> Jump Back");
            enemy.jumpBack();
            enemy.StateMachine.ChangeState(enemy.jumpState);

        }
    }


}
