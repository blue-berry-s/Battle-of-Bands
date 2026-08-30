using UnityEngine;

public class EnemyIdle : EnemyState
{
    Transform player;
    float randDist;
    float currentTargetX;
    bool atPos = false;
    bool isMoving = false;
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
        atPos = false;
        isMoving = false;
    }

    public override void ExitState()
    {
        base.ExitState();
        atPos = false;
        isMoving = false;
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();

        if (enemy.isWithinAttackingDistance)
        {
            enemy.StateMachine.ChangeState(enemy.attackState);
            return;
        }

        if (!atPos)
        {
            // 1. Check if we arrived at the target position
            if (Mathf.Abs(enemy.transform.position.x - currentTargetX) < 0.2f)
            {
                atPos = true;
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
        if (atPos) {
            // Re-trigger idle to recalculate fresh dance parameters next frame
            enemy.StateMachine.ChangeState(enemy.idleState);
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
        if (randFloat < 0.33f || (player.position.x + 7f) > 8f)
        {
            Debug.Log("Neutral Dance -> Block State");
            enemy.StateMachine.ChangeState(enemy.blockState);
        }
        // B: Reposition to an farther distance
        else if (randFloat >= 0.33f && randFloat < 0.66f)
        {
            Debug.Log("Neutral Dance -> Move Back");
            // Command the movement immediately
            //TODO: instead of just moveEnemy - we should just create a new state (enemyRetreat)
            enemy.moveEnemy(new Vector2(player.position.x + 2f, 0f));
        }
        // C: Escape backwards
        else
        {
            Debug.Log("Neutral Dance -> Jump Back");
            enemy.jumpBack();

        }
    }


}
