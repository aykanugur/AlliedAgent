using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{

    private bool moving = false;
    private NavMeshAgent agent;
    public float maxDistance = 200f;
    public float maxSpeed = 30;
    public float actionDistance = 10;
    public float sphereCastRadius = 10f;
    // Start is called before the first frame update
    void Start()
    {
        agent = transform.parent.gameObject.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Enemy follows player
        RaycastHit hitInfo;
        bool hit = Physics.Raycast(transform.position,transform.forward,out hitInfo,maxDistance);
        Debug.DrawRay(transform.position,new Vector3(0,0,maxDistance),Color.red);
        if (hit)
        {
            MoveToPlayer(hitInfo); 
        }
        else
        {
            agent.isStopped = true;
            moving = false;
        }

    }

    void MoveToPlayer(RaycastHit hitInfo)
    {
        moving = true;
        if (hitInfo.transform.gameObject.tag.Equals("Player"))
        {
            transform.parent.transform.LookAt(hitInfo.transform.gameObject.transform.position);
            if(hitInfo.distance < actionDistance)
            {
                agent.speed = agent.speed > maxSpeed ? maxSpeed : agent.speed + (0.01f * hitInfo.distance);
            }
            else
            {
                agent.speed = 1;
            }
            agent.destination = hitInfo.transform.gameObject.transform.position;

            
        }

        moving = false;
    }

    bool IsMoving()
    {
        return moving;
    }
}
