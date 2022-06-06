using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingingAxe : Trap
{

    Quaternion start, end;

    [SerializeField, Range(0.0f, 360f)]
    private float angle = 90.0f;

    [SerializeField]
    private float minSpeed = 1.5f, maxSpeed = 4.0f;

    private float speed = 2.0f;

    private float startTime = 0.0f;

    protected override void Start()
    {
        base.Start();
        speed = Random.Range(minSpeed, maxSpeed);
        start = RotatePendulum(angle);
        end = RotatePendulum(-angle);
    }

    public void FixedUpdate()
    {
        startTime += Time.fixedDeltaTime;
        transform.rotation = Quaternion.Lerp(start, end, (Mathf.Sin(startTime * speed + Mathf.PI / 2) + 1.0f) / 2.0f);
    }

    Quaternion RotatePendulum(float angle)
    {
        Quaternion pendulumRotation = transform.rotation;
        float angleZ = pendulumRotation.eulerAngles.z + angle;

        if(angleZ > 180)
        {
            angleZ -= 360;
        }
        else if(angleZ < -180)
        {
            angleZ += 360;
        }

        pendulumRotation.eulerAngles = new Vector3(pendulumRotation.eulerAngles.x, pendulumRotation.eulerAngles.y, angleZ);
        return pendulumRotation;
    }
}
