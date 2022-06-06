using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostEnemy : ChasePlayerEnemy
{
    private float SpookRange;
    private bool spooking = false;

    [SerializeField]
    private float SpookWaitTime = 1f;
    [SerializeField]
    private float SpookForce = 5f;
    [SerializeField]
    private float Damage = 100f;
    private Rigidbody rb;

    protected override void Start()
    {
        base.Start();
        SpookRange = agent.stoppingDistance;
        rb = GetComponent<Rigidbody>();
        if (!anim)
        {
            anim = GetComponent<Animator>();
        }
        StartCoroutine(LateStart());
    }

    private IEnumerator LateStart()
    {
        yield return null;
        agent.height = 0.1f;
        agent.radius = 0.1f;
    }

    public override void Move()
    {
        if (Vector3.Distance(transform.position, player.transform.position) > SpookRange)
        {
            base.Move();
        }
        else
        {
            if (canRotate)
            {
                Quaternion toPlayer = Quaternion.LookRotation(player.transform.position - transform.position, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, toPlayer, Time.deltaTime * rotationSpeed);
            }
            if (!spooking)
            {
                if (!Dead)
                {
                    StartCoroutine(SpookPlayer());
                }
            }
        }
    }

    private IEnumerator SpookPlayer()
    {
        if(Vector3.Distance(transform.position, player.transform.position) < SpookRange){

            anim.SetBool("Attacking", true);
            spooking = true;
            yield return new WaitForSeconds(SpookWaitTime);
            if (Vector3.Distance(transform.position, player.transform.position) < SpookRange)
            {
                transform.rotation = Quaternion.LookRotation(player.transform.position - transform.position, Vector3.up);
                rb.AddForce((player.transform.position - transform.position).normalized * SpookForce, ForceMode.Impulse);
                canRotate = false;
                yield return new WaitForSeconds(1f);
            }
            FinishSpooking();
        }
        else {
            yield break;
        }
    }

    private IEnumerator SlowDown()
    {
        while(rb.velocity.magnitude > 0.1f)
        {
            rb.velocity *= 0.95f;
            yield return null;
        }
    }

    private void FinishSpooking()
    {
        spooking = false;
        StartCoroutine(SlowDown());
        canRotate = true;
        anim.SetBool("Attacking", false);
    }

    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);
        FinishSpooking();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (spooking)
        {
            if (other.tag == "Player")
            {
                other.GetComponent<Player>().TakeDamage(Damage);
            }

            if (other.tag == "Wall")
            {
                FinishSpooking();
            }
        }
    }

    protected override void UpdateCanMove()
    {
        if (!spooking)
        {
            base.UpdateCanMove();
        }
    }
}
