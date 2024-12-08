using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    
    public float maxDistance = 200f;
    public float maxSpeed = 30;
    public float actionDistance = 10;
   
    [SerializeField] private float minDistance = 0.2f;
    [SerializeField] private float minSpeed = 2.6f;
    [SerializeField] private float range = 10;
    [SerializeField] private double gunRange = 10;

    
    
    private NavMeshAgent agent;
    
    
    private Vector3 oldPosition;
    private Vector3 patrolDest;
    
    private bool following; //is enemy following the player
    private bool covering = true;
    
    [SerializeField] private bool damaging; //If player shooting to us

    private GameObject player;
    private GameObject[] coverPlaces;
    
    // Start is called before the first frame update
    void Start()
    {
        coverPlaces = GameObject.FindGameObjectsWithTag("cover");
        agent = transform.gameObject.GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        following = false;
        damaging = false;
        covering = false;
        oldPosition = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Stop cover if player is too far away
        double enemyDistance = Vector3.Distance(transform.position, player.transform.position);
        if (enemyDistance > gunRange)
        {
            covering = false;
        }
        else if (!covering && damaging)
        {
            //Cover if not covering and player is shooting to us
            Cover();
            
        }
        
        if(covering && agent.pathStatus == NavMeshPathStatus.PathComplete) //If we completed covering, look at player
            transform.LookAt(player.transform.position);
        if (!covering)
        {
            //Enemy follows player
            RaycastHit hitInfo;

            bool hit = Physics.CapsuleCast(transform.position, transform.forward,
                agent.radius, transform.forward, out hitInfo, maxDistance);
            //Debug.DrawRay(transform.position,new Vector3(0,0,maxDistance),Color.red);
            if (hit)
            {
                MoveToPlayer(hitInfo);
            }
            else
            {
                StopFollow();
            }
            
            if(!agent.hasPath && !following)
                Patrol();

            
        }



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
                    //TODO: Stop 
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
    void Cover()
    {
        if (!covering)
        {


            double distanceToCover = -1;
            Vector3 coverPos = Vector3.zero;

            //Look for suitiable places to shoot
            foreach (GameObject place in coverPlaces)
            {
                double temp = Vector3.Distance(player.transform.position, place.transform.position);
                if (temp < gunRange)
                {
                    distanceToCover = temp;
                    coverPos = place.transform.position;
                    break;
                }
            }

            //If found a place, go to there
            if (distanceToCover > 0)
            {
                //If we have a cover object which the gun can shoot go to there
                Debug.Log("Cover");
                agent.SetDestination(coverPos);
                covering = true;
            }
        }



    }

  

    void Patrol()
    {
        if (!following && !agent.hasPath && !covering) //If enemy currently does not moving, make a patrol
        {
                Vector3 randomDirection = Random.insideUnitSphere * range;
                randomDirection += transform.position;
                NavMeshHit hit;
                NavMesh.SamplePosition(randomDirection, out hit, range, 1);
                patrolDest = hit.position;
                agent.SetDestination(patrolDest);

        }
        
    }
    
    void SetDamaging(bool damaging)
    {
        this.damaging = damaging;
    }
    
}
