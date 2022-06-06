using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaEnemy : RandomMovementEnemy
{
    public GameObject PS_Death;
    public Transform headBone;
    public GameObject mesh;
    public AudioClip shurikenEffect;
    public AudioClip[] shoutEffect;
    public AudioSource shoutSource;
    private Rigidbody rb;

    protected override void UpdateCanMove()
    {
        base.UpdateCanMove();

        if (CheckFrozen())
        {
            if (rb) { rb.velocity = Vector3.zero; }
        }
    }

    public override void PerformAttack()
    {
        base.PerformAttack();
        anim.SetTrigger("Attack");
    }

    public void ThrowShuriken()
    {
        bulletPattern.Fire();
        SFXPlayer.instance.PlayEffect(shurikenEffect,1);
    }

    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);
    }

    public override void Die()
    {
        base.Die();
    }

    public void CreateDeathParticles()
    {
        GameObject g = Instantiate(PS_Death, headBone.transform.position + new Vector3(0, 1, 0), Quaternion.identity);
        Destroy(g, 3f);
    }

    public override void DestroyCharacter()
    {
        base.DestroyCharacter();
    }

    public void WakeUp()
    {
        if (Random.value > 0.8f)
        {
            shoutSource.PlayOneShot(shoutEffect[Random.Range(0, shoutEffect.Length)]);
        }
    }

    private IEnumerator Shout()
    {
        yield return new WaitForSeconds(Random.Range(0f, 1.5f));
    }

    public override void Update()
    {
        base.Update();
        anim.SetBool("NearPlayer", NearPlayer);
    }

    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody>();
    }
}
