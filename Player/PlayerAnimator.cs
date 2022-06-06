using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [Header("References")]
    public Animator anim;

    private void Start()
    {
        anim.SetLayerWeight(1, 0);
    }

    public void UpdateMovementAnimation(Vector3 moveVelocity)
    {
        float forward = Vector3.Dot(moveVelocity.normalized, transform.forward.normalized);
        float side = Vector3.Dot(moveVelocity.normalized, transform.right.normalized);

        Vector3 inputs = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));

        if (inputs.magnitude > 0)
        {
            anim.SetFloat("Forward", Mathf.Lerp(anim.GetFloat("Forward"), forward, Time.deltaTime * 10f));
            anim.SetFloat("Sideways", Mathf.Lerp(anim.GetFloat("Sideways"), -side, Time.deltaTime * 10f));
        }
        else
        {
            anim.SetFloat("Forward", Mathf.Lerp(anim.GetFloat("Forward"), 0f, Time.deltaTime * 10f));
            anim.SetFloat("Sideways", Mathf.Lerp(anim.GetFloat("Sideways"), 0f, Time.deltaTime * 10f));
        }
    }

    public void Attack()
    {
        anim.SetLayerWeight(1, 1);
        anim.SetBool("Attack", true);
    }

    public void FinishAttacking()
    {
        anim.SetBool("Attack", false);
    }

    public void Die()
    {
        anim.SetTrigger("Die");
    }
}
