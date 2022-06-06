using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float speed;
    public Vector3 axis;

    private void Start()
    {
        axis = Vector3.ClampMagnitude(axis, 1);
    }

    void Update()
    {
        transform.eulerAngles += (axis * speed * Time.deltaTime);
    }
}
