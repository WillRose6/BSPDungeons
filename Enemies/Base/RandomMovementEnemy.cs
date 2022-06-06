using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMovementEnemy : AnimatedEnemy
{
    private Vector3 randomPosition;
    public float minTimeBeforeShooting, maxTimeBeforeShooting;

    public override void Move()
    {
        base.Move();
        if (NearPlayer)
        {
            if (canMove)
            {
                if (randomPosition == Vector3.zero || Vector3.Distance(transform.position, randomPosition) < 1.0f)
                {
                    randomPosition = subDungeon.GetRandomValidPointInRoom(false).transform.position;
                    agent.SetDestination(randomPosition);
                }
            }
        }
    }

    public override IEnumerator Fire()
    {
        while (true)
        {
            if (NearPlayer && canMove)
            {
                if (agent)
                {
                    yield return new WaitForSeconds(Random.Range(minTimeBeforeShooting, maxTimeBeforeShooting));
                }

                PerformAttack();
                yield return new WaitForSeconds(Random.Range(bulletPattern.minFireRate, bulletPattern.maxFireRate));
            }
            else
            {
                yield return null;
            }
        }
    }

    public override void PerformAttack()
    {
        base.PerformAttack();
    }

    public override void Update()
    {
        base.Update();
        agent.isStopped = !canMove;
    }
}
