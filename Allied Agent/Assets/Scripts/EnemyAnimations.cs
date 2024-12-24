using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;
using Random = UnityEngine.Random;

public class EnemyAnimations : MonoBehaviour
{

    public Animator animator;
    public EnemyAI enemyAI;
    public NavMeshAgent agent;
    public Collider collider,collider2;

    public float hp;
    private SkinnedMeshRenderer _meshRenderer;
    public float red;
    public GameObject ammo;
    private bool firstTimeCry;
    public RigBuilder rigBuilder;
    private bool firstTime;
    public GameObject gun;
    public RandomSpeech randomSpeech;
    public GameObject text;
    
    

    private void Start()
    {
        firstTimeCry = true;
        firstTime = true;
        
        _meshRenderer = transform.GetChild(0).GetComponent<SkinnedMeshRenderer>();
    }

    public void Die()
    {
        text.SetActive(false);
        Instantiate(ammo, transform.position, ammo.transform.rotation);
        collider2.enabled = false;
        collider.enabled = false;
        enemyAI.enabled = false;
        randomSpeech.enabled = false;
        animator.SetTrigger("Death");
        GetComponent<EnemyAnimations>().enabled = false;
        agent.enabled = false;
    }
    public void Pray()
    {
        randomSpeech.idle = false;
        randomSpeech.pray = true;
        rigBuilder.enabled = false;
        gun.SetActive(false);
        animator.SetTrigger("WarCrime");
        Instantiate(ammo, transform.position, ammo.transform.rotation);
        enemyAI.enabled = false;
        agent.enabled = false;
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
        if (firstTimeCry && hp < 30)
        {
            int chance = Random.Range(0, 10);
            if (chance == 5)
            {
                Pray();
                firstTimeCry = false;
            }
        }
        if (hp != 100 && firstTime)
        {
            firstTime = false;
            enemyAI.SetDamaging(true);
            randomSpeech.idle = false;
            randomSpeech.pray = false;
        }
        if (hp > 0)
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
