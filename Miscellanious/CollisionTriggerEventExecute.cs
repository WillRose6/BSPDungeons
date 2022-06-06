using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollisionTriggerEventExecute : MonoBehaviour
{
    public UnityEvent events;
    public string requiredTag;

    public virtual void OnCollisionEnter(Collision collision)
    {
        if (requiredTag != string.Empty)
        {
            if (collision.gameObject.tag != requiredTag)
            {
                return;
            }
        }
        events.Invoke();
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if (requiredTag != string.Empty)
        {
            if (other.gameObject.tag != requiredTag)
            {
                return;
            }
        }
        events.Invoke();
    }
}
