
using UnityEngine;

public class ProjectileRocket : MonoBehaviour
{
    public TrailRenderer fireTrail;
    public TrailRenderer smokeTrail;
    public float fireTrailTime = 0.25f;
    public float damage = 100f;
    public GameObject explosion;
    public float explosionRadius = 10f;
    public float explosionForce = 70f;
    public LayerMask layers;
    
    private Rigidbody rb;
    private Transform tf;
    private float timeSinceFired = 0f;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        tf = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        ActivateTrail();
    }

    void FixedUpdate()
    {
        PointNoseToMovementVector();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("DoNotDestroy"))
        {
            
        }
        
        Explode();
    }

    private void Explode()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius,layers);
        foreach (Collider hit in hitColliders)
        {
            float distance = Vector3.Distance(transform.position, hit.transform.position);
            if (hit.gameObject != null)
            {
                switch (hit.gameObject.tag)
                {
                    case "Player":
                        //damage the player here
                        // ahhh dont hurt him he is cute :( 
                        break;
                
                    case "Enemy":
                        hit.gameObject.GetComponent<EnemyAnimations>().hp -= damage/(distance * distance);
                        
                        break;
                
                    case "cover":
                        if (hit.gameObject.GetComponent<Cover>() != null)
                        {
                            hit.gameObject.GetComponent<Cover>().hp = hit.gameObject.GetComponent<Cover>().hp - damage/(distance * distance);
                            hit.GetComponent<Cover>().CheckHp();
                            
                        }
                        break;
                }
            }
        }
        
        Instantiate(explosion, transform.position, transform.rotation);
        //add rocket sound effect here
        Destroy(this.gameObject);
    }

    private void PointNoseToMovementVector()
    {
        Vector3 direction = rb.velocity.normalized;
        tf.right = -direction;
    }

    private void ActivateTrail()
    {
        if (timeSinceFired <= fireTrailTime)
        {
            fireTrail.emitting = true;
            timeSinceFired += Time.deltaTime;
        }
        else
        {
            fireTrail.emitting = false;
            smokeTrail.emitting = true;
            rb.useGravity = true;
        }
    }
}
