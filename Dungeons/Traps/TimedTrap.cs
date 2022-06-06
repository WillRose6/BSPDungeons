using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedTrap : Trap
{
    public float lifetime;
    public float timeBetweenPulses;

    protected override void Start()
    {
        base.Start();
        StartCoroutine(performTrap());
    }

    private IEnumerator performTrap()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenPulses);
            OnActivate();
            yield return new WaitForSeconds(lifetime);
            OnDeactivate();

        }
    }
}
