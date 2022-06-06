using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasePlayerEnemy : AnimatedEnemy
{
    public enum ChaseType
    {
        Continuous,
        Burst,
    }

    [SerializeField]
    private ChaseType chaseType;

    public override void Move()
    {
        base.Move();
        if (NearPlayer)
        {
            if (chaseType == ChaseType.Continuous)
            {
                agent.SetDestination(player.transform.position);
            }
        }
    }
}
