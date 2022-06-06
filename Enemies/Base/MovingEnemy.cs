using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MovingEnemy : Enemy
{
    public NavMeshAgent agent;
    public float minSpeed, maxSpeed;
    protected bool NearPlayer;
    public bool canMove = true;

    public override void Update()
    {
        base.Update();
        NearPlayer = (player.currentSubdungeon == subDungeon);
        Move();
    }

    public virtual void Move()
    {
        if (!agent)
        {
            agent = gameObject.AddComponent<NavMeshAgent>();
            agent.speed = Random.Range(minSpeed, maxSpeed);
        }
    }

    public override void Die()
    {
        base.Die();
        StopAllCoroutines();
        canMove = false;
        GameObjectExtensions.ResetTag(gameObject);
        gameObject.layer = LayerMask.NameToLayer("IgnoreProjectile");
    }
}
