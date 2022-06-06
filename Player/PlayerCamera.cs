using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public GameObject playerObject;
    public float Movespeed;
    public Vector3 offset;
    private float ShakeAmount = 0f;
    [SerializeField]
    private bool FollowYAxis;

    private void FixedUpdate()
    {
        Vector3 shake = Random.insideUnitSphere;
        Vector3 position = Vector3.Lerp(transform.position + (shake * ShakeAmount), playerObject.transform.position + offset, Time.fixedDeltaTime * Movespeed);
        float y = transform.position.y;
        if (FollowYAxis)
        {
            y = position.y;
        }
        transform.position = new Vector3(position.x, y, position.z);
    }

    public IEnumerator Shake(float amount, float time)
    {
        ShakeAmount = amount;
        yield return new WaitForSeconds(time);
        ShakeAmount = 0f;
    }
}
