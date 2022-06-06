using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleBulletPattern : BulletPattern
{
    public ParticleSystem bulletPattern;

    [Header("Particle system events etc")]
    private ParticleCollisionEvent[] CollisionEvents;

    public override void Start()
    {
        if (!bulletPattern)
        {
            bulletPattern = GetComponent<ParticleSystem>();
        }
        bulletPattern.Stop();
        var main = bulletPattern.main;
        main.duration = Random.Range(minFireRate, maxFireRate);
        bulletPattern.Play();
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<Player>().TakeDamage(Damage);
        }
    }
}
