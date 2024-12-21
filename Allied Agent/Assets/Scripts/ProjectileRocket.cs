
using UnityEngine;

public class ProjectileRocket : MonoBehaviour
{
    public TrailRenderer fireTrail;
    public TrailRenderer smokeTrail;
    public float fireTrailTime = 0.25f;
    
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
        Explode();
    }

    private void Explode()
    {
        //call DrawCircle here to deal splash damage
        
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
