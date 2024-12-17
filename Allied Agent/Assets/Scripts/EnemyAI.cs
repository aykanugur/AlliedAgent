using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyAI : MonoBehaviour
{
    
    public float maxDistance = 200f;
    public float maxSpeed = 30;
    public float actionDistance = 10;
   
    [SerializeField] private float minSpeed = 2.6f;
    [SerializeField] private float range = 7; //Range to patrol
    [SerializeField] private double gunRange = 2;

    [SerializeField] private CapsuleCollider coverCollider; //Collider active in cover
    [SerializeField] private CapsuleCollider normalCollider; //Collider active normal

    
    
    private NavMeshAgent agent;
    
    
    private Vector3 oldPosition;
    private Vector3 patrolDest;
    
    private bool following; //is enemy following the player
    private bool covering = true;
    private bool shooting;
    
    [SerializeField] private bool damaging; //If player shooting to us

    private GameObject player;
    private GameObject[] coverPlaces;

    private EnemyAnimations animations; //Enemy animations script
    private EnemyGun gun;
    
    // Start is called before the first frame update
    void Start()
    {
        animations = GetComponent<EnemyAnimations>();
        gun = GetComponentInChildren<EnemyGun>();
        if (gun.GetRange() > 0.1) //Set gun range
            gunRange = gun.GetRange();
        
        coverPlaces = GameObject.FindGameObjectsWithTag("cover");
        agent = transform.gameObject.GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player"); //Our player
        following = false; //Enemy is following player
        damaging = false; //Player is shooting to us
        covering = false;
        oldPosition = transform.position; //Spawn position
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //TODO: automatize Shooting=false
        if (agent.isStopped)
        {
            agent.isStopped = false;
        }

        if (Vector3.Distance(player.transform.position, transform.position) > gunRange)
        {
            shooting = false;
            animations.Aim(false);
            animations.ChangeMovement(maxSpeed);
        }

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
            //TODO: Stop covering and shoot in intervals
            
        }
        
        if(covering && agent.pathStatus == NavMeshPathStatus.PathComplete) //If we completed covering, look at player
            transform.LookAt(player.transform.position);
        if (!covering)
        {
            if (agent.hasPath && !shooting)
            {
                animations.ChangeMovement(maxSpeed);
            }
            
            DisableCoverCollider();
            
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
            following = true; //States: following / covering

            transform.LookAt(hitInfo.transform.gameObject.transform.position); //Look to the player
            


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
                if (hitInfo.distance < gunRange) //Stop and shoot in gunRange
                {
                    transform.LookAt(player.transform);
                    agent.isStopped = true;
                    Shoot();
                }
                else
                {
                    shooting = false;
                }

                
            }
            else
            {
                agent.speed = minSpeed;
                shooting = false;
            }
                agent.destination = hitInfo.transform.gameObject.transform.position; //Follow player every frame
            
        }
        else
        {
            StopFollow();
            
        }
       
    }

    //Return to the home
    void StopFollow()
    {
        shooting = false;
        if(!agent.hasPath)
            agent.SetDestination(oldPosition);
        following = false;
        

    }
    
    void Cover()
    {
        
        if (!covering)
        {
            double distanceToCover = -1;
            Vector3 coverPos = Vector3.zero;

            //Look for suitable places to shoot
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

            //If found a place, go there
            if (distanceToCover > 0)
            {
                //If we have a cover object which the gun can shoot go there
                agent.SetDestination(coverPos); //We can't use hasPath bcz player can shoot while moving
                covering = true;
                normalCollider.enabled = false;
                coverCollider.enabled = true; 
                //Cover animation
                animations.Cover(true);
            }
            
            
        }
    }

  

    void Patrol()
    {
        if (!following && !agent.hasPath && !covering) //If enemy currently does not moving, make a patrol
        {
            shooting = false;
                Vector3 randomDirection = Random.insideUnitSphere * range;
                randomDirection += transform.position;
                NavMeshHit hit;
                NavMesh.SamplePosition(randomDirection, out hit, range, 1);
                patrolDest = hit.position;
                
                agent.SetDestination(patrolDest);

        }
        
    }

    void Shoot()
    {
        if (!shooting)
        {
            shooting = true;
           
        }

        if (!gun.getCoolDown())
        {
            
            if (gun.currentCapacity > 1)
            {
                animations.Aim(true);
                gun.Shoot();
            }
            else
            {
                //Debug.Log("reload");
                animations.Aim(false);
                animations.ChangeMovement(0);
                gun.Reload();
            }
        }




    }
    
    void SetDamaging(bool damaging)
    {
        this.damaging = damaging;
    }

    void DisableCoverCollider()
    {
        if (coverCollider.enabled)
        {
            
            coverCollider.enabled = false;
            normalCollider.enabled = true;
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.tag.Equals("Player"))
        {
            transform.LookAt(other.gameObject.transform.position); //If player is too close, look at them
        }
    }
}
