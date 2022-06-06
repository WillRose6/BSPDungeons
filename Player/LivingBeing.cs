using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingBeing : MonoBehaviour
{
    [Header("Variables"), SerializeField]
    protected float StartHealth = 1000f;
    [SerializeField]
    protected float currentHealth = 0f;
    protected bool Dead = false;

    protected virtual void Start()
    {
        Dead = false;
        SetHealth(StartHealth);
    }

    public virtual void ChangeHealth(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, StartHealth);

    }

    public virtual void SetHealth(float amount)
    {
        currentHealth = amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, StartHealth);
    }

    public virtual void TakeDamage(float amount)
    {
        ChangeHealth(-amount);

        //Add other code to change things such as UI etc here
        if(currentHealth <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        Dead = true;
    }
}
