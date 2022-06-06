using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapCollisionTriggerEventExecute : MonoBehaviour
{
    public Trap trap;

    public void OnTriggerEnter(Collider other)
    {
        trap.DealDamage(other);
    }
}
