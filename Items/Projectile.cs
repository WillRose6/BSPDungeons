using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Rigidbody rb;
    public float knockbackForce;
    public float force;
    public float damage;
    public int MaxBounces = 0;
    private int bounces;
    private Vector3 lastVelocity;
    public GameObject impactEffect;
    public List<string> ignoreTags;
    public AudioClip[] impactSFX;
    public float chanceOfPlayingImpactSFX;

    private void Start()
    {
        bounces = MaxBounces;
        rb.AddForce(transform.forward * force, ForceMode.Impulse);
    }

    public void Update()
    {
        lastVelocity = rb.velocity;
    }

    public virtual void OnCollisionEnter(Collision collision)
    {
        if (!ignoreTags.Contains(collision.gameObject.tag))
        {
            Rigidbody otherRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            if (otherRigidbody)
            {
                otherRigidbody.AddForce(transform.forward * knockbackForce);
            }

            if (impactSFX.Length > 0)
            {
                if (Random.value <= chanceOfPlayingImpactSFX)
                {
                    SFXPlayer.instance.PlayEffect(impactSFX[Random.Range(0, impactSFX.Length)], 3);
                }
            }

            if (bounces > 0)
            {
                bounces--;
                Bounce(collision);
            }
            else
            {
                if (impactEffect != null)
                {
                    GameObject g = Instantiate(impactEffect, transform.position, Quaternion.identity);
                    Destroy(g, 3);
                }
                Destroy(gameObject);
            }
        }
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if (!ignoreTags.Contains(other.gameObject.tag))
        {
            Rigidbody otherRigidbody = other.gameObject.GetComponent<Rigidbody>();
            if (otherRigidbody)
            {
                otherRigidbody.AddForce(transform.forward * knockbackForce);
            }

            if (impactSFX.Length > 0)
            {
                if (Random.value <= chanceOfPlayingImpactSFX)
                {
                    SFXPlayer.instance.PlayEffect(impactSFX[Random.Range(0, impactSFX.Length)], 3);
                }
            }

            if (impactEffect != null)
            {
                GameObject g = Instantiate(impactEffect, transform.position, Quaternion.identity);
                Destroy(g, 3);
            }
            Destroy(gameObject);
        }
    }

    public void Bounce(Collision collision)
    {
        var speed = lastVelocity.magnitude;
        var direction = Vector3.Reflect(lastVelocity.normalized, collision.contacts[0].normal);
        rb.velocity = direction * speed;
    }
}
