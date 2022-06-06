using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTrap : Trap
{
    public float minRotateSpeed, maxRotateSpeed;
    private float RotateSpeed = 0f;
    public GameObject rotatingPart;
    public GameObject[] colliders;
    public AudioSource source;

    protected override void Start()
    {
        base.Start();
        RotateSpeed = Random.Range(minRotateSpeed, maxRotateSpeed);
    }

    protected override void OnActivate()
    {
        base.OnActivate();
        ParticleSystem[] particles = GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem ps in particles)
        {
            ps.Play();
        }

        foreach(GameObject g in colliders)
        {
            g.SetActive(true);
        }
        source.Play();
    }

    protected override void OnDeactivate()
    {
        base.OnDeactivate();
        ParticleSystem[] particles = GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem ps in particles)
        {
            ps.Stop();
        }

        foreach (GameObject g in colliders)
        {
            g.SetActive(false);
        }
    }

    public void Update()
    {
        if (active)
        {
            rotatingPart.transform.Rotate(Vector3.up, RotateSpeed);
        }
    }
}
