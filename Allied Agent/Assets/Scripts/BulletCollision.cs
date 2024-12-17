
using UnityEngine;

public class BulletCollision : MonoBehaviour
{
    
    
    public bool isTracer = false;
    public TrailRenderer trail;
    public float tracerTimeout = 3.5f;
    public GameObject blood;
    private Rigidbody rb;
    private Transform tf;
    public float hp;
    
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
                Debug.Log("enemy hit");
                GameObject child = Instantiate(blood, other.transform);
                var transform1 = transform;
                child.transform.position = new Vector3(other.transform.position.x,transform1.position.y,transform1.position.z);
                child.transform.eulerAngles = transform1.eulerAngles;
                other.gameObject.GetComponent<EnemyAnimations>().hp -= hp; 
                Destroy(this.gameObject);
                break;
            
            case "Reflective":
                ReflectBullet(other.gameObject.transform.forward);
                break;
            
            case  "cover":
                if (other.gameObject.GetComponent<Cover>() != null)
                {
                    other.gameObject.GetComponent<Cover>().hp = other.gameObject.GetComponent<Cover>().hp - hp;
                    other.GetComponent<Cover>().CheckHp();
                    Destroy(this.gameObject);
                }
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

