using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField]
    protected float Damage;
    protected bool active;
    public List<string> damageTags;
    public AudioClip soundEffect;
    public SubFloor subDungeon;

    public virtual void OnTriggerEnter(Collider other)
    {
        DealDamage(other);
    }

    public virtual void DealDamage(Collider other)
    {
        if (damageTags.Contains(other.tag))
        {
            other.gameObject.GetComponent<LivingBeing>().TakeDamage(Damage);
        }
    }

    protected virtual void Start()
    {
        OnDeactivate();
        if (damageTags.Count == 0) {
            damageTags.Add("Player");
        }
    }

    public virtual void ToggleActive(float lifetime)
    {
        StartCoroutine(Activate(lifetime));
    }

    protected virtual void OnActivate()
    {
        active = true;
    }

    protected virtual void OnDeactivate()
    {
        active = false;
    }

    protected virtual IEnumerator Activate(float lifetime)
    {
        OnActivate();
        yield return new WaitForSeconds(lifetime);
        OnDeactivate();
    }
}
