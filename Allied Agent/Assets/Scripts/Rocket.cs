
using UnityEngine;
using UnityEngine.Events;

public class Rocket : MonoBehaviour
{
    public TrailRenderer trail;
    public UnityEvent onRocketShoot;
    public float shootForce;
    public float rocketMass;

    private bool hasFired = false;
    private Rigidbody rb;
    private Transform tf;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        tf = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && Input.GetButton("Fire2") && !hasFired)
        {
            hasFired = true;
            Fire();
        }
        
    }

    void FixedUpdate()
    {
        if (hasFired)
        {
            PointNoseToMovementVector();
        }
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

    private void Fire()
    {
        onRocketShoot?.Invoke();
        tf.SetParent(null);
        rb.isKinematic = false;
        rb.mass = rocketMass;
        rb.AddForce(transform.TransformDirection(-Vector3.right).normalized * shootForce, ForceMode.Impulse);
        trail.emitting = true;
    }

    private void PointNoseToMovementVector()
    {
        Vector3 direction = rb.velocity.normalized;
        tf.right = -direction;
    }
}
