
using UnityEngine;
using UnityEngine.Events;

public class Rocket : MonoBehaviour
{
    public TrailRenderer fireTrail;
    public TrailRenderer smokeTrail;
    public UnityEvent onRocketShoot;
    public float shootForce;
    public float rocketMass;
    public float fireTrailTime = 0.25f;

    private bool hasFired = false;
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
        if (hasFired)
        {
            ActivateTrail();
        }
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
