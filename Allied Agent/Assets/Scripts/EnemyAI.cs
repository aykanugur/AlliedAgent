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
    [SerializeField] private float minSpeed = 2.6f;

    private NavMeshAgent agent;
    [SerializeField] private float range;
    private Vector3 oldPosition;
    
    private bool following; //is enemy following the player
    
    // Start is called before the first frame update
    void Start()
    {
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

    }

    void MoveToPlayer(RaycastHit hitInfo)
    {
        if (hitInfo.transform.gameObject.tag.Equals("Player"))
        {
            Debug.Log("Follow player");
            following = true;
            agent.isStopped = false;

            
            transform.LookAt(hitInfo.transform.gameObject.transform.position);

            if(hitInfo.distance < actionDistance)
            {
                agent.speed = agent.speed > maxSpeed ? maxSpeed : agent.speed + (0.001f * hitInfo.distance);
                if(hitInfo.distance < minDistance)
                    StopFollow();
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

    void StopFollow()
    {
        agent.destination = oldPosition;
        following = false;

    }
    
}
