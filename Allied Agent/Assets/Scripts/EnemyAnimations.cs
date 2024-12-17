using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimations : MonoBehaviour
{

    public Animator animator;

    public float hp;
    private SkinnedMeshRenderer _meshRenderer;
    public float red;

    private void Start()
    {
        _meshRenderer = transform.GetChild(0).GetComponent<SkinnedMeshRenderer>();
    }

    public void Die()
    {
        animator.SetTrigger("Death");
    }
    public void Pray()
    {
        animator.SetTrigger("WarCrime");
    }

    public void Aim(bool status)
    {
        animator.SetBool("Aim",status);
    }

    public void ChangeMovement(float velocity)
    {
        animator.SetFloat("MovementDirectionX",velocity);
    }

    public void Cover(bool status)
    {
        animator.SetBool("Cover",status);
    }

    private void Update()
    {

        if (hp >= 0)
        {
            red = 1f - (hp / 100f);
        }
        else
        {
            Die();
        }
        _meshRenderer.materials[1].color = new Vector4(red, 0, 0, 1);
        
    }
}
