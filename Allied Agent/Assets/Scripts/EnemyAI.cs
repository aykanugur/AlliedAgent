using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SocialPlatforms;

public class EnemyAI : MonoBehaviour
{
    
    public float maxDistance = 200f;
    public float maxSpeed = 30;
    public float actionDistance = 10;
    [SerializeField] private float minDistance = 0.2f;
    [SerializeField] private float minCoverDistance = 3f;
    [SerializeField] private float minSpeed = 2.6f;
    [SerializeField] private float range;

    private NavMeshAgent agent;
    
    
    private Vector3 oldPosition;
    private Vector3 patrolDest;
    
    private bool following; //is enemy following the player

    private GameObject[] coverPlaces;
    
    // Start is called before the first frame update
    void Start()
    {
        coverPlaces = GameObject.FindGameObjectsWithTag("cover");
        agent = transform.gameObject.GetComponent<NavMeshAgent>();
        following = false;
        oldPosition = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Enemy follows player
        RaycastHit hitInfo;
        
        bool hit = Physics.CapsuleCast(transform.position,transform.forward,
            agent.radius,transform.forward,out hitInfo,maxDistance);
        //Debug.DrawRay(transform.position,new Vector3(0,0,maxDistance),Color.red);
        if (hit)
        {
            MoveToPlayer(hitInfo); 
        }
        else
        {
            StopFollow();
        }
        
        Patrol();

    }

    void MoveToPlayer(RaycastHit hitInfo)
    {
        if (hitInfo.transform.gameObject.tag.Equals("Player"))
        {
            following = true;

            
            transform.LookAt(hitInfo.transform.gameObject.transform.position);


            if (Vector3.Distance(oldPosition, transform.position) > maxDistance) //Stop following if the enemy goes beyond range
            {
                StopFollow();
                agent.SetDestination(oldPosition);
                return;
            }
            //Action speeding
            
            if(hitInfo.distance < actionDistance)
            {
                agent.speed = agent.speed > maxSpeed ? maxSpeed : agent.speed + (0.001f * hitInfo.distance);
                if (hitInfo.distance < minDistance)
                {
                    //Stop in a distance
                    //TODO: Cover and shoot, following is still active
                }

                
            }
            else
            {
                agent.speed = minSpeed;
            }
            agent.destination = hitInfo.transform.gameObject.transform.position;

            
            


        }
        else
        {
            StopFollow();
        }
       
    }

    //Return to the home
    void StopFollow()
    {
        if(!agent.hasPath)
            agent.SetDestination(oldPosition);
        following = false;
        

    }

    //TODO: Cover and shoot
    bool Cover()
    {
        foreach (GameObject place in coverPlaces)
        {
            float distance = Vector3.Distance(transform.position, place.transform.position);
            if (distance < minCoverDistance)
            {
                following = false;
                agent.SetDestination(place.transform.position);
                return true;
            }
        }

        return false;
    }

    void Patrol()
    {
        if (!following && !agent.hasPath) //If enemy currently does not moving, make a patrol
        {
                Vector3 randomDirection = Random.insideUnitSphere * range;
                randomDirection += transform.position;
                NavMeshHit hit;
                NavMesh.SamplePosition(randomDirection, out hit, range, 1);
                patrolDest = hit.position;
                agent.SetDestination(patrolDest);

        }
        
    }
    
}
