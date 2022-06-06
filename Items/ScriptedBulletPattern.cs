using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptedBulletPattern : BulletPattern
{
    [SerializeField]
    private GameObject projectile;

    public override void Fire()
    {
        base.Fire();

        List<Vector3> positions = GetLocationsForFire();
        foreach(Vector3 v in positions)
        {
            GameObject g = Instantiate(projectile, v, Quaternion.LookRotation(v - transform.position, Vector3.up));
        }
    }

    public virtual List<Vector3> GetLocationsForFire()
    {
        List<Vector3> ret = new List<Vector3>();

        //For the basic model, just have one projectile towards the player. Override this for cooler effects!
        ret.Add(transform.position + transform.forward);
        return ret;
    }
}
