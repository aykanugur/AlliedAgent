
using UnityEngine;

public class BulletCollision : MonoBehaviour
{
    
    
    public bool isTracer = false;
    public TrailRenderer trail;
    public float tracerTimeout = 3.5f;
    
    private Rigidbody rb;
    private Transform tf;
    
    // Start is called before the first frame update
    void Start()
    {
        rb=GetComponent<Rigidbody>();
        tf=GetComponent<Transform>();
        trail.emitting=isTracer;
    }

    // Update is called once per frame
    void Update()
    {
        tracerTimeout-=Time.deltaTime;
        trail.emitting=isTracer && tracerTimeout>=0;
    }

    void FixedUpdate()
    {
        PointNoseToMovementVector();
    }

    
    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Enemy":

                break;
            
            case "Reflective":
                ReflectBullet(other.gameObject.transform.forward);
                break;
            case "DoNotDestroy":
                break;
            
            default:
                Destroy(this.gameObject);
                break;
                
        }
    }

    private void ReflectBullet(Vector3 normal)
    {
        Vector3 direction = rb.velocity.normalized;
        direction = Vector3.Reflect(direction, normal);
        rb.velocity = direction * rb.velocity.magnitude;
    }
    
    private void PointNoseToMovementVector()
    {
        Vector3 direction = rb.velocity.normalized;
        tf.up = direction;
    }

    public void MakeTracer()
    {
        this.isTracer=true;
    }

    public void makeNonTracer()
    {
        this.isTracer=false;
    }
}

