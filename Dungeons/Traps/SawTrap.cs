using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawTrap : Trap
{
    [SerializeField]
    private float minSawMoveSpeed, maxSawMoveSpeed;
    private float SawMoveSpeed;
    [SerializeField]
    private float SawRotationSpeed = 15f;
    [SerializeField]
    private float SawRange;
    private Vector3 startPos = Vector3.zero;

    protected override void Start()
    {
        base.Start();
        startPos = transform.position;
        SawMoveSpeed = Random.Range(minSawMoveSpeed, maxSawMoveSpeed);
    }

    public void Update()
    {
        transform.position = startPos + new Vector3(SawRange * Mathf.Sin(Time.time * SawMoveSpeed), 0f, 0f);
        transform.Rotate(0, 0, SawRotationSpeed);
    }
}
